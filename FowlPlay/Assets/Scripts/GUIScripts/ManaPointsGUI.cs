using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioSource))]

public class ManaPointsGUI : MonoBehaviour {
	
	public Font font;
	public int fontSize;
	GUIStyle style;
	public int guiDepth = 0;
	public Rect manaArea;
	Rect manaAreaNormalized;
	public Rect manaPointsArea;
	
	public static int manaPoints = 10;
	
	public AudioClip error;
	
	// Use this for initialization
	void Start () {
		manaAreaNormalized = new Rect(Screen.width - manaArea.width + manaArea.x, manaArea.y, manaArea.width, manaArea.height);

		style = new GUIStyle();
		style.font = font;
		style.fontSize = fontSize;
	}
	
	// Update is called once per frame
	void Update () {	
		if(CharacterManager.aTurn == 1)
		{
			style.normal.textColor = Color.green;
		}
		else if(CharacterManager.aTurn == 3)
		{
			style.normal.textColor = Color.blue;
		}
		else
		{
			style.normal.textColor = Color.red;
		}
	}
	
	void OnGUI()
	{
		GUI.depth = guiDepth;
		GUI.BeginGroup(manaAreaNormalized);
		GUI.Label(new Rect(manaPointsArea), manaPoints.ToString(), style);
		GUI.EndGroup();
	}
	
	void ShakeText()
	{
		audio.PlayOneShot(error);
		iTween.ShakePosition(gameObject, new Vector3(0.01f, 0.01f, 0.0f), 0.5f);
	}
}