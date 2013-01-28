using UnityEngine;
using System.Collections;

public class ClickAndMove : MonoBehaviour
{
	public bool mouseOn = false;
	public bool selected = false;
	
	void Start () 
	{
    	// Print out the starting position of the object to the console (at instantiation)
    	Debug.Log ("Starting position: ");
    	Debug.Log (transform.position);
	}


	// Update is called once per frame
  	void Update ()
	{
		if (Input.GetMouseButtonDown(0) && mouseOn)
   		{
			if (!selected)
			{
        		renderer.material.color = Color.red;
				selected = true;
				Debug.Log("Object selected");
			}
			
			else if (selected)
			{
				renderer.material.color = Color.blue;
				selected = false;
				Debug.Log("Object de-selected.");
			}
		}	
    	/*
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
	
	void OnMouseEnter() 
	{
		Debug.Log("Object entered.");
		mouseOn = true;
		
    }
	
	void OnMouseExit()
	{
		Debug.Log("Object exited.");
		mouseOn = false;
	}
	
}