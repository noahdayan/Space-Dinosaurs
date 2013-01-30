using UnityEngine;
using System.Collections;

public class TileManager : MonoBehaviour {
	
	public GameObject aCurrentlySelectedTile;
	
	private bool aSingleTileIsSelected = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void selectTile(GameObject pTile)
	{
		aCurrentlySelectedTile = pTile;
		aSingleTileIsSelected = true;
	}
	
	public void deselect()
	{
		aCurrentlySelectedTile = null;
		aSingleTileIsSelected = false;
	}
	
	public bool tileIsSelected()
	{
		return aSingleTileIsSelected;
	}
}
