using UnityEngine;
using System.Collections;

public class ClickAndMove : MonoBehaviour
{
	
	// This will eventually be an array that stores all tiles.
	// For demo purposes, it is just one.
	public GameObject destinationTile;
	
	public ObjectSelection aSelfObjectSelection;
	public TileSelection aTargetObjectSelection;
	
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
			if (aTargetObjectSelection.isObjectSelected())
			{
				aIsObjectMoving = true;
				
				Vector3 destination = destinationTile.transform.position;
				destination.y = transform.position.y;
				
				// teleport
				//transform.position = destination;
				
				// slide to location
				transform.position += (destination - transform.position).normalized * aSpeedOfMovement * Time.deltaTime;
				
				// check to see if object has reached destination tile. if so, stop movement.
				if ( (Mathf.Abs(transform.position.x - destination.x) < 0.023) && (Mathf.Abs(transform.position.z - destination.z) < 0.023))
				{
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