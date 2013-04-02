using UnityEngine;
using System.Collections;

public class mattsMash : MonoBehaviour {
	
	
	public GameObject blockChild;
	public static int theMashes;
	private Color barColor;
	Vector3 currentScale;
	
	
	
	// Use this for initialization
	void Start () {
		theMashes = 0;
		currentScale = gameObject.transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		
		blockChild.renderer.material.color = barColor;
		
		if(Input.GetButtonDown("Fire1") && currentScale.y < 10.0f)
		{
			theMashes++;
			currentScale.y += 1.0f;
			gameObject.transform.localScale = currentScale;
		}
		
		if (currentScale.y > 0.1f)
		{
			currentScale.y -= 6.7f * Time.deltaTime;
			gameObject.transform.localScale = currentScale;
			barColor = new Color ((1.0f - (currentScale.y/10.0f)), (currentScale.y/10.0f), 0.0f, 1.0f);
		}
	}
}
