using UnityEngine;
using System.Collections;

public class TileSelection : MonoBehaviour {
	
	private GameObject tileManager;
	public static bool moveIsSpent = false;
	
	// Use this for initialization
	void Start () {
		tileManager = GameObject.Find("Character");
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void OnMouseDown()
	{
		if (CharacterManager.aSingleUnitIsSelected)
		{
			CharacterManager.aCurrentlySelectedUnit.SendMessage("SendMoveSpentStatus");
		}
		
		if(PlayerFunctionalityAndStats.isLegalMove && !moveIsSpent)
		{
			// select the tile and move the currently selected unit towards it.
			if (TileManager.aCurrentlySelectedTile != gameObject && !CharacterManager.aMidTurn && gameObject.renderer.sharedMaterial.name.Equals("RangeBlue"))
			{
				if (CharacterManager.aSingleUnitIsSelected && !ClickAndMove.aIsObjectMoving)
				{
					tileManager.SendMessage("selectTile", gameObject);
					tileManager.SendMessage("move");
					//Removing Mana for the move action.
					CharacterManager.aCurrentlySelectedUnit.SendMessage("RemoveMoveMana");
					
					//ActionMenuGUI.activateMvmtMenu = true;
				}
			}
			
			// de-select the tile, but only if the unit is not moving towards it
			else if (TileManager.aCurrentlySelectedTile == gameObject && !CharacterManager.aMidTurn)
			{
				if (!CharacterManager.aSingleUnitIsSelected)
				{
					tileManager.SendMessage("deselct");
				}
			}
		}
		else
		{
			if (!PlayerFunctionalityAndStats.isLegalMove)
			{
				GameObject.Find("GUI Hot Seat").SendMessage("showText", "Insufficient Mana");
				GameObject.Find("GUI Mana Points").SendMessage("ShakeText");
			}
			else
				GameObject.Find("GUI Hot Seat").SendMessage("showText", "Can't Move Again");
			
			moveIsSpent = false;
		}
	}

}
