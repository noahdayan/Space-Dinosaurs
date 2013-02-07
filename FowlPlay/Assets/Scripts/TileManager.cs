using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TileManager : MonoBehaviour {
	
	public static GameObject aCurrentlySelectedTile;
	
	public CharacterManager aCharacterManager;
	
	private static GameObject[] allTiles;
	private static Hashtable allTilesHT;
	
	public static bool aSingleTileIsSelected = false;
	
	// Use this for initialization
	void Start () {		
		allTiles = GameObject.FindGameObjectsWithTag("Tile");
		
		allTilesHT = new Hashtable();
		
		foreach (GameObject tile in allTiles)
		{
			allTilesHT.Add(tile.transform.position, tile);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void highlightRange(GameObject pUnit)
	{	
		Vector3 unitsTile = pUnit.transform.position;
		unitsTile.y = 2;
		GameObject currentTile = getTileAt(unitsTile);
	 	highlightTile(currentTile);
	}
	
	public void highlightTile(GameObject pTile)
	{
		pTile.renderer.material.color = Color.cyan;
	}
	
	public GameObject getTileAt(Vector3 pPosition)
	{
		GameObject ltile = (GameObject)allTilesHT[pPosition];
		return ltile;
	}
	
	public static void selectTile(GameObject pTile)
	{
		aCurrentlySelectedTile = pTile;
		aSingleTileIsSelected = true;
		pTile.renderer.material.color = Color.yellow;
	}
	
	public static void deselect()
	{
		aCurrentlySelectedTile.renderer.material.color = Color.gray;
		aCurrentlySelectedTile = null;
		aSingleTileIsSelected = false;
	}
	
	private bool isTileOccupied(GameObject pTile)
	{
		/**
		if ((pTile.transform.position.x == aCharacterManager.unitPosition(0).x && pTile.transform.position.z == aCharacterManager.unitPosition(0).z) || (pTile.transform.position.x == aCharacterManager.unitPosition(1).x && pTile.transform.position.z == aCharacterManager.unitPosition(1).z))
			return true;
		else
			return false;
		*/
		return false;
	}
	
	public void pickRandomTile()
	{
		GameObject randomTile;
		
		do
		{
			randomTile = allTiles[Random.Range(0, allTiles.Length - 1)];
		}
		while (isTileOccupied(randomTile));
		
		Debug.Log("Random tile picked: " + randomTile.transform.position);
		
		AutoMove.destTile = randomTile;
		selectTile(randomTile);
		
		Debug.Log("destTile is " + AutoMove.destTile);
	}
}
