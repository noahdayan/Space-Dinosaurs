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
		
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, xRange.y, xRange.x), Mathf.Clamp(transform.position.y, yRange.y, yRange.x), Mathf.Clamp(transform.position.z, zRange.y, zRange.x));
		
		//MOUSE MOVEMENT
		//~~~HORIZONTAL~~~
		//Move Camera Right, and check if it's in range
		if ( Input.mousePosition.x >= Screen.width - mDelta)
	    {
        	transform.Translate(Vector3.right * speed * Time.deltaTime, Space.Self);
		}
		//Move Camera Left, and check if it's in range
		if (Input.mousePosition.x <= mDelta)
		{
			transform.Translate(Vector3.left * speed * Time.deltaTime, Space.Self);
		}
		//~~~VERTICAL~~~
		//Move Camera Up, and check if it's in range
		if (Input.mousePosition.y >= Screen.height - mDelta)
		{
			transform.Translate((Vector3.forward + Vector3.up) * speed * Time.deltaTime, Space.Self);
		}
		//Move Camera Down, and check if it's in range
		if (Input.mousePosition.y <= mDelta)
		{
			transform.Translate((Vector3.back + Vector3.down) * speed * Time.deltaTime, Space.Self);
		}
		
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
