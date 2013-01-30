using UnityEngine;
using System.Collections;

public class TileSelection : MonoBehaviour {
	
	private bool aMouseHoveringOnObject = false;
	private bool aObjectIsSelected = false;
	
	// This one will also be an array that compiles all units/characters.
	public ObjectSelection aCharacterObjectSelection;
	
	public ClickAndMove aObjectMovement;
	
	public TileManager aTileManager;
	
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
				if (aCharacterObjectSelection.isObjectSelected())
				{
        			selectObject();
				}
			}
			
			// de-select the tile, but only if the unit is not moving towards it
			else if (aObjectIsSelected)
			{
				if (!aObjectMovement.isObjectMoving())
				{
					deselectObject();
				}
			}
		}
		
		if (!aCharacterObjectSelection.isObjectSelected())
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
	
	public bool isMouseHoveringObject()
	{
		return aMouseHoveringOnObject;
	}
	
	public bool isObjectSelected()
	{
		return aObjectIsSelected;	
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
