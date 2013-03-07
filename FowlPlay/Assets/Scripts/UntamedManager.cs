using UnityEngine;
using System.Collections;

public class UntamedManager : MonoBehaviour {
	
	public float aSpeedOfMovement = 20.0f;
	public static bool aIsObjectMoving = false;
	public static bool aIsObjectRotating = false;
	public static bool aMovementHappened = false;
	
	public static Vector3[] aPath;
	
	private Vector3 destination;
	
	static GameObject charManager;
	
	// Use this for initialization
	void Start () {
		charManager = GameObject.Find("Character");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public static void RandomMovement()
	{
		foreach (GameObject untamedUnit in CharacterManager.untamedUnits)
		{
				CharacterManager.aCurrentlySelectedUnit = untamedUnit;
				TileManager.aCurrentlySelectedTile = TileManager.pickRandomTile();
				TileManager.aSingleTileIsSelected = true;
				charManager.SendMessage("selectUnit", untamedUnit);
				charManager.SendMessage("move");
				charManager.SendMessage("deselectUnit");
		}
	}
	
	public IEnumerator untamedMove()
	{
		foreach (GameObject untamed in CharacterManager.untamedUnits)
		{
			// Select the character and the destination tile
			charManager.SendMessage("selectUnit", untamed);
			TileManager.aCurrentlySelectedTile = TileManager.pickRandomTile();
			TileManager.aSingleTileIsSelected = true;
			
			// Start the movement.
			yield return StartCoroutine("uMoveHelper");
			
			// Proceed once the movement has ended.
			CharacterManager.aCurrentlySelectedUnit.transform.position = destination;
			charManager.SendMessage("deselectTile");
			charManager.SendMessage("paintAttackableTilesAfterMove");
			aIsObjectMoving = false;
			
			if (CharacterManager.aSingleUnitIsSelected)
				CharacterManager.aRotationAfterMove = CharacterManager.aCurrentlySelectedUnit.transform.rotation;
			
			charManager.SendMessage("deselectUnit");
			Debug.Log(CharacterManager.aTurn);
			Debug.Log(CharacterManager.aTurnIsCompleted);
		}

	}
	
	// Move takes the currently selected unit and moves it to the currently selected tile.
	IEnumerator uMoveHelper()
	{
		Vector3 startTile = TileManager.getTileUnitIsStandingOn(CharacterManager.aCurrentlySelectedUnit);
				
		destination = TileManager.aCurrentlySelectedTile.transform.position;
				
		charManager.SendMessage("shortestPath");
		aIsObjectMoving = true;
				
		// Slide the unit to the location following the path, or directly if the distance is just one.
		if (aPath.Length > 1)
			iTween.MoveTo(CharacterManager.aCurrentlySelectedUnit, iTween.Hash("path", aPath, "time", 2.0f, "orienttopath", true));
		else
			iTween.MoveTo(CharacterManager.aCurrentlySelectedUnit, iTween.Hash("position", aPath[0], "time", 1.0f, "orienttopath", true));
				
		// Update hashtable and tags
				
		// The starting tile is made unoccupied.
		TileManager.occupiedTilesHT.Remove(startTile);
		TileManager.getTileAt(startTile).tag = "Tile";
					
		// The destination tile is marked occupied.
		TileManager.occupiedTilesHT.Add(destination, CharacterManager.aCurrentlySelectedUnit);
		TileManager.getTileAt(destination).tag = "OccupiedTile";
				
		// Set the movement flag to true
		aMovementHappened = true;
				
		destination.y = CharacterManager.aCurrentlySelectedUnitOriginalPosition.y;
				
		// This next loop ensures the routine finishes once the iTween animation has finished.
		do
		{
		yield return new WaitForSeconds(0.5f);
		} while (iTween.tweens.Count > 0);
				
	}
		
}

