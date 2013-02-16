using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CharacterManager : MonoBehaviour {
	
	// Contains the currently selected object.
	public static GameObject aCurrentlySelectedUnit;
	public static GameObject aInteractUnit;
	
	// Used for selection and deselection, it contains the selected
	// unit's original position before any movement happens.
	public static Vector3 aCurrentlySelectedUnitOriginalPosition;
	public static Quaternion aCurrentlySelectedUnitOriginalRotation;
	
	// Keeps track of whether any unit is selected at the time.
	public static bool aSingleUnitIsSelected = false;
	public static bool aInteractiveUnitIsSelected = false;
	
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
	public static bool aTurnIsCompleted = false;
	public static bool aMidTurn = false;
	
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
			TileManager.occupiedTilesHT.Add(tile, unit);
		}

		foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Player2"))
		{
			player2Units.Add(unit);
			unitsHT.Add(unit.transform.position, unit);
			
			// Get the tile the unit is standing on and mark it as occupied.
			Vector3 tile = unit.transform.position;
			tile.y = 2.0f;
			TileManager.getTileAt(tile).tag = "OccupiedTile";
			
			// Add the occupied tile to a hashtable that keeps track of what tiles are occupied and who is occupying them.
			TileManager.occupiedTilesHT.Add(tile, unit);
		}
		
		foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Enemy"))
		{
			untamedUnits.Add(unit);
			unitsHT.Add(unit.transform.position, unit);
			
			// Get the tile the unit is standing on and mark it as occupied.
			Vector3 tile = unit.transform.position;
			tile.y = 2.0f;
			TileManager.getTileAt(tile).tag = "OccupiedTile";
			
			// Add the occupied tile to a hashtable that keeps track of what tiles are occupied and who is occupying them.
			TileManager.occupiedTilesHT.Add(tile, unit);
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
		aCurrentlySelectedUnitOriginalRotation = pUnit.transform.rotation;
		pUnit.renderer.material.color = Color.yellow;
		aSingleUnitIsSelected = true;
		//pUnit.GetComponentInChildren<Camera>().camera.enabled = true;
		
		// highlight tiles in range
		if (!ClickAndMove.aIsObjectMoving && (aTurn == 1 || aTurn ==3))
			GameObject.Find("Character").SendMessage("getRange", pUnit);
	}
	
	public void attack()
	{
		// Do attack stuff
		
		// When attack is complete, end turn (for now)
		
		// Deselect all tiles and clear all stuff.
		
		aCurrentlySelectedUnit.SendMessage("AttackUnit", aInteractUnit);
		
		SendMessage("unhighlightRange");
		
		deselectUnit();
		
		endTurn();
	}
	
	public static void killUnit(GameObject pUnit)
	{
		if (pUnit.tag.Equals("Player1"))
			player1Units.Remove(pUnit);
		
		else if (pUnit.tag.Equals("Player2"))
			player2Units.Remove(pUnit);
		
		else if (pUnit.tag.Equals("Enemy"))
			untamedUnits.Remove(pUnit);
		
		
		unitsHT.Remove(pUnit.transform.position);
		
		Vector3 unitsTile = pUnit.transform.position;
		unitsTile.y = 2.0f;
		
		TileManager.occupiedTilesHT.Remove(unitsTile);
		TileManager.getTileAt(unitsTile).tag = "Tile";
	}
	
	public void endTurn()
	{
		if (!ClickAndMove.aIsObjectMoving)
		{
			if (aInteractiveUnitIsSelected)
			{
				aInteractUnit.renderer.material.color = Color.blue;
				aInteractUnit = null;
			}
			
			aInteractiveUnitIsSelected = false;
			aMidTurn = false;
			aTurnIsCompleted = true;
			switchTurn();
			
			// Some costs may not have been reset. Reset them.
			resetCosts();
			SendMessage("unhighlightRange");
			deselectUnit();
			
			// Special case -- unhighlight source tile if it's within attack range if we end turn was pressed.
			SendMessage("deselectSingleTile", TileManager.getTileAt(aCurrentlySelectedUnitOriginalPosition));
		}
	}
	
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
				aCurrentlySelectedUnit.GetComponentInChildren<Camera>().camera.enabled = false;
			}
			
			//deselectUnit();
	}
	
	public void deselectUnit()
	{
		if (aCurrentlySelectedUnit != null)
		{
			aCurrentlySelectedUnit.renderer.material.color = Color.blue;
			
			// if the unit has moved, update the hashtables
			if(aCurrentlySelectedUnit.transform.position != aCurrentlySelectedUnitOriginalPosition)
			{
				unitsHT.Remove(aCurrentlySelectedUnitOriginalPosition);
				aCurrentlySelectedUnitOriginalPosition.y = 2.0f;
				TileManager.occupiedTilesHT.Remove(aCurrentlySelectedUnitOriginalPosition);
	
				unitsHT.Add(aCurrentlySelectedUnit.transform.position, aCurrentlySelectedUnit);
				Vector3 correctedPosition = aCurrentlySelectedUnit.transform.position;
				correctedPosition.y = 2.0f;
				TileManager.occupiedTilesHT.Add(correctedPosition, aCurrentlySelectedUnit);		
			}
			
			aCurrentlySelectedUnit = null;
		}
		
		//aCurrentlySelectedUnit.GetComponentInChildren<Camera>().camera.enabled = false;
		aCurrentlySelectedUnit = null;
		aSingleUnitIsSelected = false;
		
		
		
		// un-highlight tiles in range
		if (!ClickAndMove.aIsObjectMoving && (aTurn == 1 || aTurn ==3))
			SendMessage("unhighlightRange");
	}
	
	public void cancelMove()
	{
		SendMessage("unhighlightRange");
		
		GameObject temp = aCurrentlySelectedUnit;
		
		Vector3 tile = aCurrentlySelectedUnit.transform.position;
		tile.y = 2.0f;
		
		TileManager.getTileAt(tile).renderer.material.color = Color.red;
		TileManager.getTileAt(tile).tag = "Tile";
		
		TileManager.occupiedTilesHT.Remove(tile);
		
		Vector3 oldTile = aCurrentlySelectedUnitOriginalPosition;
		oldTile.y = 2.0f;
		
		TileManager.getTileAt(oldTile).tag = "OccupiedTile";
		
		aCurrentlySelectedUnit.transform.position = aCurrentlySelectedUnitOriginalPosition;
		aCurrentlySelectedUnit.transform.rotation = aCurrentlySelectedUnitOriginalRotation;
		
		resetCosts();
		aMidTurn = false;
		deselectUnit();
		SendMessage("deselectTile");
		selectUnit(temp);
		
		if (aInteractiveUnitIsSelected)
		{
			aInteractUnit.renderer.material.color = Color.blue;
			aInteractiveUnitIsSelected = false;
			aInteractUnit = null;
		}
	}
	
	public static void switchTurn()
	{
		if(aTurnIsCompleted)
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
		
		aTurnIsCompleted = false;
	}
}
