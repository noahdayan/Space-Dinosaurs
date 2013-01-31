using UnityEngine;
using System.Collections;

/**
 * This script handles the selection behavior of objects (incl. characters and
 * tiles). When the user clicks on an object, it becomes selected. When he clicks
 * it again, it becomes de-selected.
 **/

public class ObjectSelection : MonoBehaviour {
	
	private bool aMouseHoveringOnObject = false;
	public static bool aObjectIsSelected = false;
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown(0) && aMouseHoveringOnObject)
	   		{
				// select the object
				if (!aObjectIsSelected)
				{
	        		selectObject();
					//Debug.Log("Object selected.");
				}
				
				// de-select the object, but only if it's not moving.
				else if (aObjectIsSelected)
				{
					if (!ClickAndMove.aIsObjectMoving)
					{
						deselectObject();
					}
				}
			}
	}
	
	void OnMouseEnter() 
	{
		aMouseHoveringOnObject = true;
		//Debug.Log("Object entered.");	
    }
	
	void OnMouseExit()
	{
		aMouseHoveringOnObject = false;
		//Debug.Log("Object exited.");
	}
	
	public void deselectObject()
	{
		renderer.material.color = Color.blue;
		aObjectIsSelected = false;	
	}
	
	public void selectObject()
	{
		renderer.material.color = Color.yellow;
		aObjectIsSelected = true;	
	}
}
