using UnityEngine;
using System.Collections;

public class TileSelection : MonoBehaviour {
	
	private bool aMouseHoveringOnObject = false;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetMouseButtonDown(0) && aMouseHoveringOnObject)
		{
			// select the tile
			if (TileManager.aCurrentlySelectedTile != gameObject)
			{
				if (CharacterManager.aSingleUnitIsSelected && !ClickAndMove.aIsObjectMoving)
				{
        			TileManager.selectTile(gameObject);
					ActionMenuGUI.activateMvmtMenu = true;
				}
			}
			
			// de-select the tile, but only if the unit is not moving towards it
			else if (TileManager.aCurrentlySelectedTile == gameObject)
			{
				if (!CharacterManager.aSingleUnitIsSelected)
				{
					TileManager.deselect();
				}
			}
		}
		
		/*if (!CharacterManager.aSingleUnitIsSelected)
		{
			TileManager.deselect();	
		}*/
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
