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
			}
		}
		
	}
	
	void move()
	{
		if (CharacterManager.aSingleUnitIsSelected)
		{
			if (TileManager.aSingleTileIsSelected)
			{				
				destination = TileManager.aCurrentlySelectedTile.transform.position;
				destination.y = CharacterManager.aCurrentlySelectedUnit.transform.position.y;
		
				Vector3[] path = TileManager.findPath(TileManager.getTileAt(CharacterManager.aCurrentlySelectedUnitOriginalPosition), TileManager.getTileAt(destination));
				
				aIsObjectMoving = true;
				if (path.Length > 1)
					iTween.MoveTo(CharacterManager.aCurrentlySelectedUnit, iTween.Hash("path", path, "time", 3.0f)); 
				else
					iTween.MoveTo(CharacterManager.aCurrentlySelectedUnit, destination, 3.0f);
				
			}
		}
		
	}
}