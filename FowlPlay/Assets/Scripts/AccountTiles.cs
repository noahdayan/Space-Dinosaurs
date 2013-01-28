using UnityEngine;
using System.Collections;

/**
 * Script attached to the character. It will run at the start and
 * will make the character aware of all of the tiles in the map.
 * */

public class AccountTiles : MonoBehaviour {
	
	//Transform[] allChildren = GetComponentsInChildren<map>();
	
	// Use this for initialization
	void Start () {
	 
		foreach (Transform child in transform) 
		{
            Debug.Log("Currently at: " + child.name);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
