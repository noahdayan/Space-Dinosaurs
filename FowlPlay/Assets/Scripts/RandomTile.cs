using UnityEngine;
using System.Collections;

public class RandomTile : MonoBehaviour {

	// Use this for initialization
	void Start () {
	transform.Rotate(Vector3.up, Random.Range(0, 6) * 60, Space.Self);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
