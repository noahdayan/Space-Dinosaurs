using UnityEngine;
using System.Collections;

public class BlockManager : MonoBehaviour {
	
	//The Prefab information.
	public Transform prefab;
	public static int numOfBlocks = 11;
	public int numOfGreen = 1;
	public Color aGreen = Color.green;
	public int numOfYellow = 4;
	public Color aYellow = Color.yellow;
	public int numOfRed = 6;
	public Color aRed = Color.red;
	
	private static Transform[] objectList;
	private static Vector3 position;
	
	// Use this for initialization
	void Start () {
		Debug.Log ("The total number of blocks is: " + numOfBlocks + "\n");
		Debug.Log ("The number of colors is: " + (numOfGreen + numOfRed + numOfYellow) + "\n");
		
		objectList = new Transform[numOfBlocks];
		for(int i = 0; i < numOfBlocks; i++)
		{
			objectList[i] = ((Transform)Instantiate(prefab));
		}
		for (int i = 0; i < numOfGreen; i++)
		{
			objectList[i].renderer.material.color = aGreen;
		}
		for (int i = numOfGreen; i < numOfGreen + numOfYellow; i++)
		{
			objectList[i].renderer.material.color = aYellow;
		}
		for (int i = numOfGreen + numOfYellow; i < numOfGreen + numOfYellow + numOfRed; i++)
		{
			objectList[i].renderer.material.color = aRed;
		}
		Shuffle();
		PlaceBlocks();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	public static void Shuffle()
	{
		Transform value;
		//Random rng = new Random();  
    	int n = numOfBlocks;  
    	while (n > 1) 
		{
			n--;  
   	     	int k = (int) Random.Range(0, numOfBlocks);  
   	  	  	value = objectList[k];  
   	   	  	objectList[k] = objectList[n];  
   	     	objectList[n] = value;
		}
	}
	
	public static void PlaceBlocks()
	{
		//MAYBE MAKE IT A CHILD AND MAKE THESE THINGS RELATIVE
		Vector3 newPlacement = new Vector3(1, 0, 5);
		
		for (int i = 0; i < numOfBlocks; i++)
		{
			newPlacement.y = (float) i + 0.5f;
			objectList[i].localPosition = newPlacement;
		}
	}
	
	public static Transform CheckPosition(int scale)
	{
		Debug.Log("Color of the block at " + scale + " is: " + objectList[scale].renderer.material.color + "\n");
		return objectList[scale];
	}
}
