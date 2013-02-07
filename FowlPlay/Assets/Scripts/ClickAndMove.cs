using UnityEngine;
using System.Collections;

public class ClickAndMove : MonoBehaviour
{	
	public float aSpeedOfMovement = 4.0f;
	public float aSpeedOfRotation = 180f;
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
		if(!AutoMove.aRobotsTurn)
		{
			if (CharacterManager.aSingleUnitIsSelected)
			{
				if (TileManager.aSingleTileIsSelected)
				{
					aIsObjectMoving = true;
					
					Vector3 destination = TileManager.aCurrentlySelectedTile.transform.position;
					destination.y = CharacterManager.aCurrentlySelectedUnit.transform.position.y;
					
					// rotate character
					//Vector3 startPos = CharacterManager.aCurrentlySelectedUnit.transform.position;
					float angle = -Mathf.Atan2(destination.z - CharacterManager.startPos.z, destination.x - CharacterManager.startPos.x) * Mathf.Rad2Deg;
					Debug.Log(angle);
					//Vector3 startRot = CharacterManager.aCurrentlySelectedUnit.transform.rotation.eulerAngles;
					if(Mathf.Abs(CharacterManager.aCurrentlySelectedUnit.transform.rotation.eulerAngles.y - CharacterManager.startRot.y) <= Mathf.Abs(angle))
					{
						CharacterManager.aCurrentlySelectedUnit.transform.Rotate(0, Mathf.Sign(angle) * aSpeedOfRotation * Time.deltaTime, 0, Space.World);
					}
					
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
						SendMessage("pickRandomTile");
						//SendMessage("selectTile", AutoMove.destTile);
						AutoMove.aRobotsTurn = true;
					}
				}
			}
		// Uncomment if using a robot
		}
	}
}