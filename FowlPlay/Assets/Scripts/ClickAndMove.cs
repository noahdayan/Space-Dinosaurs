using UnityEngine;
using System.Collections;

public class ClickAndMove : MonoBehaviour
{	
	public float aSpeedOfMovement = 4.0f;
	public float aSpeedOfRotation = 180f;
	public static bool aIsObjectMoving = false;
	public static bool aIsObjectRotating = false;
	
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
		if(CharacterManager.aTurn == 1 || CharacterManager.aTurn == 3)
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
					if(Mathf.Abs(CharacterManager.aCurrentlySelectedUnit.transform.rotation.eulerAngles.y - CharacterManager.startRot.y) <= Mathf.Abs(angle)-2)
					{
						Vector3 newRotation = Quaternion.LookRotation(TileManager.aCurrentlySelectedTile.transform.position - CharacterManager.aCurrentlySelectedUnit.transform.position).eulerAngles;
        				newRotation.x = 270;
        				//newRotation.z = 0;
        				CharacterManager.aCurrentlySelectedUnit.transform.rotation = Quaternion.Slerp(CharacterManager.aCurrentlySelectedUnit.transform.rotation, Quaternion.Euler(newRotation), Time.deltaTime);
						
						Debug.Log("Left side: " + Mathf.Abs(CharacterManager.aCurrentlySelectedUnit.transform.rotation.eulerAngles.y - CharacterManager.startRot.y));
						Debug.Log("Right side: " + Mathf.Abs(angle));
						
						aIsObjectRotating = true;
						//CharacterManager.aCurrentlySelectedUnit.transform.Rotate(0, Mathf.Sign(angle) * aSpeedOfRotation * Time.deltaTime, 0, Space.World);
						//CharacterManager.aCurrentlySelectedUnit.transform.Rotation(0, Mathf.Sign(angle) * aSpeedOfRotation * Time.deltaTime, 0, Space.World);
					}
					else
					{
						aIsObjectRotating = false;
					}
					
					if(!aIsObjectRotating)
					{
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
							
							//SendMessage("selectTile", AutoMove.destTile);
							//AutoMove.aRobotsTurn = true;
						}
					}
				}
			}
		// Uncomment if using a robot
		}
	}
}