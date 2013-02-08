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
		
		foreach (GameObject tile in getSurroundingTiles(currentTile, 1))
			tile.renderer.material.color = Color.red;
	}
	
	public GameObject[] getSurroundingTiles(GameObject pCenterTile, int pRange)
	{
		// Number of tiles within range is up to pRange * 6.
		GameObject[] lTiles = new GameObject[pRange * 6];
		
		Vector3 position = pCenterTile.transform.position;
		
		// There are up to 6 tiles surrounding any tile.
		Vector3[] surroundingLayer = new Vector3[6];
		
		// Going clockwise, starting from tile at due north of current tile.
		
		// north
		surroundingLayer[0].x = position.x;
		surroundingLayer[0].y = position.y;
		surroundingLayer[0].z = position.z + 8;
		
		// north-east
		surroundingLayer[1].x = position.x + 7;
		surroundingLayer[1].y = position.y;
		surroundingLayer[1].z = position.z + 4;

		// south-east
		surroundingLayer[2].x = position.x + 7;
		surroundingLayer[2].y = position.y;
		surroundingLayer[2].z = position.z - 4;

		// south
		surroundingLayer[3].x = position.x;
		surroundingLayer[3].y = position.y;
		surroundingLayer[3].z = position.z - 8;

		// south-west
		surroundingLayer[4].x = position.x - 7;
		surroundingLayer[4].y = position.y;
		surroundingLayer[4].z = position.z - 4;

		// north-west
		surroundingLayer[5].x = position.x - 7;
		surroundingLayer[5].y = position.y;
		surroundingLayer[5].z = position.z + 4;
		
		for (int i = 0, multiplier = 1; i < lTiles.Length; i++, multiplier++)
			lTiles[i] = getTileAt(surroundingLayer[i]);

		return lTiles;
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
		Debug.Log("Position Selected: " + pTile.transform.position);
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
