using UnityEngine;
using System.Collections;

public class ClickAndMove : MonoBehaviour
{
	
	void Start () 
	{
    	// Print out the starting position of the object to the console (at instantiation)
    	Debug.Log ("Starting position: ");
    	Debug.Log (transform.position);
	}


	// Update is called once per frame
  	void Update ()
	{
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

	}
	
}