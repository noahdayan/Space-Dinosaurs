using UnityEngine;
using System.Collections;

public class BarGrowAndHit : MonoBehaviour {
	
	int speed = 30;
	float t = 0f;
	bool growing = true;
	bool stopped = false;
	Vector3 currentScale;
	Vector3 newScale;
	
	// Use this for initialization
	void Start () {
		//Grow();
	}
	
	
	
	
	//GREEN IS CURRENTLY between 6 and 7!
	
	// Update is called once per frame
	void Update () {
		
		currentScale = gameObject.transform.localScale;
		
		if (Input.GetButtonDown("Fire1"))// && !stopped)
		{
			stopped = true;
		}
		/*if (Input.GetButtonDown("Fire1") && stopped)
		{
			stopped = false;
		}*/
			
		if (currentScale.y < 11.0f && growing && !stopped)
		{
			newScale = gameObject.transform.localScale;
			newScale.y = Mathf.Lerp(0.1f, 11.0f, t * 1.5f);
			gameObject.transform.localScale = newScale;
			t += Time.deltaTime;
		}
		if (currentScale.y > 10.9f && currentScale.y < 11.1f && growing && !stopped)
		{
			growing = false;
			t = 0;
		}
		if (currentScale.y > 0.0f && !growing && !stopped)
		{
			newScale = gameObject.transform.localScale;
			newScale.y = Mathf.Lerp(11.0f, 0.1f, t * 1.5f);
			gameObject.transform.localScale = newScale;
			t += Time.deltaTime;
		}
		if (currentScale.y < 0.15f && currentScale.y > 0.0f && !growing && !stopped)
		{
			growing = true;
			t = 0;
		}
		
	}
}
