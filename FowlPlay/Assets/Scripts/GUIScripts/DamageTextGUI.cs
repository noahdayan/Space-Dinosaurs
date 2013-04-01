using UnityEngine;
using System.Collections;

public class DamageTextGUI : MonoBehaviour {
	
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
	
	IEnumerator showDamageText(string message) {
		//GetComponent<TextMesh>().transform.rotation = cam.transform.rotation;
		GetComponent<TextMesh>().text = message;
		//GetComponent<TextMesh>().transform.rotation = cam.transform.rotation;
		if (message[0] == '+')
			GetComponent<TextMesh>().renderer.material.color = Color.green;
		else if (message[0] == '-')
			GetComponent<TextMesh>().renderer.material.color = Color.red;
		yield return new WaitForSeconds(2.0f);
		GetComponent<TextMesh>().text = "";
	}
}
