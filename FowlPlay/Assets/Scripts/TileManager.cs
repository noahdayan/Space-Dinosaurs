using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class TileManager : MonoBehaviour {
	
	public static GameObject aCurrentlySelectedTile;
	
	// Used to access methods on CharacterManager
	public CharacterManager aCharacterManager;
	
	// Lists to aggregate tiles
	public static GameObject[] allTiles;
	private static GameObject[] allNonTiles; // NonTiles are the border tiles
	
	// Tracks all tiles (Vector3 : tile position, tile)
	public static Hashtable allTilesHT;
	
	// Tracks all occupied tiles (Vector3 : tile position, unit on tile)
	public static Hashtable occupiedTilesHT;
	
	// Used for finding the range of movement
	public static Hashtable costs;
	private List<GameObject> tilesInRange;
	private List<GameObject> tilesInAttackRange;
	public static List<GameObject> tilesInMidTurnAttackRange;
	
	// The materials used for highlighting the range and the tile colors.
	public Material aTileDefault, aTileBlue, aTileRed;
	
	public static bool aSingleTileIsSelected = false;
	
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
		tilesInAttackRange = new List<GameObject>();
		tilesInMidTurnAttackRange = new List<GameObject>();
		
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
		
		int range = 3;
		int attackRange = 2;
		
		// The function runs and updates the attributes tilesInRange and tilesInAttackRange
		getTilesInRange(getTileAt(unitsTile), range, attackRange);
	}
	
	public void unhighlightRange()
	{
		if (!CharacterManager.aMidTurn)
		{
			foreach (GameObject tile in tilesInRange)
				if (tile != null)
					tile.renderer.material = aTileDefault;
				
			foreach (GameObject tile in tilesInAttackRange)
				if (tile != null)
					tile.renderer.material = aTileDefault;
		}
		
		else
		{
			foreach (GameObject tile in tilesInMidTurnAttackRange)
				if (tile != null)
					tile.renderer.material = aTileDefault;
		}
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
	
	// My implementation of Dijkstra's for finding the range of movement.
	// The only thing to be aware of is the pRunNumber parameter. For most cases you want to use 1, if you'll only be running it once.
	// If you'll be running it a second time to get rid of problematic tiles, then use 2 (after you used 1).
	// NB, it does NOT reset the costs hashtable after a pass. That is for the function that called it to do.
	// Returns all the tiles within range.
	private List<Vector3> dijkstra(GameObject pUnit, int pRange, int pAttackRange, int pRunNumber)
	{
		int range = pRange + pAttackRange;
		
		Vector3 position = pUnit.transform.position;
		position.y = 2.0f;
		
		List<Vector3> results = new List<Vector3>();
		
		if (range == 1)
		{
			foreach(GameObject x in getSurroundingSix(getTileAt(position)))
				results.Add(x.transform.position);
			
			return results;
		}
		
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
				List<GameObject> surrounding = new List<GameObject>();
				if (pRunNumber == 1)
					surrounding = getSurroundingSix(getTileAt(x));
				else
					surrounding = getSurroundingSixX(getTileAt(x));
				
				foreach (GameObject neighbor in surrounding)
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
			if ((int)costs[x] < range )
				results.Add(x);
		
		return results;
	}
	
	/**
	 * Returns all tiles that are within a specified range of the selected unit.
	 * It does not return anything. Instead, it updates the tilesInRange list.
	 * Similar to Dijkstra's.
	 * */
	public void getTilesInRange(GameObject pUnit, int pRange, int pAttackRange)
	{	
		int range = pRange + pAttackRange;
		tilesInRange.Clear();
		tilesInAttackRange.Clear();
		
		List<Vector3> firstPass = new List<Vector3>();
		
		// The first pass. We check to see what tiles are within range, without considering obstacles that would reduce range.
		List<Vector3> closed = dijkstra(pUnit,pRange,pAttackRange,1);
		foreach (Vector3 x in closed)
		{
			if ((int)costs[x] < range )
			{
				if (getTileAt(x).tag.Equals("Tile"))
					getTileAt(x).renderer.material.color = Color.green;
				
				firstPass.Add(x);
			}
			
			costs.Remove(x);
			costs.Add(x, -1);
		}
		
		// Second pass--repeat to remove the tiles that the algorithm detected the first time, but that are not actually reachable.
		List<Vector3> closedBis = dijkstra(pUnit,pRange,pAttackRange,2);
		
		foreach (Vector3 x in closedBis)
		{	
			if ((int)costs[x] < range && (!getTileAt(x).tag.Equals("NonTile")))
			{
				// If the cost of reaching the tile is less than the walking range, and the tile
				// is unoccupied, then mark it blue.
				if ((int)costs[x] < pRange && getTileAt(x).tag.Equals("Tile"))
				{
					tilesInRange.Add(getTileAt(x));
					getTileAt(x).renderer.material = aTileBlue;
				}
				
				// Get the tiles within walking range, but not at the edge of the walkable area that contain enemy units.
				else if (getTileAt(x).tag.Equals("OccupiedTile") && (int)costs[x] < (pRange-1))
				{
					GameObject occupyingUnit = (GameObject)occupiedTilesHT[x];
					if ((occupyingUnit.tag.Equals("Player1") && CharacterManager.aCurrentlySelectedUnit.tag.Equals("Player2")) || (occupyingUnit.tag.Equals("Player2") && CharacterManager.aCurrentlySelectedUnit.tag.Equals("Player1")) || occupyingUnit.tag.Equals("Enemy"))
					{
						tilesInAttackRange.Add(getTileAt(x));
						getTileAt(x).renderer.material = aTileRed;
					}
				}
				
				// Get the tiles at the edge of the walking range that are occupied. These are special cases and must be handled separately.
				else if (getTileAt(x).tag.Equals("OccupiedTile") && (int)costs[x] == (pRange-1))
				{
					GameObject occupyingUnit = (GameObject)occupiedTilesHT[x];
					if ((occupyingUnit.tag.Equals("Player1") && CharacterManager.aCurrentlySelectedUnit.tag.Equals("Player2")) || (occupyingUnit.tag.Equals("Player2") && CharacterManager.aCurrentlySelectedUnit.tag.Equals("Player1")) || occupyingUnit.tag.Equals("Enemy"))
					{
						tilesInAttackRange.Add(getTileAt(x));
						getTileAt(x).renderer.material = aTileRed;
					}
				}
				
				// Get the tiles beyond the walking range that can be attacked.
				else if (((int)costs[x] > (pRange-1)) && ((int)costs[x] < (range)))
				{
					if(getTileAt(x).tag.Equals("OccupiedTile"))
					{
						// the tile is occupied, check if it's by an enemy
						GameObject occupyingUnit = (GameObject)occupiedTilesHT[x];
						if ((occupyingUnit.tag.Equals("Player1") && CharacterManager.aCurrentlySelectedUnit.tag.Equals("Player2")) || (occupyingUnit.tag.Equals("Player2") && CharacterManager.aCurrentlySelectedUnit.tag.Equals("Player1")) || occupyingUnit.tag.Equals("Enemy"))
						{
							tilesInAttackRange.Add(getTileAt(x));
							getTileAt(x).renderer.material = aTileRed;
						}
					}
					
					// if it's not occupied, mark it.
					else if(getTileAt(x).tag.Equals("Tile"))
					{
						tilesInAttackRange.Add(getTileAt(x));
						getTileAt(x).renderer.material = aTileRed;
					}
				}
			}
			
			// Now we need to find the tiles occupied by enemies not at the fringe that are actually reachable.
			if ((int)costs[x] < (range-1) )
			{
				foreach(GameObject xx in getSurroundingSix(getTileAt(x)))
				{
					if (xx.tag.Equals("OccupiedTile"))
					{
						GameObject occupyingUnit = (GameObject)occupiedTilesHT[xx.transform.position];
						if ((occupyingUnit.tag.Equals("Player1") && CharacterManager.aCurrentlySelectedUnit.tag.Equals("Player2")) || (occupyingUnit.tag.Equals("Player2") && CharacterManager.aCurrentlySelectedUnit.tag.Equals("Player1")) || occupyingUnit.tag.Equals("Enemy"))
						{
							xx.renderer.material = aTileRed;
							tilesInAttackRange.Add(xx);
						}
					}
				}
			}
			
			// Reset the hashtable.
			costs.Remove(x);
			costs.Add(x, -1);
				
		}
		
		// Get rid of the tiles that we marked as valid in the first pass, but were discovered to be invalid in the second pass.
		foreach (Vector3 y in firstPass)
			if (!closedBis.Contains(y) && getTileAt(y).renderer.sharedMaterial != aTileRed)
				getTileAt(y).renderer.material = aTileDefault;
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
			if (x != null && !x.tag.Equals("NonTile"))
				lTiles.Add(x);
		}
		
		return lTiles;
	}
	
	// This version is for range-finding purposes.
	public List<GameObject> getSurroundingSixX (GameObject pTile)
	{
		List<GameObject> lTiles = new List<GameObject>();
		
		for (int i = 1; i < 7; i++)
		{
			GameObject x = getSingleNeighbor(pTile, i);
			if (x != null && x.renderer.material.color == Color.green && !x.tag.Equals("NonTile"))
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
			unhighlightRange();
			
			pTile.renderer.material.color = Color.yellow;
		}		
	}
	
	public void deselect()
	{
		// Cannot deselct if no tile is selected!
		if (aSingleTileIsSelected) 
		{
			// mark the old tile as unoccupied
			Vector3 tile = CharacterManager.aCurrentlySelectedUnitOriginalPosition;
			tile.y = 2.0f;
			getTileAt(tile).tag = "Tile";
			
			// and mark the new tile as occupied
			aCurrentlySelectedTile.tag = "OccupiedTile";
			
			aCurrentlySelectedTile.renderer.material = aTileDefault;
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
	
	// After the move is complete, we check to see if there are any enemies within attack range.
	// If there are, paint the attack range red and allow the player to choose to attack.
	// If there are not, end the turn (for now). TODO.
	public void paintAttackableTilesAfterMove()
	{	
		// If it's the robot, ignore all of this and end its turn.
		if (CharacterManager.aCurrentlySelectedUnit.tag.Equals("Enemy"))
			SendMessage("endTurn");
		
		else
		{
			tilesInMidTurnAttackRange.Clear();
			
			bool canAttack = false;
			
			Vector3 unitsTile = CharacterManager.aCurrentlySelectedUnit.transform.position;
			unitsTile.y = 2.0f;
			
			foreach (Vector3 x in dijkstra(CharacterManager.aCurrentlySelectedUnit, 0, 2, 1))
			{
				if (getTileAt(x).tag.Equals("OccupiedTile") && x!= unitsTile)
				{
					GameObject occupyingUnit = (GameObject)occupiedTilesHT[x];
					if ((occupyingUnit.tag.Equals("Player1") && CharacterManager.aCurrentlySelectedUnit.tag.Equals("Player2")) || (occupyingUnit.tag.Equals("Player2") && CharacterManager.aCurrentlySelectedUnit.tag.Equals("Player1")) || occupyingUnit.tag.Equals("Enemy"))	
						canAttack = true;
				}
				
				tilesInMidTurnAttackRange.Add(getTileAt(x));
				costs.Remove(x);
				costs.Add(x, -1);
			}
			
			if (canAttack)
			{
				foreach (GameObject x in tilesInMidTurnAttackRange)
				{
					if (x.Equals(getTileAt(unitsTile)))
						x.renderer.material = aTileDefault;
					else
						x.renderer.material = aTileRed;
				}
				
				tilesInMidTurnAttackRange.Clear();
				
			}
			
			else
				SendMessage("endTurn");
		}
			
	}
}
