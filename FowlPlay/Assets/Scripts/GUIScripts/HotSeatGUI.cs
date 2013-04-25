using UnityEngine;
using System.Collections;

public class HotSeatGUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(CharacterManager.aTurn == 1)
		{
			guiText.material.color = Color.green;
		}
		else if(CharacterManager.aTurn == 3)
		{
			guiText.material.color = Color.blue;
		}
		else
		{
			guiText.material.color = Color.red;
		}
	}
	
	IEnumerator showText(string message) {
		guiText.text = message;
		iTween.ValueTo(gameObject, iTween.Hash("from", -0.5f, "to", 0.5f, "onupdate", "UpdatePosition"));
		guiText.enabled = true;
		yield return new WaitForSeconds(2.0f);
		iTween.ValueTo(gameObject, iTween.Hash("from", 0.5f, "to", 1.5f, "onupdate", "UpdatePosition"));
		yield return new WaitForSeconds(2.0f);
		guiText.enabled = false;
	}
	
	void showTextEnd(string message) {
		guiText.text = message;
		UpdatePositionEnd();
		guiText.enabled = true;
	}
	
	void UpdatePosition(float newValue)
	{
		gameObject.transform.position = new Vector3(newValue, 0.5f, 0.0f);
	}
	
	void UpdatePositionEnd()
	{
		gameObject.transform.position = new Vector3(0.5f, 0.7f, 0.0f);
	}
}