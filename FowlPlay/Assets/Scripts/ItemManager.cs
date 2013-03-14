using UnityEngine;
using System.Collections;

public class ItemManager : MonoBehaviour {
	
	public static Hashtable tilesWithItems;
	
	public GameObject dinoCoOil, birdSeed, dinoChow;
	
	private GameObject[] prefabs;
	
	// Use this for initialization
	void Start () {
		tilesWithItems = new Hashtable();
		prefabs = new GameObject[] {dinoCoOil, birdSeed, dinoChow};
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
			
			// Set position
			Vector3 itemPosition = tile.transform.position;
			itemPosition.y = 6.0f;
			
			// Instantiate (put on map) and add to hashtable
			GameObject item = (GameObject)Instantiate(prefabs[Random.Range(0, prefabs.Length)], itemPosition, Quaternion.identity);
			tilesWithItems.Add(tile.transform.position, item);
			
			switch (item.name)
			{
				case "DinoChow(Clone)" :
					item.tag = "DinoChow";
					break;
				case "BirdSeed(Clone)" :
					item.tag = "BirdSeed";
					break;
				case "DinoCoOil(Clone)" :
					item.tag = "DinoCoOil";
					break;
			}
			
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
