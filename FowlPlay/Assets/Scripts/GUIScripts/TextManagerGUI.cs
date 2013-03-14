using UnityEngine;
using System.Collections;

public class TextManagerGUI : MonoBehaviour {
	
	GameObject cam;
	
	// Use this for initialization
	void Start () {
	cam = GameObject.Find("Main Camera");
	}
	
	// Update is called once per frame
	void Update () {
	gameObject.transform.rotation = cam.transform.rotation;
	}
}
