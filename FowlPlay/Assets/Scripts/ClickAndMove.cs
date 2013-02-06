using UnityEngine;
using System.Collections;

public class ClickAndMove : MonoBehaviour
{	
	public float aSpeedOfMovement = 4.0f;
	public static bool aIsObjectMoving = false;
	
	void Start () 
	{
    	// Print out the starting position of the object to the console (at instantiation)
    	Debug.Log ("Starting position: ");
    	Debug.Log (transform.position);
		
		// Apply script to tiles
		
	}


	// Update is called once per frame
  	void Update ()
	{
		// Uncomment if using a robot
		/**
		if(!AutoMove.aRobotsTurn)
		{
		*/
			if (CharacterManager.aSingleUnitIsSelected)
			{
				if (TileManager.aSingleTileIsSelected)
				{
					aIsObjectMoving = true;
					
					Vector3 destination = TileManager.aCurrentlySelectedTile.transform.position;
					destination.y = CharacterManager.aCurrentlySelectedUnit.transform.position.y;
					
					// slide to location
					CharacterManager.aCurrentlySelectedUnit.transform.position += (destination - CharacterManager.aCurrentlySelectedUnit.transform.position).normalized * aSpeedOfMovement * Time.deltaTime;
					
					// check to see if object has reached destination tile. if so, stop movement.
					if ( (Mathf.Abs(CharacterManager.aCurrentlySelectedUnit.transform.position.x - destination.x) < 0.5) && (Mathf.Abs(CharacterManager.aCurrentlySelectedUnit.transform.position.z - destination.z) < 0.5))
					{
						CharacterManager.aCurrentlySelectedUnit.transform.position = destination;
						CharacterManager.deselect();
						aIsObjectMoving = false;
					
						TileManager.deselect();
						CharacterManager.switchTurn();
						
						// Uncomment if using a robot
						/**
						SendMessage("pickRandomTile");
						SendMessage("selectTile", AutoMove.destTile);
						AutoMove.aRobotsTurn = true;
						*/
					}
				}
			}
		// Uncomment if using a robot
		/**
		}
		*/
	}
}