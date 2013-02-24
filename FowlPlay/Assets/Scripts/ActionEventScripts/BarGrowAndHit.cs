using UnityEngine;
using System.Collections;

public class BarGrowAndHit : MonoBehaviour {
	
	public GameObject blockChild;
	
	int speed = 30;
	float t = 0f;
	bool growing = true;
	bool stopped = false;
	Vector3 currentScale;
	Vector3 newScale;
	public float flashRate = 0.5f;
	
	private Color originalColor;
	private Color currentColor;
	
	// Use this for initialization
	void Start () {
		originalColor = Color.white;
		blockChild.renderer.material.color = originalColor;
		//Grow();
	}
	
	
/*	8 - 11 red
	6 - 8 yellow
	5 - 6 green
	3 - 5 yellow
	0 - 3 red */
	
	
	
	
	// Update is called once per frame
	void Update () {
		
		currentScale = gameObject.transform.localScale;
		
		
		//Maybe get the difference of the absolute value of the current scale and 5.5. Then divide by that.
		//If between 5 and 6, it's actually a multiplication. Other wise it's less damage!
		if (Input.GetButtonDown("Fire1") && !stopped)
		{
			//stopped = true;
			Debug.Log("The current scale is: " + currentScale.y);
			StopCoroutine("Return");
			currentColor = BlockManager.CheckPosition((int) currentScale.y).renderer.material.color;
			blockChild.renderer.material.color = currentColor;
			BlockManager.Shuffle();
			BlockManager.PlaceBlocks();
			//LERPING IS TAKING TOO LONG :(
			//blockChild.renderer.material.color = Color.magenta;
			StartCoroutine("Return");
			//Send a message with the current scale, and send a message to block manage to shuffle the blocks!
		}
		else if (Input.GetButtonDown("Fire1") && stopped)
		{
			//stopped = false;
		}
			
		if (currentScale.y < (float) BlockManager.numOfBlocks - 0.05f && growing && !stopped)
		{
			newScale = gameObject.transform.localScale;
			newScale.y = Mathf.Lerp(0.1f, 10.99f, t * 1.5f);
			gameObject.transform.localScale = newScale;
			t += Time.deltaTime;
		}
		if (currentScale.y > (float) BlockManager.numOfBlocks - 0.1f &&
			currentScale.y < (float) BlockManager.numOfBlocks + 0.1f && 
			growing && !stopped)
		{
			growing = false;
			t = 0;
		}
		if (currentScale.y > 0.0f && !growing && !stopped)
		{
			newScale = gameObject.transform.localScale;
			newScale.y = Mathf.Lerp(10.99f, 0.1f, t * 1.5f);
			gameObject.transform.localScale = newScale;
			t += Time.deltaTime;
		}
		if (currentScale.y < 0.15f && currentScale.y > 0.0f && !growing && !stopped)
		{
			growing = true;
			t = 0;
		}
		
	}
	
	
	
	IEnumerator Flash () 
	{
		float t = 0;
		while(t < flashRate)
		{
			blockChild.renderer.material.color = Color.Lerp(originalColor, Color.magenta, t/flashRate);
			t += Time.deltaTime;
			yield return null;
		}
		blockChild.renderer.material.color = Color.magenta;
		StartCoroutine("Return");
	}
	
	IEnumerator Return()
	{
		float x = 0;
		while(x < flashRate)
		{
			blockChild.renderer.material.color = Color.Lerp(currentColor, originalColor, x/flashRate);// * 0.01f);
			x += Time.deltaTime;
			yield return null;
		}
		blockChild.renderer.material.color = originalColor;
		//StartCoroutine("Flash");
	}
}
