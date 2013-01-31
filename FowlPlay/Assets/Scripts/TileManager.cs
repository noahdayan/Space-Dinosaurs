using UnityEngine;
using System.Collections.Generic;

public class TileManager : MonoBehaviour {
	
	public GameObject aCurrentlySelectedTile;
	
	public CharacterManager aCharacterManager;
	
	private GameObject[] allTiles;
	
	private bool aSingleTileIsSelected = false;
	
	// Use this for initialization
	void Start () {		
		allTiles = GameObject.FindGameObjectsWithTag("Tile");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void selectTile(GameObject pTile)
	{
		aCurrentlySelectedTile = pTile;
		aSingleTileIsSelected = true;
		Debug.Log("CurrentlySelTile set at: " + aCurrentlySelectedTile.transform.position);
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
	
	private bool isTileOccupied(GameObject pTile)
	{
		if ((pTile.transform.position.x == aCharacterManager.unitPosition(0).x && pTile.transform.position.z == aCharacterManager.unitPosition(0).z) || (pTile.transform.position.x == aCharacterManager.unitPosition(1).x && pTile.transform.position.z == aCharacterManager.unitPosition(1).z))
			return true;
		else
			return false;
	}
	
	public GameObject pickRandomTile()
	{
		GameObject randomTile;
		
		do
		{
			randomTile = allTiles[Random.Range(0, allTiles.Length - 1)];
		}
		while (isTileOccupied(randomTile));
		
		return randomTile;
	}
}
