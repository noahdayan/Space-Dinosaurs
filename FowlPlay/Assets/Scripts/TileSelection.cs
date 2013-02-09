using UnityEngine;
using System.Collections;

public class TileSelection : MonoBehaviour {
	
	private GameObject tileManager;
	
	// Use this for initialization
	void Start () {
		tileManager = GameObject.Find("Character");
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void OnMouseDown()
	{
		// select the tile
		if (TileManager.aCurrentlySelectedTile != gameObject)
		{
			if (CharacterManager.aSingleUnitIsSelected && !ClickAndMove.aIsObjectMoving)
			{
				tileManager.SendMessage("selectTile", gameObject);
				//ActionMenuGUI.activateMvmtMenu = true;
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

}
