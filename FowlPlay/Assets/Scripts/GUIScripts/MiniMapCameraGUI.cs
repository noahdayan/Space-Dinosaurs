using UnityEngine;
using System.Collections;

public class MiniMapCameraGUI : MonoBehaviour {
	
	public Rect cameraArea;
	
	// Use this for initialization
	void Start () {
		camera.pixelRect = new Rect(Screen.width - cameraArea.width + cameraArea.x, cameraArea.y, cameraArea.width, cameraArea.height);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
