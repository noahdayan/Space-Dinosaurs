using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CharacterManager : MonoBehaviour {
	
	// Contains the currently selected object.
	public static GameObject aCurrentlySelectedUnit;
	
	// Used for selection and deselection, it contains the selected
	// unit's original position before any movement happens.
	public static Vector3 aCurrentlySelectedUnitOriginalPosition;
	
	// Keeps track of whether any unit is selected at the time.
	public static bool aSingleUnitIsSelected = false;
	
	// These lists aggregate all units.
	private static List<GameObject> player1Units;
	private static List<GameObject> player2Units;
	private static List<GameObject> untamedUnits;
	
	// Used for rotation of units.
	public static Vector3 startPos;
	public static Vector3 startRot;
	
	// Stores (Vector3: unit position, unit)
	public static Hashtable unitsHT;
	
	// Can be either 1 or 2 or 3 or 4
	public static int aTurn = 1;
	
	// Use this for initialization
	void Start () {
		
		// Initialize and populate all collections
		
		player1Units = new List<GameObject>();
		player2Units = new List<GameObject>();
		untamedUnits = new List<GameObject>();
		
		unitsHT = new Hashtable();
		
		foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Player1"))
		{
			player1Units.Add(unit);
			unitsHT.Add(unit.transform.position, unit);
			
			// Get the tile the unit is standing on and mark it as occupied.
			Vector3 tile = unit.transform.position;
			tile.y = 2.0f;
			TileManager.getTileAt(tile).tag = "OccupiedTile";
			
			// Add the occupied tile to a hashtable that keeps track of what tiles are occupied and who is occupying them.
			TileManager.occupiedTilesHT.Add(TileManager.getTileAt(tile), unit);
		}

		foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Player2"))
		{
			player2Units.Add(unit);
			unitsHT.Add(unit.transform.position, unit);
			Vector3 tile = unit.transform.position;
			tile.y = 2.0f;
			TileManager.getTileAt(tile).tag = "OccupiedTile";
			TileManager.occupiedTilesHT.Add(TileManager.getTileAt(tile), unit);
		}
		
		foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Enemy"))
		{
			untamedUnits.Add(unit);
			unitsHT.Add(unit.transform.position, unit);
			Vector3 tile = unit.transform.position;
			tile.y = 2.0f;
			TileManager.getTileAt(tile).tag = "OccupiedTile";
			TileManager.occupiedTilesHT.Add(TileManager.getTileAt(tile), unit);
		}
		
		GameObject.Find("GUI Hot Seat").SendMessage("showText", "Player 1's Turn");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void selectUnit(GameObject pUnit)
	{
		startPos = pUnit.transform.position;
		startRot = pUnit.transform.rotation.eulerAngles;
		aCurrentlySelectedUnit = pUnit;
		aCurrentlySelectedUnitOriginalPosition = pUnit.transform.position;
		pUnit.renderer.material.color = Color.yellow;
		aSingleUnitIsSelected = true;
		
		// highlight tiles in range
		if (!ClickAndMove.aIsObjectMoving && (aTurn == 1 || aTurn ==3))
			GameObject.Find("Character").SendMessage("getRange", pUnit);
	}
	
	public static void deselect()
	{
		aCurrentlySelectedUnit.renderer.material.color = Color.blue;
		
		// if the unit has moved, update the hashtable
		if(aCurrentlySelectedUnit.transform.position != aCurrentlySelectedUnitOriginalPosition)
		{
				unitsHT.Remove(aCurrentlySelectedUnitOriginalPosition);
				unitsHT.Add(aCurrentlySelectedUnit.transform.position, aCurrentlySelectedUnit);
		}
		aCurrentlySelectedUnit = null;
		aSingleUnitIsSelected = false;
		
		// un-highlight tiles in range
		if (!ClickAndMove.aIsObjectMoving && (aTurn == 1 || aTurn ==3))
			GameObject.Find("Character").SendMessage("unhighlightRange");
	}
	
	public static void switchTurn()
	{
		if (aTurn == 1)
		{
			GameObject.Find("GUI Hot Seat").SendMessage("showText", "Untamed Turn");
			//GameObject.Find("Character").SendMessage("pickRandomTile");
			/*foreach (GameObject unit in player1Units)
			{
				unit.SendMessage("EndTurnTickUntame", 1);
			}*/
			aTurn = 2;
		}
		else if (aTurn == 2)
		{
			GameObject.Find("GUI Hot Seat").SendMessage("showText", "Player 2's Turn");
			aTurn = 3;
		}
		else if (aTurn == 3)
		{
			GameObject.Find("GUI Hot Seat").SendMessage("showText", "Untamed's Turn");
			//GameObject.Find("Character").SendMessage("pickRandomTile");
			aTurn = 4;
		}
		else if (aTurn == 4)
		{
			GameObject.Find("GUI Hot Seat").SendMessage("showText", "Player 1's Turn");
			aTurn = 1;
		}
	}
}
