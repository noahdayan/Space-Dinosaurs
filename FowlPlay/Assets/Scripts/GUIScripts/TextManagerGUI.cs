using UnityEngine;
using System.Collections;

public class TextManagerGUI : MonoBehaviour {
	
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
	gameObject.transform.rotation = cam.transform.rotation;
	}
}
