using UnityEngine;
using System.Collections;

public class ClickAndMove : MonoBehaviour
{
	
	// This will eventually be an array that stores all tiles.
	// For demo purposes, it is just one.
	public GameObject destinationTile;
	
	public ObjectSelection aSelfObjectSelection;
	public TileSelection aTargetObjectSelection;
	
	void Start () 
	{
    	// Print out the starting position of the object to the console (at instantiation)
    	Debug.Log ("Starting position: ");
    	Debug.Log (transform.position);
	}


	// Update is called once per frame
  	void Update ()
	{
	
		if (aSelfObjectSelection.isObjectSelected())
		{
			if (aTargetObjectSelection.isObjectSelected())
			{
				Vector3 destination = destinationTile.transform.position;
				destination.y = transform.position.y;
				transform.position = destination;
			}
			//Debug.Log ("It works!");
		}
		
    	/*
    	 * if(destinationTile.isObjectSelected())
			{
				
			}
    	 * 
    	 * 
		// True when the player clicks the left mouse button.
    	if (Input.GetMouseButtonDown(0))
   		{
      		Debug.Log("Left mouse button pressed.");
      		Vector3 newPosition = transform.position;
      
			// Get mouse x and y values
			newPosition.x = Input.mousePosition.x;
      		newPosition.y = Input.mousePosition.y;

      		// Update location of object with the coordinates from above.
      		transform.position = newPosition;
			
			// Print new location
      		Debug.Log (transform.position);

    	}
		*/
	}
	
}