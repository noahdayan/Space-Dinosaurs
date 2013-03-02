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
	
	private GameObject charManager;
	public float aSpeedOfRotation = 10.0f;
	
	// Use this for initialization
	void Start () {
		charManager = GameObject.Find("Character");
	}
	
	// Update is called once per frame
	void Update ()
	{

	}
	
	void OnMouseDown()
	{
		// check that it is the object's turn to move and that we're not mid-turn.
		if (((transform.gameObject.tag == "Player1" && CharacterManager.aTurn == 1) || (transform.gameObject.tag == "Player2" && CharacterManager.aTurn == 3)) && !CharacterManager.aMidTurn)
		{
			// select the object only if it is not selected and no objects are in movement
			if (CharacterManager.aCurrentlySelectedUnit != gameObject && !ClickAndMove.aIsObjectMoving)
			{
				// deselect the previously selected unit
				if (CharacterManager.aSingleUnitIsSelected)
				{
					charManager.SendMessage("deselectUnit");
				}
				
				// select the new unit
				charManager.SendMessage("selectUnit", gameObject);
			}
			
			// de-select the object, but only if it's not moving.
			else if (CharacterManager.aCurrentlySelectedUnit == gameObject)
			{
				if (!ClickAndMove.aIsObjectMoving)
				{
					charManager.SendMessage("deselectUnit");
				}
			}
		}
			
		// If it's not the object's turn, then check to see whether it is being attacked/tamed. 
		else if (((transform.gameObject.tag == "Player1" && CharacterManager.aTurn == 3) || (transform.gameObject.tag == "Player2" && CharacterManager.aTurn == 1) || transform.gameObject.tag == "Enemy") && CharacterManager.aMidTurn)
		{
			// if it is already the interact unit, deselect it.
			if (CharacterManager.aInteractUnit == gameObject)
			{
				//CharacterManager.aInteractUnit.renderer.material.color = Color.blue;
				CharacterManager.aInteractUnit.GetComponentInChildren<Renderer>().renderer.material.color = Color.blue;
				CharacterManager.aInteractUnit.transform.rotation = CharacterManager.aInteractUnitOriginalRotation;
				CharacterManager.aInteractiveUnitIsSelected = false;
				CharacterManager.aInteractUnit = null;
				//CharacterManager.aInteractUnit.transform.rotation = Quaternion.Slerp(CharacterManager.aInteractUnit.transform.rotation, CharacterManager.aInteractUnitOriginalRotation, Time.deltaTime * 50.0f);
			}
			
			// select the object only if it is mid-turn
			else if (CharacterManager.aMidTurn)
			{
				// check to see if it's in range
				Vector3 unitsPosition = gameObject.transform.position;
				unitsPosition.y = 2.0f;

				if(TileManager.tilesInMidTurnAttackRange.Contains(TileManager.getTileAt(unitsPosition)))
				{
					// if another interact unit is already selected, deselect it and revert its rotation to the original.
					if (CharacterManager.aInteractiveUnitIsSelected)
					{
						//CharacterManager.aInteractUnit.renderer.material.color = Color.blue;
						CharacterManager.aInteractUnit.GetComponentInChildren<Renderer>().renderer.material.color = Color.blue;
						CharacterManager.aInteractUnit.transform.rotation = CharacterManager.aInteractUnitOriginalRotation;
						//CharacterManager.aInteractUnit.transform.rotation = Quaternion.Slerp(CharacterManager.aInteractUnit.transform.rotation, CharacterManager.aInteractUnitOriginalRotation, Time.deltaTime * 50.0f);
					}
					
					// select the new interact unit.
					CharacterManager.aInteractiveUnitIsSelected = true;
					CharacterManager.aInteractUnit = gameObject;
					CharacterManager.aInteractUnitOriginalRotation = gameObject.transform.rotation;
					//gameObject.renderer.material.color = Color.red;
					gameObject.GetComponentInChildren<Renderer>().renderer.material.color = Color.red;
					
				}
				
			}
		}
	}
}
