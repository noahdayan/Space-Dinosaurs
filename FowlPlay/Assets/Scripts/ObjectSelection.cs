using UnityEngine;
using System.Collections;

/**
 * This script handles the selection behavior of units
 * When the user clicks on an object, it becomes selected. 
 * When he clicks it again, it becomes de-selected.
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
			if ((!CharacterManager.isBird(gameObject) && !gameObject.GetComponent<DinosaurUnitFunctionalityAndStats>().attackSpent && !gameObject.GetComponent<DinosaurUnitFunctionalityAndStats>().moveSpent) || CharacterManager.isBird(gameObject))
			{
				// FIRST - Click on an object
				
				// SECOND - Check that it is the object's turn to move (e.g. turn 1 for units belonging to team 1) 
				// 			and that it is not mid-turn.
				if (((transform.gameObject.tag == "Player1" && CharacterManager.aTurn == 1) || (transform.gameObject.tag == "Player2" && CharacterManager.aTurn == 3)) && !CharacterManager.aMidTurn)
				{
					// THIRD - Select the object only if it is not already selected and no objects are in movement.
					if (CharacterManager.aCurrentlySelectedUnit != gameObject && !ClickAndMove.aIsObjectMoving)
					{
						CharacterManager.aRotationBeforeMove = gameObject.transform.rotation;

						// If another object is already selected, we deselect it.
						if (CharacterManager.aSingleUnitIsSelected)
						{
							charManager.SendMessage("deselectUnit");
						}

						// FOURTH - Select the object.
						charManager.SendMessage("selectUnit", gameObject);
						if (CharacterManager.aTurn == 1)
							CharacterManager.bird1.SendMessage("SetOriginalMana");
						else if (CharacterManager.aTurn == 3)
							CharacterManager.bird2.SendMessage("SetOriginalMana");
					}

					else if (CharacterManager.aCurrentlySelectedUnit == gameObject && !ClickAndMove.aIsObjectMoving && CharacterManager.aInteractiveUnitIsSelected)
					{
						CharacterManager.aInteractUnit.SendMessage("UpdateColor");
						// Revert the interact unit's rotation.
						iTween.RotateTo(CharacterManager.aInteractUnit, CharacterManager.aCurrentlySelectedUnitOriginalRotation.eulerAngles, 2.0f);

						// Revert the attacker/tamer's rotation.
						iTween.RotateTo (CharacterManager.aCurrentlySelectedUnit, CharacterManager.aRotationBeforeMove.eulerAngles, 2.0f);

						CharacterManager.aCurrentlySelectedUnit.SendMessage("UpdateGuiStats");
						CharacterManager.aInteractiveUnitIsSelected = false;
						CharacterManager.aInteractUnit = null;

						charManager.SendMessage("deselectUnit");
					}

					// THIRD - If the object is already selected, then deselect it.
					else if (CharacterManager.aCurrentlySelectedUnit == gameObject && !ClickAndMove.aIsObjectMoving)
					{
						charManager.SendMessage("deselectUnit");
					}
				}

				// INTERACT UNIT SELECTION

				// SECOND - If it is not the object's turn, check to see that it is:
				//			1) mid-turn
				//				AND
				//			2) it belongs to the opposite team or is untamed. 
				else if (((transform.gameObject.tag == "Player1" && CharacterManager.aTurn == 3) || (transform.gameObject.tag == "Player2" && CharacterManager.aTurn == 1) || transform.gameObject.tag == "Enemy") && CharacterManager.aMidTurn)
				{

					// THIRD - If the object we're trying to select is already selected
					//			(i.e., it is already the interact unit), then we deselect it
					//			and return the attacking/taming unit to its original rotation.
					if (CharacterManager.aInteractUnit == gameObject)
					{
						CharacterManager.aInteractUnit.SendMessage("UpdateColor");
						// Revert the interact unit's rotation.
						iTween.RotateTo(CharacterManager.aInteractUnit, CharacterManager.aCurrentlySelectedUnitOriginalRotation.eulerAngles, 2.0f);

						// Revert the attacker/tamer's rotation.
						iTween.RotateTo (CharacterManager.aCurrentlySelectedUnit, CharacterManager.aRotationAfterMove.eulerAngles, 2.0f);

						CharacterManager.aCurrentlySelectedUnit.SendMessage("UpdateGuiStats");
						CharacterManager.aInteractiveUnitIsSelected = false;
						CharacterManager.aInteractUnit = null;
					}

					// THIRD - Else, the object we're trying to select is not selected, so let's select it.
					else
					{

						// FOURTH - We have to check that it is within "interact" range, i.e., it's within attacking
						//			or taming range.

						// Get the object's position.
						Vector3 unitsPosition = TileManager.getTileUnitIsStandingOn(gameObject);

						// And now perform the check.
						if(TileManager.tilesInMidTurnAttackRange.Contains(TileManager.getTileAt(unitsPosition)))
						{
							// FIFTH - If another interact unit is already selected, deselect it and revert its rotation to the original.
							if (CharacterManager.aInteractiveUnitIsSelected)
							{
								CharacterManager.aInteractUnit.SendMessage("UpdateColor");
								iTween.RotateTo(CharacterManager.aInteractUnit, CharacterManager.aCurrentlySelectedUnitOriginalRotation.eulerAngles, 2.0f);
								CharacterManager.aInteractUnit.SendMessage("UpdateGuiStats");
							}

							// Select the new interact unit.
							CharacterManager.aInteractiveUnitIsSelected = true;
							CharacterManager.aInteractUnit = gameObject;

							CharacterManager.aInteractUnit.SendMessage("UpdateGuiStats");

							if(CharacterManager.aCurrentlySelectedUnit == CharacterManager.bird1 && CharacterManager.aTurn == 1)
							{
								CharacterManager.bird1.SendMessage("CheckLegalMove", CharacterManager.aCurrentlySelectedUnit.GetComponent<BirdUnitFunctionalityAndStats>().attackCost);
							}
							else if(CharacterManager.aCurrentlySelectedUnit != CharacterManager.bird1 && CharacterManager.aTurn == 1)
							{
								CharacterManager.bird1.SendMessage("CheckLegalMove", CharacterManager.aCurrentlySelectedUnit.GetComponent<DinosaurUnitFunctionalityAndStats>().attackCost);
							}
							if(CharacterManager.aCurrentlySelectedUnit == CharacterManager.bird2 && CharacterManager.aTurn == 3)
							{
								CharacterManager.bird2.SendMessage("CheckLegalMove", CharacterManager.aCurrentlySelectedUnit.GetComponent<BirdUnitFunctionalityAndStats>().attackCost);
							}
							else if(CharacterManager.aCurrentlySelectedUnit != CharacterManager.bird2 && CharacterManager.aTurn == 3)
							{
								CharacterManager.bird2.SendMessage("CheckLegalMove", CharacterManager.aCurrentlySelectedUnit.GetComponent<DinosaurUnitFunctionalityAndStats>().attackCost);
							}
							CharacterManager.aInteractUnit.SendMessage("SelectedColor");
							// and rotate it to face the attacker/tamer
							Vector3 tileOne = TileManager.getTileUnitIsStandingOn(CharacterManager.aInteractUnit);
							Vector3 tileTwo = TileManager.getTileUnitIsStandingOn(CharacterManager.aCurrentlySelectedUnit);

							Vector3 newRotation = Quaternion.LookRotation(tileTwo - tileOne).eulerAngles;
							newRotation.x = CharacterManager.startRot.x;
							newRotation.z = CharacterManager.startRot.z;

							iTween.RotateTo(CharacterManager.aInteractUnit, newRotation, 1.0f);

							//CharacterManager.aCurrentlySelectedUnit.transform.rotation = Quaternion.Slerp(CharacterManager.aCurrentlySelectedUnit.transform.rotation, Quaternion.Euler(newRotation), Time.deltaTime * aSpeedOfRotation);

							//if (CharacterManager.aInteractUnit != CharacterManager.aCurrentlySelectedUnit)
							//{
							Vector3 opponentRotation = Quaternion.LookRotation(tileOne - tileTwo).eulerAngles;
							opponentRotation.x = CharacterManager.startRot.x;
							opponentRotation.z = CharacterManager.startRot.z;

							iTween.RotateTo(CharacterManager.aCurrentlySelectedUnit, opponentRotation, 1.0f);

							//CharacterManager.aInteractUnit.transform.rotation = Quaternion.Slerp(CharacterManager.aInteractUnit.transform.rotation, Quaternion.Euler(opponentRotation), Time.deltaTime * aSpeedOfRotation);
							//}
						}

					}

				}

				else if (((transform.gameObject.tag == "Player1" && CharacterManager.aTurn == 3) || (transform.gameObject.tag == "Player2" && CharacterManager.aTurn == 1) || transform.gameObject.tag == "Enemy"))
				{
					if (CharacterManager.aInteractUnit == gameObject)
					{
						CharacterManager.aInteractUnit.SendMessage("UpdateColor");
						// Revert the interact unit's rotation.
						iTween.RotateTo(CharacterManager.aInteractUnit, CharacterManager.aCurrentlySelectedUnitOriginalRotation.eulerAngles, 2.0f);

						// Revert the attacker/tamer's rotation.
						iTween.RotateTo (CharacterManager.aCurrentlySelectedUnit, CharacterManager.aRotationAfterMove.eulerAngles, 2.0f);

						CharacterManager.aCurrentlySelectedUnit.SendMessage("UpdateGuiStats");
						CharacterManager.aInteractiveUnitIsSelected = false;
						CharacterManager.aInteractUnit = null;
					}

					else if(CharacterManager.aSingleUnitIsSelected)
					{				
						// Get the object's position.
						Vector3 unitsPosition = TileManager.getTileUnitIsStandingOn(gameObject);

						// And now perform the check.
						if(TileManager.tilesInMidTurnAttackRange.Contains(TileManager.getTileAt(unitsPosition)))
						{
							// FIFTH - If another interact unit is already selected, deselect it and revert its rotation to the original.
							if (CharacterManager.aInteractiveUnitIsSelected)
							{
								CharacterManager.aInteractUnit.SendMessage("UpdateColor");
								iTween.RotateTo(CharacterManager.aInteractUnit, CharacterManager.aCurrentlySelectedUnitOriginalRotation.eulerAngles, 2.0f);
								CharacterManager.aInteractUnit.SendMessage("UpdateGuiStats");
							}

							// Select the new interact unit.
							CharacterManager.aInteractiveUnitIsSelected = true;
							CharacterManager.aInteractUnit = gameObject;

							CharacterManager.aInteractUnit.SendMessage("UpdateGuiStats");

							if(CharacterManager.aCurrentlySelectedUnit == CharacterManager.bird1 && CharacterManager.aTurn == 1)
							{
								CharacterManager.bird1.SendMessage("CheckLegalMove", CharacterManager.aCurrentlySelectedUnit.GetComponent<BirdUnitFunctionalityAndStats>().attackCost);
							}
							else if(CharacterManager.aCurrentlySelectedUnit != CharacterManager.bird1 && CharacterManager.aTurn == 1)
							{
								CharacterManager.bird1.SendMessage("CheckLegalMove", CharacterManager.aCurrentlySelectedUnit.GetComponent<DinosaurUnitFunctionalityAndStats>().attackCost);
							}
							if(CharacterManager.aCurrentlySelectedUnit == CharacterManager.bird2 && CharacterManager.aTurn == 3)
							{
								CharacterManager.bird2.SendMessage("CheckLegalMove", CharacterManager.aCurrentlySelectedUnit.GetComponent<BirdUnitFunctionalityAndStats>().attackCost);
							}
							else if(CharacterManager.aCurrentlySelectedUnit != CharacterManager.bird2 && CharacterManager.aTurn == 3)
							{
								CharacterManager.bird2.SendMessage("CheckLegalMove", CharacterManager.aCurrentlySelectedUnit.GetComponent<DinosaurUnitFunctionalityAndStats>().attackCost);
							}
							CharacterManager.aInteractUnit.SendMessage("SelectedColor");
							// and rotate it to face the attacker/tamer
							Vector3 tileOne = TileManager.getTileUnitIsStandingOn(CharacterManager.aInteractUnit);
							Vector3 tileTwo = TileManager.getTileUnitIsStandingOn(CharacterManager.aCurrentlySelectedUnit);

							Vector3 newRotation = Quaternion.LookRotation(tileTwo - tileOne).eulerAngles;
							newRotation.x = CharacterManager.startRot.x;
							newRotation.z = CharacterManager.startRot.z;

							iTween.RotateTo(CharacterManager.aInteractUnit, newRotation, 1.0f);

							//CharacterManager.aCurrentlySelectedUnit.transform.rotation = Quaternion.Slerp(CharacterManager.aCurrentlySelectedUnit.transform.rotation, Quaternion.Euler(newRotation), Time.deltaTime * aSpeedOfRotation);

							//if (CharacterManager.aInteractUnit != CharacterManager.aCurrentlySelectedUnit)
							//{
							Vector3 opponentRotation = Quaternion.LookRotation(tileOne - tileTwo).eulerAngles;
							opponentRotation.x = CharacterManager.startRot.x;
							opponentRotation.z = CharacterManager.startRot.z;

							iTween.RotateTo(CharacterManager.aCurrentlySelectedUnit, opponentRotation, 1.0f);

							//CharacterManager.aInteractUnit.transform.rotation = Quaternion.Slerp(CharacterManager.aInteractUnit.transform.rotation, Quaternion.Euler(opponentRotation), Time.deltaTime * aSpeedOfRotation);
							//}
						}
					}
				
					//if it's not interact or currently selected we update the stats here 
					//But updateGuiStats only checks for interact or currently selected. fixing that now...
					gameObject.SendMessage("UpdateGuiStats");
					ProgressBarGUI.show2 = true;
				}

			}
		
			// Select a friendly dino to add tameness to it.
			// The currently selected unit must be the bird and we must be in mid-turn the interact unit is a friendly dino
			if(CharacterManager.aMidTurn && CharacterManager.isBird(CharacterManager.aCurrentlySelectedUnit) && ((transform.gameObject.tag == "Player1" && CharacterManager.aTurn == 1) || (transform.gameObject.tag == "Player2" && CharacterManager.aTurn == 3)) && ((CharacterManager.aCurrentlySelectedUnit.tag.Equals("Player1") && CharacterManager.aTurn == 1) || (CharacterManager.aCurrentlySelectedUnit.tag.Equals("Player2") && CharacterManager.aTurn == 3)))
			{
				if(gameObject != CharacterManager.aCurrentlySelectedUnit)
				{
					// if it's already selected, deselect it.
					if (CharacterManager.aInteractUnit == gameObject)
					{
						CharacterManager.aInteractUnit.SendMessage("UpdateColor");
						// Revert the interact unit's rotation.
						iTween.RotateTo(CharacterManager.aInteractUnit, CharacterManager.aCurrentlySelectedUnitOriginalRotation.eulerAngles, 2.0f);
						
						// Revert the attacker/tamer's rotation.
						iTween.RotateTo (CharacterManager.aCurrentlySelectedUnit, CharacterManager.aRotationAfterMove.eulerAngles, 2.0f);
						CharacterManager.aCurrentlySelectedUnit.SendMessage("UpdateGuiStats");
						CharacterManager.aInteractiveUnitIsSelected = false;
						CharacterManager.aInteractUnit = null;
					}
				
					// Select the new interact unit.
					CharacterManager.aInteractiveUnitIsSelected = true;
					CharacterManager.aInteractUnit = gameObject;
					CharacterManager.aInteractUnit.SendMessage("UpdateGuiStats");
				
					if(CharacterManager.aCurrentlySelectedUnit == CharacterManager.bird1 && CharacterManager.aTurn == 1)
					{
						CharacterManager.bird1.SendMessage("CheckLegalMove", CharacterManager.aCurrentlySelectedUnit.GetComponent<BirdUnitFunctionalityAndStats>().attackCost);
					}
					else if(CharacterManager.aCurrentlySelectedUnit != CharacterManager.bird1 && CharacterManager.aTurn == 1)
					{
						CharacterManager.bird1.SendMessage("CheckLegalMove", CharacterManager.aCurrentlySelectedUnit.GetComponent<DinosaurUnitFunctionalityAndStats>().attackCost);
					}
					if(CharacterManager.aCurrentlySelectedUnit == CharacterManager.bird2 && CharacterManager.aTurn == 3)
					{
						CharacterManager.bird2.SendMessage("CheckLegalMove", CharacterManager.aCurrentlySelectedUnit.GetComponent<BirdUnitFunctionalityAndStats>().attackCost);
					}
					else if(CharacterManager.aCurrentlySelectedUnit != CharacterManager.bird2 && CharacterManager.aTurn == 3)
					{
						CharacterManager.bird2.SendMessage("CheckLegalMove", CharacterManager.aCurrentlySelectedUnit.GetComponent<DinosaurUnitFunctionalityAndStats>().attackCost);
					}
					CharacterManager.aInteractUnit.SendMessage("SelectedColor");
					// and rotate it to face the attacker/tamer
					Vector3 tileOne = TileManager.getTileUnitIsStandingOn(CharacterManager.aInteractUnit);
					Vector3 tileTwo = TileManager.getTileUnitIsStandingOn(CharacterManager.aCurrentlySelectedUnit);
					Vector3 newRotation = Quaternion.LookRotation(tileTwo - tileOne).eulerAngles;
					newRotation.x = CharacterManager.startRot.x;
					newRotation.z = CharacterManager.startRot.z;
					iTween.RotateTo(CharacterManager.aInteractUnit, newRotation, 1.0f);
					Vector3 opponentRotation = Quaternion.LookRotation(tileOne - tileTwo).eulerAngles;
					opponentRotation.x = CharacterManager.startRot.x;
					opponentRotation.z = CharacterManager.startRot.z;
	
					iTween.RotateTo(CharacterManager.aCurrentlySelectedUnit, opponentRotation, 1.0f);
				}
			}	
	}
}
