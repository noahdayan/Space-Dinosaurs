using UnityEngine;
using System.Collections;

public class TileSelection : MonoBehaviour {
	
	private bool aMouseHoveringOnObject = false;
	private bool aObjectIsSelected = false;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetMouseButtonDown(0) && aMouseHoveringOnObject)
		{
			// select the tile
			if (!aObjectIsSelected)
			{
				if (CharacterManager.aSingleUnitIsSelected && !ClickAndMove.aIsObjectMoving)
				{
        			TileManager.selectTile(gameObject);
					aObjectIsSelected = true;
				}
			}
			
			// de-select the tile, but only if the unit is not moving towards it
			else if (aObjectIsSelected)
			{
				if (!CharacterManager.aSingleUnitIsSelected)
				{
					TileManager.deselect();
					aObjectIsSelected = false;
				}
			}
		}
		
		if (!ObjectSelection.aObjectIsSelected)
		{
			deselectObject();	
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
		renderer.material.color = Color.gray;
		aObjectIsSelected = false;
		GameObject.Find("Character").SendMessage("deselect");

	}
	
	public void selectObject()
	{
		renderer.material.color = Color.yellow;
		aObjectIsSelected = true;
		GameObject.Find("Character").SendMessage("selectTile", gameObject);
	}
}
