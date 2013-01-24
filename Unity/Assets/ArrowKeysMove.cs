using UnityEngine;
using System.Collections;

public class ArrowKeysMove : MonoBehaviour {

	public float speed = 2.0f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 newPosition = transform.position;
		
		// Time.deltaTime corrects for errors with the framerate, otherwise it's a pain.
		newPosition.x += Input.GetAxis("Horizontal") * speed * Time.deltaTime;
		newPosition.z += Input.GetAxis("Vertical") * speed * Time.deltaTime;
		transform.position = newPosition;
	}
}
