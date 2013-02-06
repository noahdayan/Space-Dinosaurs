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
				if (CharacterManager.aCurrentlySelectedUnit != gameObject)
				{
					CharacterManager.selectUnit(gameObject);
					aObjectIsSelected = true;
				}
				
				// de-select the object, but only if it's not moving.
				else if (CharacterManager.aCurrentlySelectedUnit == gameObject)
				{
					if (!ClickAndMove.aIsObjectMoving)
					{
						CharacterManager.deselect();
						aObjectIsSelected = false;	
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
}
