using UnityEngine;
using System.Collections;

public class RandomSpawn : MonoBehaviour {
	
	public int numberOfDinos;
	
	public static Hashtable tilesWithDinos;
	
	public GameObject trexPrefab;
	public GameObject anquiloPrefab;
	public GameObject tricePrefab;
	public GameObject pteroPrefab;
	public GameObject veloPrefab;
	
	private GameObject[] prefabs;
	
	// Use this for initialization
	void Start () {
	
		tilesWithDinos = new Hashtable();
		
		prefabs = new GameObject[] {trexPrefab, anquiloPrefab, tricePrefab, pteroPrefab, veloPrefab};
		
		StartCoroutine("SpawnDinos");
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	IEnumerator SpawnDinos()
	{
		yield return new WaitForSeconds(1.0f);
		
		for (int i = 0; i < numberOfDinos; i++)
		{
			GameObject tile = TileManager.TruePickRandomTile();
			
			Vector3 dinoPosition = tile.transform.position;
			dinoPosition.y = 0.0f;
			
			Quaternion rot = Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0);
			
			GameObject dino = (GameObject)Instantiate(prefabs[Random.Range(0, prefabs.Length)], dinoPosition, rot);
			CharacterManager.aUnitsAndTilesHT.Add(dino, TileManager.getTileAt(TileManager.getTileUnitIsStandingOn(dino)));
			TileManager.occupiedTilesHT.Add(tile.transform.position, dino);
			CharacterManager.untamedUnits.Add (dino);
			tile.tag = "OccupiedTile";
			dino.tag = "Enemy";
			
		}
		
	}
}
