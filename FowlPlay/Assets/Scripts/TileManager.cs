using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class TileManager : MonoBehaviour {
	
	public static GameObject aCurrentlySelectedTile;
	
	public CharacterManager aCharacterManager;
	
	private static GameObject[] allTiles;
	private static GameObject[] allNonTiles;
	public static Hashtable allTilesHT;
	private Hashtable rangeHT;
	private List<GameObject> tilesInRange;
	
	private Hashtable costs;
	
	public static bool aSingleTileIsSelected = false;
	
	public static bool tileOccupied;
	
	// Use this for initialization
	void Start () {
		
		// Aggregate tiles
		allTiles = GameObject.FindGameObjectsWithTag("Tile");
		allNonTiles = GameObject.FindGameObjectsWithTag("NonTile");
		allTilesHT = new Hashtable();
		
		// Used for calculating ranges.
		costs = new Hashtable();
		rangeHT = new Hashtable();
		tilesInRange = new List<GameObject>();
		
		foreach (GameObject tile in allTiles)
		{
			allTilesHT.Add(tile.transform.position, tile);
			costs.Add (tile.transform.position, -1);
		}
		
		foreach (GameObject tile in allNonTiles)
		{
			allTilesHT.Add(tile.transform.position, tile);
			costs.Add (tile.transform.position, -1);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void getRange(GameObject pUnit)
	{	
		Vector3 unitsTile = pUnit.transform.position;
		unitsTile.y = 2;
		
		// The function runs and updates the attribute tilesInRange
		getTilesInRange(getTileAt(unitsTile));
	}
	
	public void unhighlightRange()
	{		
		foreach (GameObject tile in tilesInRange)
			tile.renderer.material.color = Color.gray;
	}
	
	public static int movementCost(GameObject pFrom, GameObject pTo)
	{
		if (pFrom == null || pTo == null)
			return 0;
		
		Vector3 lFrom = pFrom.transform.position;
		Vector3 lTo = pTo.transform.position;

		int distance = (int)(Math.Abs((lTo.x - lFrom.x)) + Math.Abs((lTo.z - lFrom.z)));
		
		if (distance == 0)
			return 0;
		
		int cost = 1;
		
		while (distance > 11)
		{
			distance -= 11;
			cost++;
		}
		
		return cost;
	}
	
	public void getTilesInRange(GameObject pUnit)
	{		
		int range = 4;
		tilesInRange.Clear();
		
		Vector3 position = pUnit.transform.position;
		position.y = 2.0f;
		Queue<Vector3> open = new Queue<Vector3>();
		
		List<Vector3> closed = new List<Vector3>();
		
		open.Enqueue(position);
		bool empty = false;
		
		do
		{
			Vector3 x = open.Dequeue();
			closed.Add(x);
			
			if ((int)costs[x] < range)
			{
				foreach (GameObject neighbor in getSurroundingSix(getTileAt(x)))
				{
					int newCost = (int)costs[x] + movementCost((GameObject)allTilesHT[x],neighbor);
					
					try {
						int costOfNeighbor = (int)costs[neighbor.transform.position];
						if ( costOfNeighbor == -1 || newCost < costOfNeighbor )
						{
							costs.Remove(neighbor.transform.position);
							costs.Add(neighbor.transform.position, newCost);
							
							if (!open.Contains(neighbor.transform.position))
								open.Enqueue(neighbor.transform.position);
						}
					} catch (NullReferenceException e) {
						Debug.Log("Caught exception - Null tile");
					}
				}
			}
			
			try { open.Peek(); }
			catch (InvalidOperationException e)
				{ empty = true; }
		
		} while (!empty);
		
		foreach (Vector3 x in closed)
		{
			if ((int)costs[x] < range && getTileAt(x).tag.Equals("Tile"))
			{
				tilesInRange.Add(getTileAt(x));
				
				// Hilight the tile
				getTileAt(x).renderer.material.color = Color.red;
			}
			
			costs.Remove(x);
			costs.Add(x, -1);
		}
	}
	
	
	
	/**
	 * Calculates the total number of tiles reachable from the current tile
	 * for the given range. Does not count the current tile.
	 * */
	public int totalNumberOfReachableTiles(int pRange)
	{
		int total = 0;
		
		for (int i = 1; i <= pRange; i++)
			total += (6*i);
		
		return total;
	}
	
	
	
	/**
	 * Returns the neighbor of a tile at a specified direction
	 * */
	public GameObject getSingleNeighbor (GameObject pTile, int pDirection)
	{
		if (pTile == null)
			return null;
		
		Vector3 position = pTile.transform.position;
		
		switch (pDirection)
		{
			// north - 1
			case 1:
				position.z += 8;
				break;
			
			// north-east - 2
			case 2:
				position.x += 7;
				position.z += 4;
				break;
			
			// south-east - 3
			case 3:
				position.x += 7;
				position.z -= 4;
				break;
			
			// south - 4
			case 4:
				position.z -= 8;
				break;
			
			// south-west - 5
			case 5:
				position.x -= 7;
				position.z -= 4;
				break;
			
			// north-west - 6
			case 6:
				position.x -= 7;
				position.z += 4;
				break;
			
		}
	
		return getTileAt(position);
	}
	
	/**
	 * Returns the six tiles that getTilesInRange the chosen tile.
	 * */
	public List<GameObject> getSurroundingSix (GameObject pTile)
	{
		List<GameObject> lTiles = new List<GameObject>();
		
		Vector3 position = pTile.transform.position;
		
		for (int i = 1; i < 7; i++)
		{
			GameObject x = getSingleNeighbor(pTile, i);
			if (x != null)
				lTiles.Add(x);
		}
		
		return lTiles;
	}
	
	public void highlightTile(GameObject pTile)
	{
		pTile.renderer.material.color = Color.cyan;
	}
	
	/**
	 * Returns the tile at the given position.
	 * */
	public GameObject getTileAt(Vector3 pPosition)
	{
		GameObject ltile = null;
		
		if(allTilesHT.Contains(pPosition))
			ltile = (GameObject)allTilesHT[pPosition];
		return ltile;
	}
	
	public void selectTile(GameObject pTile)
	{
		//if (tilesInRange.Contains(pTile) && !isTileOccupied(pTile))
		if (pTile.renderer.material.color == Color.red && !isTileOccupied(pTile))
		{
			aCurrentlySelectedTile = pTile;
			aSingleTileIsSelected = true;
			
			foreach (GameObject tile in tilesInRange)
				if (tile != null)
					tile.renderer.material.color = Color.gray;
			
			pTile.renderer.material.color = Color.yellow;
		}		
	}
	
	public static void deselect()
	{
		if (aSingleTileIsSelected) 
		{
			aCurrentlySelectedTile.renderer.material.color = Color.gray;
			aCurrentlySelectedTile = null;
			aSingleTileIsSelected = false;
		}
	}
	
	private bool isTileOccupied(GameObject pTile)
	{
		// Note, there are 2 corrected positions, one for dinos and one for shapes.
		Vector3 correctedPosition = pTile.transform.position;
		Vector3 correctedPosition1 = pTile.transform.position;
		correctedPosition.y = 7;
		correctedPosition1.y = 2.5f;
		
		if (CharacterManager.unitsHT.ContainsKey(correctedPosition) || CharacterManager.unitsHT.ContainsKey(correctedPosition1))
			return true;
		
		else
			return false;
	}
	
	public void pickRandomTile()
	{
		GameObject randomTile;
		
		do
		{
			randomTile = allTiles[UnityEngine.Random.Range(0, allTiles.Length - 1)];
		}
		while (isTileOccupied(randomTile));
		
		AutoMove.destTile = randomTile;
		selectTile(randomTile);
	}
}
