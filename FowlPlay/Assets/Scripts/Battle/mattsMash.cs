using UnityEngine;
using System.Collections;

public class mattsMash : MonoBehaviour {
	
	public static int theMashes;
	
	// Use this for initialization
	void Start () {
		theMashes = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire1"))//Input.GetKeyDown(KeyCode.Z))
			theMashes++;
	}
}
