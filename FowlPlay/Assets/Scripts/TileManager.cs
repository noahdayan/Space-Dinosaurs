using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class TileManager : MonoBehaviour {
	
	public static GameObject aCurrentlySelectedTile;
	
	public CharacterManager aCharacterManager;
	
	private static GameObject[] allTiles;
	public static Hashtable allTilesHT;
	private Hashtable rangeHT;
	private List<GameObject> tilesInRange;
	
	private Hashtable costs;
	
	public static bool aSingleTileIsSelected = false;
	
	public static bool tileOccupied;
	
	// Use this for initialization
	void Start () {		
		allTiles = GameObject.FindGameObjectsWithTag("Tile");
		
		Debug.Log("Position of first tile: " + allTiles[0].transform.position);
		Debug.Log("Position of second tile: " + allTiles[1].transform.position);
		
		allTilesHT = new Hashtable();
		costs = new Hashtable();
		
		foreach (GameObject tile in allTiles)
		{
			allTilesHT.Add(tile.transform.position, tile);
			costs.Add (tile.transform.position, -1);
		}
		
		rangeHT = new Hashtable();
		
		tilesInRange = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void highlightRange(GameObject pUnit)
	{	
		Vector3 unitsTile = pUnit.transform.position;
		unitsTile.y = 2;
		GameObject currentTile = getTileAt(unitsTile);
		
		surround(currentTile);
		
		foreach (GameObject x in tilesInRange)
			x.renderer.material.color = Color.red;
		
		//GameObject[] surr = getSurroundingTiles(currentTile, 4);

		//foreach (GameObject tile in surr)
		//	if (tile != null)
		//		tile.renderer.material.color = Color.green;

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
	
	public void surround(GameObject pUnit)
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
				GameObject[] neighborsOfX = getSurroundingSix(getTileAt(x));
				
				foreach (GameObject neighbor in neighborsOfX)
				{
					int newCost = (int)costs[x] + movementCost((GameObject)allTilesHT[x],neighbor);
					
					//Debug.Log("LHS = " + costs[neighbor.transform.position]);
					//Debug.Log("RHS = " + costs[neighbor.transform.position]);
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
						Debug.Log("Caught exception");
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
			}
			
			costs.Remove(x);
			costs.Add(x, -1);
		}
	}
	
	
	
	/**
	 * Calculates the total number of tiles surrounding the current tile
	 * for the given range. Does not count the current tile.
	 * */
	public int totalNumberOfSurroundingTiles(int pRange)
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
	 * Returns the six tiles that surround the chosen tile.
	 * */
	public GameObject[] getSurroundingSix (GameObject pTile)
	{
		GameObject[] lTiles;
		Vector3 position = pTile.transform.position;
		Vector3[] surroundingLayer = new Vector3[6];
		
		int size = 0;
		
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
		
		for (int i = 0; i < 6; i++)
		{
			if (getTileAt(surroundingLayer[i]) != null)
				size++;
		}
		
		lTiles  = new GameObject[size];
		
		for (int i = 0; i < size; i++)
		{	
			if (getTileAt(surroundingLayer[i]) != null)
				lTiles[i] = getTileAt(surroundingLayer[i]);
		}
		
		Debug.Log("Size of first six: " + size);
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
		//if (rangeHT.ContainsKey(pTile.transform.position) && !isTileOccupied(pTile))
		if (tilesInRange.Contains(pTile) && !isTileOccupied(pTile))
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
		Debug.Log("Checking whether a tile is occupied.");
		Vector3 correctedPosition = pTile.transform.position;
		Vector3 correctedPosition1 = pTile.transform.position;
		Debug.Log("Tile position = " + correctedPosition);
		correctedPosition.y = 7;
		correctedPosition1.y = 2.5f;
		Debug.Log("Corrected position = " + correctedPosition);
		
		if (CharacterManager.unitsHT.ContainsKey(correctedPosition) || CharacterManager.unitsHT.ContainsKey(correctedPosition1))
		{
			Debug.Log("Yep, occupied");
			return true;
		}
		
		else
		{
			Debug.Log("No, free.");
			return false;
		}
	}
	
	public void pickRandomTile()
	{
		GameObject randomTile;
		
		do
		{
			randomTile = allTiles[UnityEngine.Random.Range(0, allTiles.Length - 1)];
		}
		while (isTileOccupied(randomTile));
		
		Debug.Log("Random tile picked: " + randomTile.transform.position);
		
		AutoMove.destTile = randomTile;
		selectTile(randomTile);
		
		Debug.Log("destTile is " + AutoMove.destTile);
	}
}
