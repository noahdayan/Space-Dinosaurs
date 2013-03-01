using UnityEngine;
using System.Collections;

public class ClickAndMove : MonoBehaviour
{	
	public float aSpeedOfMovement = 20.0f;
	public static bool aIsObjectMoving = false;
	public static bool aIsObjectRotating = false;
	
	private Vector3 destination;
	
	private GameObject manager;
	
	void Start () 
	{
		manager = GameObject.Find("Character");
	}

	// Update is called once per frame
  	void Update ()
	{
		if (aIsObjectMoving)
		{
			if ( (Mathf.Abs(CharacterManager.aCurrentlySelectedUnit.transform.position.x - destination.x) < 0.5) && (Mathf.Abs(CharacterManager.aCurrentlySelectedUnit.transform.position.z - destination.z) < 0.5))
			{
				CharacterManager.aCurrentlySelectedUnit.transform.position = destination;
				manager.SendMessage("deselectTile");
				CharacterManager.aMidTurn = true;
				manager.SendMessage("paintAttackableTilesAfterMove");
				aIsObjectMoving = false;
				Debug.Log("Movement ended. Unit at: " + CharacterManager.aCurrentlySelectedUnit.transform.position);
			}
		}
		
	}
	
	
	// Move takes the currently selected unit and moves it to the currently selected tile.
	void move()
	{
		if (CharacterManager.aSingleUnitIsSelected)
		{
			if (TileManager.aSingleTileIsSelected)
			{	
				Vector3 startTile = CharacterManager.aCurrentlySelectedUnitOriginalPosition;
				startTile.y = 2.0f;
				
				destination = TileManager.aCurrentlySelectedTile.transform.position;
				//destination.y = CharacterManager.aCurrentlySelectedUnit.transform.position.y;
				
				Debug.Log("Initiate movement. Unit moving to: " + destination);
				// Get the path that is to be followed.
				Vector3[] path = TileManager.findPath(TileManager.getTileAt(startTile), TileManager.getTileAt(destination));
				
				aIsObjectMoving = true;
				
				// Slide the unit to the location following the path, or directly if the distance is just one.
				if (path.Length > 1)
					iTween.MoveTo(CharacterManager.aCurrentlySelectedUnit, iTween.Hash("path", path, "speed", 18.0f)); 
				else
					iTween.MoveTo(CharacterManager.aCurrentlySelectedUnit, destination, 1.0f);
				
			}
		}
		
	}
	
}