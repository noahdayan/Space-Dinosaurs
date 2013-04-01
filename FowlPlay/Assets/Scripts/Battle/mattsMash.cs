using UnityEngine;
using System.Collections;

public class mattsMash : MonoBehaviour {
	
	
	public GameObject blockChild;
	public static int theMashes;
	Vector3 currentScale;
	
	
	
	// Use this for initialization
	void Start () {
		theMashes = 0;
		currentScale = gameObject.transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire1"))
		{
			theMashes++;
		//	currentScale.y += 0.2f;
		//	gameObject.transform.localScale = currentScale;
		}
		
		//if (currentScale.y > 0)
		//{
		//	currentScale.y -= 0.1f;
		//	gameObject.transform.localScale -= currentScale;
		//}
	}
}
