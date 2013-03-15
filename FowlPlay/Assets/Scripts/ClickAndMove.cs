using UnityEngine;
using System.Collections;

public class ClickAndMove : MonoBehaviour
{	
	public float aSpeedOfMovement = 20.0f;
	public static bool aIsObjectMoving = false;
	public static bool aIsObjectRotating = false;
	public static bool aMovementHappened = false;
	public static bool fullHP = false;
	
	public static Vector3[] aPath;
	
	private Vector3 destination;
	
	private GameObject manager;
	
	void Start () 
	{
		manager = GameObject.Find("Character");
	}

	// Update is called once per frame
  	void Update ()
	{
		
	}
	
	
	IEnumerator move()
	{
		fullHP = false;
		
		// Start the movement.
		yield return StartCoroutine("moveHelper");
		
		// Proceed once the movement has ended.
		CharacterManager.aCurrentlySelectedUnit.transform.position = destination;
		manager.SendMessage("deselectTile");
		manager.SendMessage("paintAttackableTilesAfterMove");

		CharacterManager.aUnitsAndTilesHT.Remove(CharacterManager.aCurrentlySelectedUnit);
		CharacterManager.aUnitsAndTilesHT.Add(CharacterManager.aCurrentlySelectedUnit, TileManager.getTileAt(TileManager.getTileUnitIsStandingOn(CharacterManager.aCurrentlySelectedUnit)));
		aIsObjectMoving = false;
		
		CharacterManager.aMidTurn = true;
		
		// Check to see if there are any items at the destination.
		if (ItemManager.tilesWithItems.ContainsKey(TileManager.getTileUnitIsStandingOn(CharacterManager.aCurrentlySelectedUnit)))
		{
			Debug.Log("here2");
			GameObject item = (GameObject)ItemManager.tilesWithItems[TileManager.getTileUnitIsStandingOn(CharacterManager.aCurrentlySelectedUnit)];
			bool consume = false;
			
			if (item.tag.Equals("DinoChow") && !CharacterManager.isBird(CharacterManager.aCurrentlySelectedUnit))
			{
				CharacterManager.aCurrentlySelectedUnit.SendMessage("CheckHP");
				
				if (!fullHP)
				{
					consume = true;
					CharacterManager.aCurrentlySelectedUnit.SendMessage("RecoverHP",10);
				}
			}
			else if (item.tag.Equals("BirdSeed") && CharacterManager.isBird(CharacterManager.aCurrentlySelectedUnit))
			{
				CharacterManager.aCurrentlySelectedUnit.SendMessage("CheckHP");
				
				if (!fullHP)
				{
					consume = true;
					CharacterManager.aCurrentlySelectedUnit.SendMessage("RecoverHP",10);
				}	
			}
			else if (item.tag.Equals("DinoCoOil") && !CharacterManager.isBird(CharacterManager.aCurrentlySelectedUnit))
			{	
				consume = true;
				// plug-in functionality here	
			}
			
			if (consume)
			{
				// Destroy the object and update hashtable. Gotta use DestroyImmediate, because Unity won't let you Destroy an asset.
				GameObject.Destroy((Object)ItemManager.tilesWithItems[TileManager.getTileUnitIsStandingOn(CharacterManager.aCurrentlySelectedUnit)]);
				ItemManager.tilesWithItems.Remove(TileManager.getTileUnitIsStandingOn(CharacterManager.aCurrentlySelectedUnit));
			}
		}
		
		if (CharacterManager.aSingleUnitIsSelected)
			CharacterManager.aRotationAfterMove = CharacterManager.aCurrentlySelectedUnit.transform.rotation;

	}
	
	// Move takes the currently selected unit and moves it to the currently selected tile.
	IEnumerator moveHelper()
	{
		if (CharacterManager.aSingleUnitIsSelected)
		{
			if (TileManager.aSingleTileIsSelected)
			{	
				Vector3 startTile = TileManager.getTileUnitIsStandingOn(CharacterManager.aCurrentlySelectedUnitOriginalPosition);
				
				destination = TileManager.aCurrentlySelectedTile.transform.position;
				
				manager.SendMessage("shortestPath");
				aIsObjectMoving = true;
				
				// Slide the unit to the location following the path, or directly if the distance is just one.
				if (aPath.Length > 1)
				{
					float movementSpeed = 30.0f;
					
					if (!CharacterManager.isBird(CharacterManager.aCurrentlySelectedUnit))
					{	
						switch (CharacterManager.aCurrentlySelectedUnit.GetComponent<DinosaurUnitFunctionalityAndStats>().species)
						{	
							case "tyrannosaur" :
							{
								movementSpeed = 10.0f;
								break;
							}
							case "anquilosaurus" :
							{
								movementSpeed = 10.0f;
								break;
							}
							case "pterodactyl" :
							{
								movementSpeed = 35.0f;
								break;
							}
							case "triceratops" :
							{
								movementSpeed = 10.0f;
								break;
							}
							case "velociraptor" :
							{
								movementSpeed = 40.0f;
								break;
							}
						}
					}
					
					iTween.MoveTo(CharacterManager.aCurrentlySelectedUnit, iTween.Hash("path", aPath, "speed", movementSpeed, "orienttopath", true, "lookahead", 0.1f, "easetype", "linear"));
				}
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
				
				bool hasArrived = false;
				
				// This next loop ensures the routine finishes once the iTween animation has finished.
				do
				{
					yield return new WaitForSeconds(0.5f);
				} while (iTween.tweens.Count > 0);
				
			}
		}
	}
	
}