using UnityEngine;
using System.Collections;

public class TileSelection : MonoBehaviour {
	
	private bool aMouseHoveringOnObject = false;
	private bool aObjectIsSelected = false;
	
	// This one will also be an array that compiles all units/characters.
	
	public TileManager aTileManager;
	
	// Use this for initialization
	void Start () {
		//aTileManager.addTile(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetMouseButtonDown(0) && aMouseHoveringOnObject)
		{
			// select the tile
			if (!aObjectIsSelected)
			{
				if (ObjectSelection.aObjectIsSelected)
				{
        			selectObject();
				}
			}
			
			// de-select the tile, but only if the unit is not moving towards it
			else if (aObjectIsSelected)
			{
				if (!ClickAndMove.aIsObjectMoving)
				{
					deselectObject();
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
		aTileManager.deselect();
	}
	
	public void selectObject()
	{
		renderer.material.color = Color.yellow;
		aObjectIsSelected = true;
		aTileManager.selectTile(gameObject);
	}
}
