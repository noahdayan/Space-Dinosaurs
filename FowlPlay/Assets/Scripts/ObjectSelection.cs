using UnityEngine;
using System.Collections;

/**
 * This script handles the selection behavior of objects (incl. characters and
 * tiles). When the user clicks on an object, it becomes selected. When he clicks
 * it again, it becomes de-selected.
 **/

public class ObjectSelection : MonoBehaviour {
	
	private bool aMouseHoveringOnObject = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		// check that the mouse is on and clicked
		if (Input.GetMouseButtonDown(0) && aMouseHoveringOnObject)
	   	{
			// check that it is the object's turn to move
			if ((transform.gameObject.tag == "Player1" && CharacterManager.aTurn == 1) || (transform.gameObject.tag == "Player2" && CharacterManager.aTurn == 3))
			{
				// select the object only if it is not selected and no objects are in movement
				if (CharacterManager.aCurrentlySelectedUnit != gameObject && !ClickAndMove.aIsObjectMoving)
				{
					if (CharacterManager.aSingleUnitIsSelected)
						CharacterManager.deselect();
					
					CharacterManager.selectUnit(gameObject);
				}
				
				// de-select the object, but only if it's not moving.
				else if (CharacterManager.aCurrentlySelectedUnit == gameObject)
				{
					if (!ClickAndMove.aIsObjectMoving)
					{
						CharacterManager.deselect();
					}
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
