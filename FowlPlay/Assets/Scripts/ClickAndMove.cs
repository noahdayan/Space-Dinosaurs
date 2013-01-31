using UnityEngine;
using System.Collections;

public class ClickAndMove : MonoBehaviour
{	
	public ObjectSelection aSelfObjectSelection;
	
	public TileManager aTileManager;
	
	public float aSpeedOfMovement = 4.0f;
	private bool aIsObjectMoving = false;
	
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
	
		if (aSelfObjectSelection.isObjectSelected())
		{
			if (aTileManager.tileIsSelected())
			{
				aIsObjectMoving = true;
				
				Vector3 destination = aTileManager.aCurrentlySelectedTile.transform.position;
				destination.y = transform.position.y;
				
				// slide to location
				transform.position += (destination - transform.position).normalized * aSpeedOfMovement * Time.deltaTime;
				
				// check to see if object has reached destination tile. if so, stop movement.
				if ( (Mathf.Abs(transform.position.x - destination.x) < 0.5) && (Mathf.Abs(transform.position.z - destination.z) < 0.5))
				{
					transform.position = destination;
					aSelfObjectSelection.deselectObject();
					aIsObjectMoving = false;
				}
			}
		}
	}
	
	public bool isObjectMoving()
	{
		return aIsObjectMoving;
	}
	
}