using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioSource))]

public class PlayMenuGUI : MonoBehaviour {
	
	public AudioClip click;
	public GUISkin menuSkin;
	public int guiDepth = 0;
	public Rect menuArea;
	public Rect attackButton;
	public Rect abilityButton;
	public Rect cancelButton;
	public Rect endTurnButton;
	Rect menuAreaNormalized;
	
	GameObject manager;
	
	// Use this for initialization
	void Start () {
		menuAreaNormalized = new Rect(menuArea.x * Screen.width - (menuArea.width * 0.5f), menuArea.y * Screen.height - (menuArea.height * 0.5f), menuArea.width, menuArea.height);
		manager = GameObject.Find("Character");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		GUI.skin = menuSkin;
		GUI.depth = guiDepth;
		GUI.BeginGroup(menuAreaNormalized);
		if(!PauseMenuGUI.isPaused)
		{
			GUI.enabled = CharacterManager.aCurrentlySelectedUnit && !ClickAndMove.aIsObjectMoving && CharacterManager.aMidTurn;
		}
		else
		{
			GUI.enabled = false;
		}
		if(GUI.Button(new Rect(attackButton), "Attack"))
		{
			audio.PlayOneShot(click);
			if (CharacterManager.aInteractiveUnitIsSelected)
			{
				manager.SendMessage("attack");
			}
		}
		if(GUI.Button(new Rect(abilityButton), "Ability"))
		{
			audio.PlayOneShot(click);
		}
		if(GUI.Button(new Rect(cancelButton), "Cancel"))
		{
			audio.PlayOneShot(click);
			
			if(CharacterManager.aMidTurn)
				manager.SendMessage("cancelMove");
		}
		if(!PauseMenuGUI.isPaused)
		{
			GUI.enabled = !ClickAndMove.aIsObjectMoving;
		}
		if(GUI.Button(new Rect(endTurnButton), "End Turn"))
		{
			audio.PlayOneShot(click);
			manager.SendMessage("endTurn");
		}
		GUI.EndGroup();
	}
}
