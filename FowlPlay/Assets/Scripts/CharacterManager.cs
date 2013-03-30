using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CharacterManager : MonoBehaviour {
	
	// Contains the currently selected object.
	public static GameObject aCurrentlySelectedUnit;
	public static GameObject aInteractUnit;
	public static string aInteractSpecies;
	public static string aCurrentlySelectedSpecies;
	
	// GameObject Unit -> GameObject Tile
	public static Hashtable aUnitsAndTilesHT;
	
	// Used for selection and deselection, it contains the selected
	// unit's original position before any movement happens, as well the rotation
	// of the unit of any selected interactive unit.
	public static Vector3 aCurrentlySelectedUnitOriginalPosition;
	public static Quaternion aCurrentlySelectedUnitOriginalRotation;
	public static Quaternion aInteractUnitOriginalRotation;

	public static Quaternion aRotationAfterMove;
	public static Quaternion aRotationBeforeMove;

	public static int aOriginalMana;

	
	// Keeps track of whether any unit is selected at the time.
	public static bool aSingleUnitIsSelected = false;
	public static bool aInteractiveUnitIsSelected = false;
	
	public static GameObject bird1;
	public static GameObject bird2;
	
	// These lists aggregate all units.
	public static List<GameObject> player1Units;
	public static List<GameObject> player2Units;
	public static List<GameObject> untamedUnits;
	
	// Used for rotation of units.
	public static Vector3 startPos;
	public static Vector3 startRot;
	
	// Materials
	public Material aMaterialTeamRed, aMaterialTeamBlue;
	
	// Can be either 1 or 2 or 3 or 4
	//1 for player 1's turn.
	//3 for player 2's turn.
	//and 2 or 4 for untamed unit turn.
	public static int aTurn = 1;
	public static int aTurnCount = 1;
	public static bool aTurnIsCompleted = false;
	public static bool aMidTurn = false;
	
	public UntamedManager untamedMovementManager;
	
	// Sounds
	public AudioClip selectTrex, selectTricera, selectVeloci, selectAnquilo, selectPtero, selectBird, soundTame;
		
	// Use this for initialization
	void Start () {
		
		// Initialize and populate all collections
		player1Units = new List<GameObject>();
		player2Units = new List<GameObject>();
		untamedUnits = new List<GameObject>();
		
		aUnitsAndTilesHT = new Hashtable();
		
		foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Player1"))
		{
			player1Units.Add(unit);
			
			// Get the tile the unit is standing on and mark it as occupied.
			Vector3 tile = TileManager.getTileUnitIsStandingOn(unit);
			TileManager.getTileAt(tile).tag = "OccupiedTile";
			
			// Add the occupied tile to a hashtable that keeps track of what tiles are occupied and who is occupying them.
			TileManager.occupiedTilesHT.Add(tile, unit);
			aUnitsAndTilesHT.Add(unit, TileManager.getTileAt(tile));
		}

		foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Player2"))
		{
			player2Units.Add(unit);
			
			// Get the tile the unit is standing on and mark it as occupied.
			Vector3 tile = TileManager.getTileUnitIsStandingOn(unit);
			TileManager.getTileAt(tile).tag = "OccupiedTile";
			
			// Add the occupied tile to a hashtable that keeps track of what tiles are occupied and who is occupying them.
			TileManager.occupiedTilesHT.Add(tile, unit);
			aUnitsAndTilesHT.Add(unit, TileManager.getTileAt(tile));
		}
		
		foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Enemy"))
		{
			untamedUnits.Add(unit);
			
			// Get the tile the unit is standing on and mark it as occupied.
			Vector3 tile = TileManager.getTileUnitIsStandingOn(unit);
			TileManager.getTileAt(tile).tag = "OccupiedTile";
			
			// Add the occupied tile to a hashtable that keeps track of what tiles are occupied and who is occupying them.
			TileManager.occupiedTilesHT.Add(tile, unit);
			aUnitsAndTilesHT.Add(unit, TileManager.getTileAt(tile));
		}
		
		GameObject.Find("GUI Hot Seat").SendMessage("showText", "Player 1's Turn");
		
		bird1 = GameObject.Find("Bird1");
		bird2 = GameObject.Find("Bird2");

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (aInteractUnit)
			aInteractUnit.SendMessage("UpdateGuiTameButton");
		else
			PlayMenuGUI.untamed = false;
	}
	
	// Selects a unit of the player's team.
	public void selectUnit(GameObject pUnit)
	{
		// Track starting position and rotations.
		startPos = pUnit.transform.position;
		startRot = pUnit.transform.rotation.eulerAngles;
		
		// Set the unit as the currently selected unit and track its position and rotation.
		aCurrentlySelectedUnit = pUnit;
		aSingleUnitIsSelected = true;
		aCurrentlySelectedUnitOriginalPosition = pUnit.transform.position;
		aCurrentlySelectedUnitOriginalRotation = pUnit.transform.rotation;
		
		// Make it visible
		pUnit.SendMessage("SelectedColor");
		
		// Set the GUI components for players.
		if (aTurn == 1 || aTurn == 3)
		{
			pUnit.GetComponentInChildren<Camera>().camera.enabled = true;
			pUnit.SendMessage("UpdateGuiStats");
			ProgressBarGUI.show1 = true;
			ProgressBarGUI.show2 = true;
		}
		
		// Highlight tiles in range
		if (!ClickAndMove.aIsObjectMoving && (aTurn == 1 || aTurn ==3))
			GameObject.Find("Character").SendMessage("getRange", pUnit);
		
		if(aCurrentlySelectedUnit == bird1 && aTurn == 1)
		{
			PlayMenuGUI.isBird = true;
			bird1.SendMessage("CheckLegalMove", aCurrentlySelectedUnit.GetComponent<BirdUnitFunctionalityAndStats>().moveCost);
		}
		else if(aCurrentlySelectedUnit != bird1 && aTurn == 1)
		{
			PlayMenuGUI.isBird = false;
			bird1.SendMessage("CheckLegalMove", aCurrentlySelectedUnit.GetComponent<DinosaurUnitFunctionalityAndStats>().moveCost);
		}
		if(aCurrentlySelectedUnit == bird2 && aTurn == 3)
		{
			PlayMenuGUI.isBird = true;
			bird2.SendMessage("CheckLegalMove", aCurrentlySelectedUnit.GetComponent<BirdUnitFunctionalityAndStats>().moveCost);
		}
		else if(aCurrentlySelectedUnit != bird2 && aTurn == 3)
		{
			PlayMenuGUI.isBird = false;
			bird2.SendMessage("CheckLegalMove", aCurrentlySelectedUnit.GetComponent<DinosaurUnitFunctionalityAndStats>().moveCost);
		}
		
		// Play the selection sound
		if (!CharacterManager.isBird(CharacterManager.aCurrentlySelectedUnit) && (aTurn == 1 || aTurn == 3))
		{	
			switch (CharacterManager.aCurrentlySelectedUnit.GetComponent<DinosaurUnitFunctionalityAndStats>().species)
			{	
				case "Tyrannosaur" :
				{
					audio.PlayOneShot(selectTrex);
					break;
				}
				case "Anquilosaurus" :
				{
					audio.PlayOneShot(selectAnquilo);
					break;
				}
				case "Pterodactyl" :
				{
					audio.PlayOneShot(selectPtero);
					break;
				}
				case "Triceratops" :
				{
					audio.PlayOneShot(selectTricera);
					break;
				}
				case "Velociraptor" :
				{
					audio.PlayOneShot(selectVeloci);
					break;
				}
			}
		}
		
		if(isBird(pUnit))
		{
			audio.PlayOneShot(selectBird);
		}
	}
	
	public void attack()
	{
		if (aCurrentlySelectedUnit == bird1 || aCurrentlySelectedUnit == bird2)
			aCurrentlySelectedUnit.GetComponent<BirdUnitFunctionalityAndStats>().StartCoroutine("AttackUnit", aInteractUnit);
		else
			aCurrentlySelectedUnit.GetComponent<DinosaurUnitFunctionalityAndStats>().StartCoroutine("AttackUnit", aInteractUnit);

		//aCurrentlySelectedUnit.SendMessage("AttackUnit", aInteractUnit);
		
		SendMessage("unhighlightRange");
		
		//deselectUnit();
		
		//endTurn();
	}
	
	public void tame()
	{
		aCurrentlySelectedUnit.SendMessage("TameUnit", aInteractUnit);
		
		SendMessage("unhighlightRange");
		
		audio.PlayOneShot(soundTame);
		
		//deselectUnit();
		
		//endTurn();
	}
	
	// Executed when a unit's HP hits 0.
	public static void killUnit(GameObject pUnit)
	{
		// Remove it from its list.
		if (pUnit.tag.Equals("Player1"))
			player1Units.Remove(pUnit);
		
		else if (pUnit.tag.Equals("Player2"))
			player2Units.Remove(pUnit);
		
		else if (pUnit.tag.Equals("Enemy"))
			untamedUnits.Remove(pUnit);
		
		// Mark the tile as unoccupied.
		Vector3 unitsTile = TileManager.getTileUnitIsStandingOn(pUnit);
		
		TileManager.occupiedTilesHT.Remove(unitsTile);
		TileManager.getTileAt(unitsTile).tag = "Tile";
	}
	
	// Ends the players turn.
	public void endTurn()
	{
		if (!ClickAndMove.aIsObjectMoving)
		{
			if (aInteractiveUnitIsSelected)
			{
				aInteractUnit.SendMessage("UpdateColor");
				aInteractUnit = null;
			}
			
			// Some costs may not have been reset. Reset them.
			resetCosts();
			SendMessage("unhighlightRange");
			
			// Special case -- unhighlight source tile if it's within attack range if we end turn was pressed.
			if (TileManager.getTileAt(aCurrentlySelectedUnitOriginalPosition) != null)
				SendMessage("deselectSingleTile", TileManager.getTileAt(aCurrentlySelectedUnitOriginalPosition));
			
			deselectUnit();
			
			//End Of Turn Updates for the Units.
			
			//The units all lose a little bit of their tameness.
			if (aTurn == 1)
			{
				List<GameObject> toUntame = new List<GameObject>();
				
				foreach (GameObject unit in player1Units)
				{
					unit.SendMessage("EndTurnTickUntame", bird1.transform.position);
					unit.SendMessage("StartTurnUpdateSpent");
					
					if (!isBird(unit))
						if (unit.GetComponent<DinosaurUnitFunctionalityAndStats>().tamePoints <= 0)
							toUntame.Add(unit);
				}
				
				foreach (GameObject unit in toUntame)
				{
					player1Units.Remove(unit);
					unit.GetComponent<DinosaurUnitFunctionalityAndStats>().SwitchTeams("Enemy");
					unit.GetComponent<DinosaurUnitFunctionalityAndStats>().UpdateColor();
					unit.GetComponent<DinosaurUnitFunctionalityAndStats>().tamed = false;
				}
			}
			else if (aTurn == 3)
			{
				List<GameObject> toUntame = new List<GameObject>();
				
				foreach (GameObject unit in player2Units)
				{
					unit.SendMessage("EndTurnTickUntame", bird2.transform.position);
					unit.SendMessage("StartTurnUpdateSpent");
					
					if (!isBird(unit))
						if (unit.GetComponent<DinosaurUnitFunctionalityAndStats>().tamePoints <= 0)
							toUntame.Add(unit);
				}
				
				foreach (GameObject unit in toUntame)
				{
					player2Units.Remove(unit);
					unit.GetComponent<DinosaurUnitFunctionalityAndStats>().SwitchTeams("Enemy");
					unit.GetComponent<DinosaurUnitFunctionalityAndStats>().UpdateColor();
					unit.GetComponent<DinosaurUnitFunctionalityAndStats>().tamed = false;
				}
			}
			else if (aTurn == 2 || aTurn == 4)
			{
				foreach (GameObject unit in untamedUnits)
				{
					unit.SendMessage("StartTurnUpdateSpent");
				}
			}
			
			aInteractiveUnitIsSelected = false;
			aMidTurn = false;
			aTurnIsCompleted = true;
			switchTurn();
		}
		
		//yield return untamedMovementManager.StartCoroutine("untamedMove");
		
	}
	
	// Resets costs hashtable back to -1 for all tiles.
	public static void resetCosts()
	{
			foreach (GameObject tile in TileManager.allTiles)
			{
				if ((int)TileManager.costs[tile.transform.position] != -1)
				{
					TileManager.costs.Remove(tile.transform.position);
					TileManager.costs.Add(tile.transform.position, -1);
				}
			}
			
			if(aCurrentlySelectedUnit && aCurrentlySelectedUnit.tag != "Enemy")
			{
				//aCurrentlySelectedUnit.GetComponentInChildren<Camera>().camera.enabled = false;
			}
	}
	
	// Deselects the currently selected unit.
	public void deselectUnit()
	{
		if (aCurrentlySelectedUnit != null)
		{
			aCurrentlySelectedUnit.SendMessage("UpdateColor");
			
			if (aTurn == 1 || aTurn == 3)
			{
				aCurrentlySelectedUnit.GetComponentInChildren<Camera>().camera.enabled = false;
				ProgressBarGUI.show1 = false;
				ProgressBarGUI.show2 = false;
				aCurrentlySelectedUnit = null;
			}
			
		}
		
		//aCurrentlySelectedUnit.GetComponentInChildren<Camera>().camera.enabled = false;
		aCurrentlySelectedUnit = null;
		aSingleUnitIsSelected = false;
		
		// un-highlight tiles in range
		if (!ClickAndMove.aIsObjectMoving && (aTurn == 1 || aTurn ==3))
			SendMessage("unhighlightRange");
		
		resetCosts();
	}
	
	// Undo a move at mid-turn.
	public void cancelMove()
	{
		SendMessage("unhighlightRange");
		
		GameObject temp = aCurrentlySelectedUnit;
		
		GameObject tileGO = (GameObject)aUnitsAndTilesHT[aCurrentlySelectedUnit];
		Vector3 tile = tileGO.transform.position;
		
		if (ClickAndMove.aMovementHappened)
		{
			
			TileManager.occupiedTilesHT.Remove(tile);
			TileManager.getTileAt(tile).tag = "Tile";
			
			TileManager.occupiedTilesHT.Add(TileManager.getTileUnitIsStandingOn(aCurrentlySelectedUnitOriginalPosition), aCurrentlySelectedUnit);
			TileManager.getTileAt(TileManager.getTileUnitIsStandingOn(aCurrentlySelectedUnitOriginalPosition)).tag = "OccupiedTile";
		}
		
		aCurrentlySelectedUnit.transform.position = aCurrentlySelectedUnitOriginalPosition;
		aCurrentlySelectedUnit.transform.rotation = aCurrentlySelectedUnitOriginalRotation;
		
		// Update HT with new position
		aUnitsAndTilesHT.Remove(aCurrentlySelectedUnit);
		aUnitsAndTilesHT.Add(aCurrentlySelectedUnit,TileManager.getTileAt(TileManager.getTileUnitIsStandingOn(aCurrentlySelectedUnit)));

		if (aCurrentlySelectedUnit.tag == "Player1")
		{
			bird1.SendMessage("RestoreMana");
		}
		else if (aCurrentlySelectedUnit.tag == "Player2")
		{
			bird2.SendMessage("RestoreMana");
		}
		
		resetCosts();
		aMidTurn = false;
		deselectUnit();
		SendMessage("deselectTile");
		selectUnit(temp);
		ClickAndMove.aMovementHappened = false;
		//Unspending Movement
		CharacterManager.aCurrentlySelectedUnit.SendMessage("UnspendMovement");
		
		if (aInteractiveUnitIsSelected)
		{
			aInteractUnit.transform.rotation = aInteractUnitOriginalRotation;
			aInteractUnit.SendMessage("UpdateColor");
			aInteractiveUnitIsSelected = false;
			aInteractUnit = null;
		}
	}
	
	public static void switchTurn()
	{
		
		if(aTurnIsCompleted && !PauseMenuGUI.gameOver)
		{
			if (aTurn == 1)
			{
				//GameObject.Find("Character").SendMessage("pickRandomTile");
				/*foreach (GameObject unit in player1Units)
				{
					unit.SendMessage("EndTurnTickUntame", 1);
				}*/
				
				if (untamedUnits.Count == 0)
				{
					GameObject.Find("GUI Hot Seat").SendMessage("showText", "Player 2's Turn");
					bird2.SendMessage("StartTurn");
					aTurn = 3;	
				}
				else
				{
					GameObject.Find("GUI Hot Seat").SendMessage("showText", "Untamed Turn");
					aTurn = 2;
					GameObject.Find("Character").SendMessage("untamedMove");
				}
				
				bird1.SendMessage("PlayerEndTurn");
			}
			else if (aTurn == 2)
			{
				GameObject.Find("GUI Hot Seat").SendMessage("showText", "Player 2's Turn");
				bird2.SendMessage("StartTurn");
				aTurn = 3;
			}
			else if (aTurn == 3)
			{
				//GameObject.Find("Character").SendMessage("pickRandomTile");
				if (untamedUnits.Count == 0)
				{
					GameObject.Find("GUI Hot Seat").SendMessage("showText", "Player 1's Turn");
					bird1.SendMessage("StartTurn");
					aTurn = 1;
				}
				else
				{
					GameObject.Find("GUI Hot Seat").SendMessage("showText", "Untamed's Turn");
					aTurn = 4;
					GameObject.Find("Character").SendMessage("untamedMove");
				}
				
				bird2.SendMessage("PlayerEndTurn");
			}
			else if (aTurn == 4)
			{
				GameObject.Find("GUI Hot Seat").SendMessage("showText", "Player 1's Turn");
				bird1.SendMessage("StartTurn");
				aTurn = 1;
			}
			
			aTurnIsCompleted = false;
			aTurnCount++;
			
			//if (aTurn == 2 || aTurn == 4)
				//GameObject.Find("Character").SendMessage("untamedMove");
		}
		
	}
	
	public void EndMidTurn()
	{
		if (!ClickAndMove.aIsObjectMoving)
		{
			if (aInteractiveUnitIsSelected)
			{
				aInteractUnit.SendMessage("UpdateColor");
				aInteractUnit = null;
			}
			
			// Some costs may not have been reset. Reset them.
			resetCosts();
			SendMessage("unhighlightRange");
			
			// Special case -- unhighlight source tile if it's within attack range if we end turn was pressed.
			if (TileManager.getTileAt(aCurrentlySelectedUnitOriginalPosition) != null)
				SendMessage("deselectSingleTile", TileManager.getTileAt(aCurrentlySelectedUnitOriginalPosition));
			
			if(!isBird(aCurrentlySelectedUnit))
			{
				aCurrentlySelectedUnit.GetComponent<DinosaurUnitFunctionalityAndStats>().attackSpent = true;
				aCurrentlySelectedUnit.GetComponent<DinosaurUnitFunctionalityAndStats>().moveSpent = true;
			}
			
			deselectUnit();
			
			aInteractiveUnitIsSelected = false;
			aMidTurn = false;
		}
		
		ClickAndMove.aMovementHappened = false;
	}
	
	public static bool isBird(GameObject pUnit)
	{
		bool result = false;
		
		if (pUnit == bird1 || pUnit == bird2)
			result = true;
		
		return result;
	}
}