using UnityEngine;
using System.Collections;

public class RandomTile : MonoBehaviour {
	
	public static bool isMiniGame;
	
	// Use this for initialization
	void Start () {
		if(!isMiniGame)
			transform.Rotate(Vector3.up, Random.Range(0, 6) * 60, Space.Self);
		else
			Debug.Log("Whatup!");
		/*if(transform.Find("model"))
		{
			transform.Find("model").animation["standing"].time = Random.Range(0.0f, animation["standing"].length);
		}*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
