using UnityEngine;
using System.Collections;

/**
 * This script handles the selection behavior of units
 * When the user clicks on an object, it becomes selected. 
 * When he clicks it again, it becomes de-selected.
 * 
 * It uses OnMouseEnter instead of OnMouseDown because the game makes use of two cameras.
 **/

public class ObjectSelection : MonoBehaviour {
	
	private bool aMouseHoveringOnObject = false;
	private GameObject charManager;
	
	// Use this for initialization
	void Start () {
		charManager = GameObject.Find("Character");
	}
	
	// Update is called once per frame
	void Update ()
	{
		// check that the mouse is on and clicked
		if (Input.GetMouseButtonDown(0) && aMouseHoveringOnObject)
	   	{
			// check that it is the object's turn to move
			if (((transform.gameObject.tag == "Player1" && CharacterManager.aTurn == 1) || (transform.gameObject.tag == "Player2" && CharacterManager.aTurn == 3)) && !CharacterManager.aMidTurn)
			{
				// select the object only if it is not selected and no objects are in movement
				if (CharacterManager.aCurrentlySelectedUnit != gameObject && !ClickAndMove.aIsObjectMoving)
				{
					if (CharacterManager.aSingleUnitIsSelected)
					{
						charManager.SendMessage("deselectUnit");
						gameObject.GetComponentInChildren<Camera>().camera.enabled = false;
					}
					charManager.SendMessage("selectUnit", gameObject);
					gameObject.GetComponentInChildren<Camera>().camera.enabled = true;
				}
				
				// de-select the object, but only if it's not moving.
				else if (CharacterManager.aCurrentlySelectedUnit == gameObject)
				{
					if (!ClickAndMove.aIsObjectMoving)
					{
						charManager.SendMessage("deselectUnit");
						gameObject.GetComponentInChildren<Camera>().camera.enabled = false;
					}
				}
			}
			
			// If it's not the object's turn, then check to see whether it is being attacked/tamed. 
			else// if ((transform.gameObject.tag == "Player1" && CharacterManager.aTurn == 2) || (transform.gameObject.tag == "Player2" && CharacterManager.aTurn == 4) || transform.gameObject.tag == "Enemy")
			{
				// select the object only if it is mid-turn
				if (CharacterManager.aMidTurn)
				{
					// check to see if it's in range
					Vector3 unitsPosition = gameObject.transform.position;
					unitsPosition.y = 2.0f;
					
					Debug.Log("midrange tiles size = " + TileManager.tilesInMidTurnAttackRange.Count);

					if(TileManager.tilesInMidTurnAttackRange.Contains(TileManager.getTileAt(unitsPosition)))
					{
						CharacterManager.aInteractiveUnitIsSelected = true;
						CharacterManager.aInteractUnit = gameObject;
						gameObject.renderer.material.color = Color.red;

						//charManager.SendMessage("attack");
					}
					
				}
			}
		}
	}
	
	void OnMouseEnter() 
	{
		aMouseHoveringOnObject = true;	
    }
	
	void OnMouseExit()
	{
		aMouseHoveringOnObject = false;
	}
}
