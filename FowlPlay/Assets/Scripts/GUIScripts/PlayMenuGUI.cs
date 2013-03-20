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
	
	public static int attackCost = 0;
	public static int tameCost = 0;
	public static int abilityCost = 0;
	public static int moveCost = 0;
	
	Rect menuAreaNormalized;
	public static bool isBird = false;
	public static bool untamed = false;
	//public static bool attackIsSpent = false;
	
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
		if (CharacterManager.aCurrentlySelectedUnit)
		{
			CharacterManager.aCurrentlySelectedUnit.SendMessage("UpdateGuiCosts");
		}
		else
		{
			attackCost = 0;
			tameCost = 0;
			abilityCost = 0;
			moveCost = 0;
		}
		GUI.skin = menuSkin;
		GUI.depth = guiDepth;
		GUI.BeginGroup(menuAreaNormalized);
		
		GUI.enabled = !PauseMenuGUI.isPaused && CharacterManager.aCurrentlySelectedUnit && !ClickAndMove.aIsObjectMoving && CharacterManager.aMidTurn && (CharacterManager.aTurn == 1 || CharacterManager.aTurn == 3);
		if(GUI.Button(new Rect(cancelButton), "[3] Cancel") || (Input.GetKeyDown(KeyCode.Alpha3) && GUI.enabled))
		{
			audio.PlayOneShot(click);
			
			if(CharacterManager.aMidTurn)
				manager.SendMessage("cancelMove");
		}
		//if (!isBird && CharacterManager.aCurrentlySelectedUnit)
		//	CharacterManager.aCurrentlySelectedUnit.SendMessage("SendAttackSpentStatus");
		//else
		//	attackIsSpent = false;
		GUI.enabled = CharacterManager.aSingleUnitIsSelected && CharacterManager.aInteractiveUnitIsSelected && /*PlayerFunctionalityAndStats.isLegalMove &&*/ (CharacterManager.aTurn == 1 || CharacterManager.aTurn == 3);// && !attackIsSpent;
		if(GUI.Button(new Rect(attackButton), "[1] Attack: " + attackCost) || (Input.GetKeyDown(KeyCode.Alpha1) && GUI.enabled))
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
			if(GUI.Button(new Rect(abilityButton), "[2] Ability: " + abilityCost) || (Input.GetKeyDown(KeyCode.Alpha2) && GUI.enabled))
			{
				audio.PlayOneShot(click);
			}
		}
		else
		{
			GUI.enabled = untamed;
			if(GUI.Button(new Rect(tameButton), "[2] Tame: " + tameCost) || (Input.GetKeyDown(KeyCode.Alpha2) && GUI.enabled))
			{
				audio.PlayOneShot(click);
				if (CharacterManager.aInteractiveUnitIsSelected)
				{
					manager.SendMessage("tame");
					manager.SendMessage("EndMidTurn");
				}
			}
		}
		GUI.enabled = !PauseMenuGUI.isPaused && !ClickAndMove.aIsObjectMoving && CharacterManager.aSingleUnitIsSelected && (CharacterManager.aTurn == 1 || CharacterManager.aTurn == 3);
		if(GUI.Button(new Rect(waitButton), "[4] Wait: " + moveCost) || (Input.GetKeyDown(KeyCode.Alpha4) && GUI.enabled))
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
		GUI.enabled = !PauseMenuGUI.isPaused && !ClickAndMove.aIsObjectMoving && (CharacterManager.aTurn == 1 || CharacterManager.aTurn == 3);
		if(GUI.Button(new Rect(endTurnButton), "[5] End Turn") || (Input.GetKeyDown(KeyCode.Alpha5) && GUI.enabled))
		{
			audio.PlayOneShot(click);
			manager.SendMessage("endTurn");
		}
		GUI.EndGroup();
	}
}
