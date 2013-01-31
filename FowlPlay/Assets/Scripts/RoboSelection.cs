using UnityEngine;
using System.Collections;

/**
 * This script handles the selection behavior of objects (incl. characters and
 * tiles). When the user clicks on an object, it becomes selected. When he clicks
 * it again, it becomes de-selected.
 **/

public class RoboSelection : MonoBehaviour {
	
	private bool aObjectIsSelected = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		
				// select the object
				if (!aObjectIsSelected && AutoMove.aRobotsTurn)
				{
	        		selectObject();
					//Debug.Log("Object selected.");
				}
				
				// de-select the object, but only if it's not moving.
				else if (aObjectIsSelected && !AutoMove.aRobotsTurn)
				{
					if (!AutoMove.aIsObjectMoving)
					{
						deselectObject();
					}
				}
			
	}
	
	public void deselectObject()
	{
		renderer.material.color = Color.red;
		aObjectIsSelected = false;	
	}
	
	public void selectObject()
	{
		renderer.material.color = Color.yellow;
		aObjectIsSelected = true;	
	}
}
