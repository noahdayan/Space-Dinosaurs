using UnityEngine;
using System.Collections;

public class DamageTextGUI : MonoBehaviour {
	
	GameObject cam;
	
	// Use this for initialization
	void Start () {
		cam = GameObject.Find("Main Camera");
	}
	
	// Update is called once per frame
	void Update () {
	GetComponent<TextMesh>().transform.rotation = cam.transform.rotation;
	}
	
	IEnumerator showDamageText(string message) {
		//GetComponent<TextMesh>().transform.rotation = cam.transform.rotation;
		GetComponent<TextMesh>().text = message;
		yield return new WaitForSeconds(2.0f);
		GetComponent<TextMesh>().text = "";
	}
}
