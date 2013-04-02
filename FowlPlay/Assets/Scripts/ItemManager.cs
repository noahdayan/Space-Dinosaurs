using UnityEngine;
using System.Collections;

public class ItemManager : MonoBehaviour {
	
	public static Hashtable tilesWithItems;
	
	// Use this for initialization
	void Start () {
		
		tilesWithItems = new Hashtable();
		
		foreach (GameObject item in GameObject.FindGameObjectsWithTag("DinoChow"))
		{
			// Get the tile the item is standing on.
			Vector3 tile = TileManager.getTileUnitIsStandingOn(item);
			tilesWithItems.Add(tile, item);
		}
		
		foreach (GameObject item in GameObject.FindGameObjectsWithTag("BirdSeed"))
		{
			// Get the tile the item is standing on.
			Vector3 tile = TileManager.getTileUnitIsStandingOn(item);
			tilesWithItems.Add(tile, item);
		}
		
		foreach (GameObject item in GameObject.FindGameObjectsWithTag("DinoCoOil"))
		{
			// Get the tile the item is standing on.
			Vector3 tile = TileManager.getTileUnitIsStandingOn(item);
			tilesWithItems.Add(tile, item);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
