using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class TileManager : MonoBehaviour {
	
	public static GameObject aCurrentlySelectedTile;
	
	// Used to access methods on CharacterManager
	public CharacterManager aCharacterManager;
	
	// Lists to aggregate tiles
	private static GameObject[] allTiles;
	private static GameObject[] allNonTiles; // NonTiles are the border tiles
	
	// Tracks all tiles (Vector3 : tile position, tile)
	public static Hashtable allTilesHT;
	
	// Tracks all occupied tiles (Vector3 : tile position, unit on tile)
	public static Hashtable occupiedTilesHT;
	
	// Used for finding the range of movement
	private Hashtable costs;
	private List<GameObject> tilesInRange;
	
	public static bool aSingleTileIsSelected = false;
	
	// For testing purposes of old function. Could still be used.
	//public Hashtable rangeHT;
	
	// Use this for initialization
	void Start () {
		
		// Aggregate tiles
		allTiles = GameObject.FindGameObjectsWithTag("Tile");
		allNonTiles = GameObject.FindGameObjectsWithTag("NonTile");
		allTilesHT = new Hashtable();
		occupiedTilesHT = new Hashtable();
		
		// Used for calculating ranges.
		costs = new Hashtable();
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
		
		// For testing purposes of old function
		//rangeHT = new Hashtable();
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void getRange(GameObject pUnit)
	{	
		Vector3 unitsTile = pUnit.transform.position;
		unitsTile.y = 2;
		
		int range = 3;
		
		// The function runs and updates the attribute tilesInRange
		getTilesInRange(getTileAt(unitsTile), range);
		
		// Same thing, but using the old algorithm.
		//getSurroundingTiles(getTileAt(unitsTile), range);
	}
	
	public void unhighlightRange()
	{		
		foreach (GameObject tile in tilesInRange)
			tile.renderer.material.color = Color.gray;
	}
	
	/**
	 * Returns how many tiles you have to cross to reach the destination tile, including the destination tile.
	 * */
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
		
		// 11 because that's the max distance within one layer of hexagons.
		while (distance > 11)
		{
			distance -= 11;
			cost++;
		}
		
		return cost;
	}
	
	/**
	 * Returns all tiles that are within a specified range of the selected unit.
	 * It does not return anything. Instead, it updates the tilesInRange list.
	 * Similar to Dijkstra's.
	 * */
	public void getTilesInRange(GameObject pUnit, int pRange)
	{	
		int range = pRange;
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
			
			// Try-catch to safely detect whether the queue is empty.
			try { open.Peek(); }
			catch (InvalidOperationException e)
				{ empty = true; }
		
		} while (!empty);
		
		foreach (Vector3 x in closed)
		{
			if ((int)costs[x] < range && getTileAt(x).tag.Equals("Tile"))
			{
				tilesInRange.Add(getTileAt(x));
				
				/**
				 * TODO - still unsure of whether to do the highlighting here or in its own function.
				 * Doing it here is more efficient--you color as you find, rather than going through the
				 * entire list all over again.
				 * */
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
	public static GameObject getTileAt(Vector3 pPosition)
	{
		GameObject ltile = null;
		
		if(allTilesHT.Contains(pPosition))
			ltile = (GameObject)allTilesHT[pPosition];
		return ltile;
	}
	
	public void selectTile(GameObject pTile)
	{
		// If the tile is not occupied and is within range
		if (pTile.tag.Equals("Tile") && tilesInRange.Contains(pTile))
		{
			aCurrentlySelectedTile = pTile;
			aSingleTileIsSelected = true;
			
			// un-paint the range
			foreach (GameObject tile in tilesInRange)
				if (tile != null)
					tile.renderer.material.color = Color.gray;
			
			pTile.renderer.material.color = Color.yellow;
		}		
	}
	
	public static void deselect()
	{
		// Cannot deselct if no tile is selected!
		if (aSingleTileIsSelected) 
		{
			// mark the old tile as unoccupied
			Vector3 tile = CharacterManager.aCurrentlySelectedUnitOriginalPosition;
			tile.y = 2.0f;
			getTileAt(tile).tag = "Tile";
			occupiedTilesHT.Remove(getTileAt(tile));
			
			// and mark the new tile as occupied
			aCurrentlySelectedTile.tag = "OccupiedTile";
			Vector3 tilen = CharacterManager.aCurrentlySelectedUnit.transform.position;
			tilen = CharacterManager.aCurrentlySelectedUnit.transform.position;
			tilen.y = 2.0f;
			occupiedTilesHT.Add(tilen, CharacterManager.aCurrentlySelectedUnit);
			
			aCurrentlySelectedTile.renderer.material.color = Color.gray;
			aCurrentlySelectedTile = null;
			aSingleTileIsSelected = false;
		}
	}
	
	private static bool isTileOccupied(GameObject pTile)
	{
		if (pTile.tag.Equals("OccupiedTile"))
		{
			Debug.Log("Occupied");
			return true;
		}
		else
			return false;
	}
	
	public static GameObject pickRandomTile()
	{
		GameObject randomTile;
		
		do
		{
			randomTile = allTiles[UnityEngine.Random.Range(0, allTiles.Length - 1)];
		}
		while (isTileOccupied(randomTile));
		
		return randomTile;
	}
	

	/**
	 * The old algorithm for finding the tiles in range.
	 * Still unsure of which one is more efficient, I'll leave this one here
	 * just in case.
	 * */
	public void getSurroundingTiles(GameObject pCenterTile, int pRange)
	{
		tilesInRange.Clear();
		// rangeHT.Clear();
		
		// Keeps track of the layers we still have to account for.
		int layersToGo = pRange - 1;
		
		// Get the first layer (the surrounding six of the center tile).
		foreach (GameObject tile in getSurroundingSix(pCenterTile))
		{
			if(tile != null)
			{
				tilesInRange.Add(tile);
				//rangeHT.Add(tile.transform.position, tile);
			}
		}
			
		if (pRange >= 2) 
		{
			GameObject[] nextLayer = getNextLayer(getSurroundingSixA(pCenterTile));
			while (layersToGo > 0)
			{
				foreach (GameObject tile in nextLayer)
				{
					if (tile != null)
					{
						tilesInRange.Add(tile);
						tile.renderer.material.color = Color.red;
						//if (!rangeHT.ContainsKey(tile.transform.position))
						//	rangeHT.Add(tile.transform.position, tile);
					}
				}
				
				layersToGo -= 1;
				nextLayer = getNextLayer(nextLayer);
			}
		}
	}
	
	/**
	 * Given a layer of hexagon tiles, it will return the next layer.
	 * For use with the old range finding algorithm.
	 * */
	public GameObject[] getNextLayer (GameObject[] currentLayer)
	{
		GameObject[] nextLayer = new GameObject[currentLayer.Length + 6];
		int nextLayerLevel = (currentLayer.Length / 6) + 1;
		
		int counter = 0;
		int innerCounter = 0;
		
		if (getSingleNeighbor(currentLayer[innerCounter], 1) != null)
		{
			nextLayer[counter] = getSingleNeighbor(currentLayer[innerCounter], 1);
			counter++;
		}
		
		if (getSingleNeighbor(currentLayer[innerCounter], 2) != null)
		{
			nextLayer[counter] = getSingleNeighbor(currentLayer[innerCounter], 2);
			counter++;
		}
		
		innerCounter++;
		
		if (nextLayerLevel >= 3)
		{
			for (int i = 0; i < nextLayerLevel - 2; i++)
			{
				if (getSingleNeighbor(currentLayer[innerCounter], 2) != null)
				{
					nextLayer[counter] = getSingleNeighbor(currentLayer[innerCounter], 2);
					counter++;
				}
				
				innerCounter++;
			}
		}
		
		if (getSingleNeighbor(currentLayer[innerCounter], 2) != null)
		{
			nextLayer[counter] = getSingleNeighbor(currentLayer[innerCounter], 2);
			counter++;
		}
		
		if (getSingleNeighbor(currentLayer[innerCounter], 3) != null)
		{
			nextLayer[counter] = getSingleNeighbor(currentLayer[innerCounter], 3);
			counter++;
		}
		
		innerCounter++;
		

		if (nextLayerLevel >= 3)
		{
			for (int i = 0; i < nextLayerLevel - 2; i++)
			{
				if (getSingleNeighbor(currentLayer[innerCounter], 3) != null)
				{
					nextLayer[counter] = getSingleNeighbor(currentLayer[innerCounter], 3);
					counter++;
				}
				
				innerCounter++;
			}
		}

				
		if (getSingleNeighbor(currentLayer[innerCounter], 3) != null)
		{
			nextLayer[counter] = getSingleNeighbor(currentLayer[innerCounter], 3);
			counter++;
		}
		
		if (getSingleNeighbor(currentLayer[innerCounter], 4) != null)
		{
			nextLayer[counter] = getSingleNeighbor(currentLayer[innerCounter], 4);
			counter++;
		}
		
		innerCounter++;
		

		if (nextLayerLevel >= 3)
		{
			for (int i = 0; i < nextLayerLevel - 2; i++)
			{
				if (getSingleNeighbor(currentLayer[innerCounter], 4) != null)
				{
					nextLayer[counter] = getSingleNeighbor(currentLayer[innerCounter], 4);
					counter++;

				}
				
				innerCounter++;
			}
		}
		
		if (getSingleNeighbor(currentLayer[innerCounter], 4) != null)
		{
			nextLayer[counter] = getSingleNeighbor(currentLayer[innerCounter], 4);
			counter++;
		}
		
		if (getSingleNeighbor(currentLayer[innerCounter], 5) != null)
		{
			nextLayer[counter] = getSingleNeighbor(currentLayer[innerCounter], 5);
			counter++;
		}
		
		innerCounter++;

		
		if (nextLayerLevel >= 3)
		{
			for (int i = 0; i < nextLayerLevel - 2; i++)
			{
				if (getSingleNeighbor(currentLayer[innerCounter], 5) != null)
				{
					nextLayer[counter] = getSingleNeighbor(currentLayer[innerCounter], 5);
					counter++;
				}
				
				innerCounter++;
			}
		}

		if (getSingleNeighbor(currentLayer[innerCounter], 5) != null)
		{
			nextLayer[counter] = getSingleNeighbor(currentLayer[innerCounter], 5);
			counter++;
		}
		
		if (getSingleNeighbor(currentLayer[innerCounter], 6) != null)
		{
			nextLayer[counter] = getSingleNeighbor(currentLayer[innerCounter], 6);
			counter++;
		}
		
		innerCounter++;
		
		if (nextLayerLevel >= 3)
		{
			for (int i = 0; i < nextLayerLevel - 2; i++)
			{
				if (getSingleNeighbor(currentLayer[innerCounter], 6) != null)
				{
					nextLayer[counter] = getSingleNeighbor(currentLayer[innerCounter], 6);
					counter++;
				}
				
				innerCounter++;
			}
		}
		
		if (getSingleNeighbor(currentLayer[innerCounter], 6) != null)
		{
			nextLayer[counter] = getSingleNeighbor(currentLayer[innerCounter], 6);
			counter++;
		}
		
		if (getSingleNeighbor(currentLayer[innerCounter], 1) != null)
		{
			nextLayer[counter] = getSingleNeighbor(currentLayer[innerCounter], 1);
			counter++;
		}
		
		innerCounter++;
		
		
		if (nextLayerLevel >= 3)
		{
			for (int i = 0; i < nextLayerLevel - 2; i++)
			{
				if (getSingleNeighbor(currentLayer[innerCounter], 1) != null)
				{
					nextLayer[counter] = getSingleNeighbor(currentLayer[innerCounter], 1);
					counter++;
				}
				innerCounter++;
			}
		}	
		
		return nextLayer;
	}
	
	// Same as other getSurroundingSix, but this one returns an array instead.
	// For use with the old range-finding algorithm.
	public GameObject[] getSurroundingSixA (GameObject pTile)
	{
		GameObject[] lTiles = new GameObject[6];
		
		for (int i = 1; i < 7; i++)
		{
			GameObject x = getSingleNeighbor(pTile, i);
			if (x != null)
				lTiles[i - 1] = x;
		}
		
		return lTiles;
	}
	
}
