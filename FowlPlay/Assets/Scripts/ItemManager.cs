using UnityEngine;
using System.Collections;

public class ItemManager : MonoBehaviour {
	
	public static Hashtable tilesWithItems;
	
	public GameObject dinoCoOil, birdSeed, dinoChow;
	
	// Use this for initialization
	void Start () {
		
		tilesWithItems = new Hashtable();
		StartCoroutine("SpawnItems");
	}
	
	IEnumerator SpawnItems()
	{
		// Necessary to wait for a second before proceeding because TileManager needs to start-up first.
		yield return new WaitForSeconds(0.5f);
		
		// Spawn some items
		for (int i = 0; i < 5; i++)
		{
			GameObject tile;
			
			do
				 tile = TileManager.TruePickRandomTile();
			while (tilesWithItems.Contains(tile.transform.position));
			
			Vector3 itemPosition = tile.transform.position;
			itemPosition.y = 6.0f;
			
			tilesWithItems.Add(tile.transform.position, dinoChow);
			Instantiate(dinoChow, itemPosition, Quaternion.identity);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
