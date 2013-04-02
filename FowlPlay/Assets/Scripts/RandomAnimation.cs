using UnityEngine;
using System.Collections;

public class RandomAnimation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		animation["standing"].time = Random.Range(0.0f, animation["standing"].length);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
