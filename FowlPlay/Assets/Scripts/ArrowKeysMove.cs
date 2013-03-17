using UnityEngine;
using System.Collections;

public class ArrowKeysMove : MonoBehaviour {

	public float speed = 20.0f;
	public float scrollSpeed = 100.0f;
	public Vector2 xRange;
	public Vector2 yRange;
	public Vector2 zRange;
	
	int mDelta = 10; // Pixels. The width border at the edge in which the movement work	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 tempKey = Vector3.zero;
		Vector3 tempMouse = Vector3.zero;
		
		tempKey.z = Input.GetAxis("Vertical");
		tempKey.y = Input.GetAxis("Vertical");
    	tempKey.x = Input.GetAxis("Horizontal");
		tempMouse.z = Input.GetAxis("Mouse ScrollWheel");
		
		transform.Translate(tempKey * speed * Time.deltaTime, Space.Self);
		transform.Translate(tempMouse * scrollSpeed * Time.deltaTime, Space.Self);
		
		/*
		//MOUSE MOVEMENT
		//~~~HORIZONTAL~~~
		//Move Camera Right, and check if it's in range
		if ( Input.mousePosition.x >= Screen.width - mDelta && transform.position.x < xRange.x && transform.position.z < zRange.x)
	    {
        	// Move the camera
        	transform.position += (Vector3.forward + Vector3.right) * Time.deltaTime * speed;
		}
		//Move Camera Left, and check if it's in range
		if (Input.mousePosition.x <= mDelta && xRange.y < transform.position.x && zRange.y < transform.position.z)
		{
			transform.position += (Vector3.back + Vector3.left) * Time.deltaTime * speed;
		}
		//~~~VERTICAL~~~
		//Move Camera Up, and check if it's in range
		if (Input.mousePosition.y >= Screen.height - mDelta && transform.position.z < zRange.x && xRange.y < transform.position.x)
		{
			transform.position += (Vector3.forward + Vector3.left) * Time.deltaTime * speed;
		}
		//Move Camera Down, and check if it's in range
		if (Input.mousePosition.y <= mDelta && zRange.y < transform.position.z && transform.position.x < xRange.x)
		{
			transform.position += (Vector3.back + Vector3.right) * Time.deltaTime * speed;
		}	
		*/
		
		if(Input.GetKey(KeyCode.Q))
		{
			transform.RotateAround(Vector3.zero, Vector3.up, speed * Time.deltaTime);
		}
		if(Input.GetKey(KeyCode.E))
		{
			transform.RotateAround(Vector3.zero, Vector3.down, speed * Time.deltaTime);
		}
	}
}
