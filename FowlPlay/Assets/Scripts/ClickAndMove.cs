using UnityEngine;
using System.Collections;

public class ClickAndMove : MonoBehaviour
{	
	public float aSpeedOfMovement = 20.0f;
	public float aSpeedOfRotation = 10.0f;
	public static bool aIsObjectMoving = false;
	public static bool aIsObjectRotating = false;
	
	private GameObject manager;
	
	void Start () 
	{
		manager = GameObject.Find("Character");
	}

	// Update is called once per frame
  	void Update ()
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
				//float angle = -Mathf.Atan2(destination.z - CharacterManager.startPos.z, destination.x - CharacterManager.startPos.x) * Mathf.Rad2Deg;
				//Vector3 startRot = CharacterManager.aCurrentlySelectedUnit.transform.rotation.eulerAngles;
				//if(Mathf.Abs(CharacterManager.aCurrentlySelectedUnit.transform.rotation.eulerAngles.y - CharacterManager.startRot.y) <= Mathf.Abs(angle)-2)

				aIsObjectRotating = true;
				Vector3 newRotation = Quaternion.LookRotation(TileManager.aCurrentlySelectedTile.transform.position - CharacterManager.aCurrentlySelectedUnit.transform.position).eulerAngles;
				newRotation.x = CharacterManager.startRot.x;
				newRotation.z = CharacterManager.startRot.z;
				CharacterManager.aCurrentlySelectedUnit.transform.rotation = Quaternion.Slerp(CharacterManager.aCurrentlySelectedUnit.transform.rotation, Quaternion.Euler(newRotation), Time.deltaTime * aSpeedOfRotation);
				
				//CharacterManager.aCurrentlySelectedUnit.transform.Rotate(0, Mathf.Sign(angle) * aSpeedOfRotation * Time.deltaTime, 0, Space.World);
				if(CharacterManager.aCurrentlySelectedUnit.transform.rotation.eulerAngles.y >= newRotation.y - 2)
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
						manager.SendMessage("deselct");
						CharacterManager.deselect();
						aIsObjectMoving = false;
						CharacterManager.switchTurn();
					}
				}
			}
		}
	}
}