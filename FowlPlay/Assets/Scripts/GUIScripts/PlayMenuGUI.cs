using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioSource))]

public class PlayMenuGUI : MonoBehaviour {
	
	public AudioClip click;
	public GUISkin menuSkin;
	public Rect menuArea;
	public Rect attackButton;
	public Rect abilityButton;
	public Rect endTurnButton;
	public Rect cancelButton;
	Rect menuAreaNormalized;
	
	// Use this for initialization
	void Start () {
		menuAreaNormalized = new Rect(menuArea.x * Screen.width - (menuArea.width * 0.5f), menuArea.y * Screen.height - (menuArea.height * 0.5f), menuArea.width, menuArea.height);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		GUI.skin = menuSkin;
		GUI.BeginGroup(menuAreaNormalized);
		if(GUI.Button(new Rect(attackButton), "Attack"))
		{
			audio.PlayOneShot(click);
		}
		if(GUI.Button(new Rect(abilityButton), "Ability"))
		{
			audio.PlayOneShot(click);
		}
		if(GUI.Button(new Rect(endTurnButton), "End Turn"))
		{
			audio.PlayOneShot(click);
		}
		if(GUI.Button(new Rect(cancelButton), "Cancel"))
		{
			audio.PlayOneShot(click);
		}
		GUI.EndGroup();
	}
}
