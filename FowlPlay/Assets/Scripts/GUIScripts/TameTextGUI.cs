using UnityEngine;
using System.Collections;

public class TameTextGUI : MonoBehaviour {

	GameObject cam;
	public static bool isMiniGame = false;
	
	// Use this for initialization
	void Start () {
		if (!isMiniGame)
			cam = GameObject.Find("Main Camera");
		else
			cam = GameObject.Find("Mini Game Camera");
	}
	
	// Update is called once per frame
	void Update () {
	GetComponent<TextMesh>().transform.rotation = cam.transform.rotation;
	}
	
	IEnumerator showTameText(string message)
	{
		//GetComponent<TextMesh>().transform.rotation = cam.transform.rotation;
		if (message[0] == '+')
			GetComponent<TextMesh>().renderer.material.color = Color.cyan;
		else if (message[0] == '-')
			GetComponent<TextMesh>().renderer.material.color = Color.magenta;
		GetComponent<TextMesh>().text = message;
		yield return new WaitForSeconds(2.0f);
		GetComponent<TextMesh>().text = "";
	}
	
}
