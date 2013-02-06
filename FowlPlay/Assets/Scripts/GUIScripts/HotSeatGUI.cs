using UnityEngine;
using System.Collections;

public class HotSeatGUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	IEnumerator showText(string message) {
		guiText.text = message;
		guiText.enabled = true;
		yield return new WaitForSeconds(2.0f);
		guiText.enabled = false;
	}
}
