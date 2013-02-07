using UnityEngine;
using System.Collections;

/**
 * This script handles the selection behavior of objects (incl. characters and
 * tiles). When the user clicks on an object, it becomes selected. When he clicks
 * it again, it becomes de-selected.
 **/

public class RoboSelection : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
			// check that it is the object's turn to move
			if (CharacterManager.aTurn == 3)
			{		
				//CharacterManager.selectUnit(gameObject);
			}
				
				// de-select the object, but only if it's not moving.
				else if (CharacterManager.aCurrentlySelectedUnit == gameObject)
				{
					if (!ClickAndMove.aIsObjectMoving)
					{
						//CharacterManager.deselect();
					}
				}
		}
			
	}

