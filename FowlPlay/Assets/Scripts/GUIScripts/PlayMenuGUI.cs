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
	public Rect tameButton;
	public Rect waitButton;
	public Rect cancelButton;
	public Rect endTurnButton;
	Rect menuAreaNormalized;
	public static bool isBird = false;
	public static bool untamed = false;
	
	GameObject manager;
	
	// Use this for initialization
	void Start () {
		menuAreaNormalized = new Rect(menuArea.x, Screen.height - menuArea.height + menuArea.y, menuArea.width, menuArea.height);
		manager = GameObject.Find("Character");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		GUI.skin = menuSkin;
		GUI.depth = guiDepth;
		GUI.BeginGroup(menuAreaNormalized);
		
		GUI.enabled = !PauseMenuGUI.isPaused && CharacterManager.aCurrentlySelectedUnit && !ClickAndMove.aIsObjectMoving && CharacterManager.aMidTurn;
		if(GUI.Button(new Rect(cancelButton), "Cancel"))
		{
			audio.PlayOneShot(click);
			
			if(CharacterManager.aMidTurn)
				manager.SendMessage("cancelMove");
		}
		GUI.enabled = CharacterManager.aInteractiveUnitIsSelected && PlayerFunctionalityAndStats.isLegalMove;
		if(GUI.Button(new Rect(attackButton), "Attack"))
		{
			audio.PlayOneShot(click);
			if (CharacterManager.aInteractiveUnitIsSelected)
			{
				manager.SendMessage("attack");
				manager.SendMessage("EndMidTurn");
			}
		}
		if(!isBird)
		{
			if(GUI.Button(new Rect(abilityButton), "Ability"))
			{
				audio.PlayOneShot(click);
			}
		}
		else
		{
			GUI.enabled = untamed;
			if(GUI.Button(new Rect(tameButton), "Tame"))
			{
				audio.PlayOneShot(click);
				if (CharacterManager.aInteractiveUnitIsSelected)
				{
					manager.SendMessage("tame");
					manager.SendMessage("EndMidTurn");
				}
			}
		}
		GUI.enabled = !PauseMenuGUI.isPaused && !ClickAndMove.aIsObjectMoving && CharacterManager.aSingleUnitIsSelected;
		if(GUI.Button(new Rect(waitButton), "Wait"))
		{
			audio.PlayOneShot(click);
			if(CharacterManager.aMidTurn)
			{
				//manager.SendMessage("endTurn");
				manager.SendMessage("EndMidTurn");
			}
			else
			{
				manager.SendMessage("unhighlightRange");
				CharacterManager.aMidTurn = true;
				ClickAndMove.aIsObjectMoving = false;
				manager.SendMessage("paintAttackableTilesAfterMove");
			}
		}
		GUI.enabled = !PauseMenuGUI.isPaused && !ClickAndMove.aIsObjectMoving;
		if(GUI.Button(new Rect(endTurnButton), "End Turn"))
		{
			audio.PlayOneShot(click);
			manager.SendMessage("endTurn");
		}
		GUI.EndGroup();
	}
}
