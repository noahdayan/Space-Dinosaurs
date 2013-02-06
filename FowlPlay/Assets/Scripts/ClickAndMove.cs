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
			if (ObjectSelection.aObjectIsSelected)
			{
				if (TileManager.aSingleTileIsSelected)
				{
					aIsObjectMoving = true;
					
					Vector3 destination = TileManager.aCurrentlySelectedTile.transform.position;
					destination.y = transform.position.y;
					
					// slide to location
					transform.position += (destination - transform.position).normalized * aSpeedOfMovement * Time.deltaTime;
					
					// check to see if object has reached destination tile. if so, stop movement.
					if ( (Mathf.Abs(transform.position.x - destination.x) < 0.5) && (Mathf.Abs(transform.position.z - destination.z) < 0.5))
					{
						transform.position = destination;
						SendMessage("deselectObject");
						aIsObjectMoving = false;
						
						
						SendMessage("pickRandomTile");
						SendMessage("selectTile", AutoMove.destTile);
						AutoMove.aRobotsTurn = true;
					}
				}
			}
		// Uncomment if using a robot
		/**
		}
		*/
	}
}