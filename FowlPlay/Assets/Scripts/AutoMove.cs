using UnityEngine;
using System.Collections;

public class AutoMove : MonoBehaviour {
	
	public float aSpeedOfMovement = 20.0f;
	public static GameObject destTile;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (CharacterManager.aTurn == 2 || CharacterManager.aTurn == 4)
		{
				CharacterManager.selectUnit(gameObject);
				ClickAndMove.aIsObjectMoving = true;
				
				Vector3 destination = destTile.transform.position;
				destination.y = transform.position.y;
				
				// slide to location
				transform.position += (destination - transform.position).normalized * aSpeedOfMovement * Time.deltaTime;
				
				// check to see if object has reached destination tile. if so, stop movement.
					if ( (Mathf.Abs(CharacterManager.aCurrentlySelectedUnit.transform.position.x - destination.x) < 0.5) && (Mathf.Abs(CharacterManager.aCurrentlySelectedUnit.transform.position.z - destination.z) < 0.5))
					{
						CharacterManager.aCurrentlySelectedUnit.transform.position = destination;
						CharacterManager.deselect();
						ClickAndMove.aIsObjectMoving = false;
						
						TileManager.deselect();
						CharacterManager.switchTurn();
					}
			
		}
	}
}
