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
	
	public bool networking = false;
	
	GameObject manager;
	
	// Use this for initialization
	void Start () {
		menuAreaNormalized = new Rect(menuArea.x, Screen.height - menuArea.height + menuArea.y, menuArea.width, menuArea.height);
		manager = GameObject.Find("Character");
	}
	
	// Update is called once per frame
	void Update () {
		
		// Hotkeys
		if (!PauseMenuGUI.isPaused)
		{
			// End turn
			if (Input.GetKeyDown(KeyCode.Alpha5) && !ClickAndMove.aIsObjectMoving && ((CharacterManager.aTurn == 1 && (Network.isServer || !networking)) || (CharacterManager.aTurn == 3 && (Network.isClient || !networking))))
			{
				audio.PlayOneShot(click);
				manager.SendMessage("endTurn");
			}
			
			if (CharacterManager.aSingleUnitIsSelected && CharacterManager.aInteractiveUnitIsSelected && ((CharacterManager.aTurn == 1 && (Network.isServer || !networking)) || (CharacterManager.aTurn == 3 && (Network.isClient || !networking))))
			{
				// Attack
				if (Input.GetKeyDown(KeyCode.Alpha1))
				{
					if (CharacterManager.aInteractiveUnitIsSelected)
					{
						manager.SendMessage("attack");
						manager.SendMessage("EndMidTurn");
					}
				}
				
				// Ability / Tame
				else if (Input.GetKeyDown(KeyCode.Alpha2))
				{
					if (!isBird) // ability
					{
						audio.PlayOneShot(click);
						// plug ability code here
					}
					
					else // attack
					{
						audio.PlayOneShot(click);
						if (CharacterManager.aInteractiveUnitIsSelected)
						{
							manager.SendMessage("tame");
							manager.SendMessage("EndMidTurn");
						}
					}
				}
			}
			
			// Cancel
			if (CharacterManager.aCurrentlySelectedUnit && !ClickAndMove.aIsObjectMoving && CharacterManager.aMidTurn && ((CharacterManager.aTurn == 1 && (Network.isServer || !networking)) || (CharacterManager.aTurn == 3 && (Network.isClient || !networking))))
			{
				if (Input.GetKeyDown(KeyCode.Alpha3))
				{
					audio.PlayOneShot(click);
					if(CharacterManager.aMidTurn)
						manager.SendMessage("cancelMove");
				}
			}
			
			if (!ClickAndMove.aIsObjectMoving && CharacterManager.aSingleUnitIsSelected && ((CharacterManager.aTurn == 1 && (Network.isServer || !networking)) || (CharacterManager.aTurn == 3 && (Network.isClient || !networking))))
			{
				if (Input.GetKeyDown(KeyCode.Alpha4))
				{
					audio.PlayOneShot(click);
					if(CharacterManager.aMidTurn)
					{
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
			}	
		}
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
		
		GUI.enabled = !PauseMenuGUI.isPaused && CharacterManager.aCurrentlySelectedUnit && !ClickAndMove.aIsObjectMoving && CharacterManager.aMidTurn && ((CharacterManager.aTurn == 1 && (Network.isServer || !networking)) || (CharacterManager.aTurn == 3 && (Network.isClient || !networking)));
		if(GUI.Button(new Rect(cancelButton), "[3] Cancel"))
		{
			audio.PlayOneShot(click);
			
			if(CharacterManager.aMidTurn)
				manager.SendMessage("cancelMove");
		}
		//if (!isBird && CharacterManager.aCurrentlySelectedUnit)
		//	CharacterManager.aCurrentlySelectedUnit.SendMessage("SendAttackSpentStatus");
		//else
		//	attackIsSpent = false;
		GUI.enabled = CharacterManager.aSingleUnitIsSelected && CharacterManager.aInteractiveUnitIsSelected && /*PlayerFunctionalityAndStats.isLegalMove &&*/ ((CharacterManager.aTurn == 1 && (Network.isServer || !networking)) || (CharacterManager.aTurn == 3 && (Network.isClient || !networking)));// && !attackIsSpent;
		if(GUI.Button(new Rect(attackButton), "[1] Attack: " + attackCost))
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
			/*if(GUI.Button(new Rect(abilityButton), "[2] Ability: " + abilityCost))
			{
				audio.PlayOneShot(click);
			}*/
			GUI.enabled = false;
			if(GUI.Button(new Rect(tameButton), "[2] Tame: " + tameCost))
			{
				audio.PlayOneShot(click);
				if (CharacterManager.aInteractiveUnitIsSelected)
				{
					manager.SendMessage("tame");
					manager.SendMessage("EndMidTurn");
				}
			}
		}
		
		else
		{
			//GUI.enabled = untamed;
			if(GUI.Button(new Rect(tameButton), "[2] Tame: " + tameCost))
			{
				audio.PlayOneShot(click);
				if (CharacterManager.aInteractiveUnitIsSelected)
				{
					manager.SendMessage("tame");
					manager.SendMessage("EndMidTurn");
				}
			}
		}
		
		if (CharacterManager.aInteractiveUnitIsSelected)
		{
		if (isBird && CharacterManager.aInteractUnit.tag.Equals(CharacterManager.aCurrentlySelectedUnit.tag))
		{
			//GUI.enabled = untamed;
			if(GUI.Button(new Rect(tameButton), "[2] Tame: " + tameCost))
			{
				audio.PlayOneShot(click);
				manager.SendMessage("tame");
				manager.SendMessage("EndMidTurn");
			}
		}
		}
		GUI.enabled = !PauseMenuGUI.isPaused && !ClickAndMove.aIsObjectMoving && CharacterManager.aSingleUnitIsSelected && ((CharacterManager.aTurn == 1 && (Network.isServer || !networking)) || (CharacterManager.aTurn == 3 && (Network.isClient || !networking)));
		if(GUI.Button(new Rect(waitButton), "[4] Wait: " + moveCost))
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
		GUI.enabled = !PauseMenuGUI.isPaused && !ClickAndMove.aIsObjectMoving && ((CharacterManager.aTurn == 1 && (Network.isServer || !networking)) || (CharacterManager.aTurn == 3 && (Network.isClient || !networking))) && !PauseMenuGUI.gameOver;
		if(GUI.Button(new Rect(endTurnButton), "[5] End Turn"))
		{
			audio.PlayOneShot(click);
			manager.SendMessage("endTurn");
		}
		GUI.EndGroup();
	}
}
