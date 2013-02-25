using UnityEngine;
using System.Collections;

public class ManaPointsGUI : MonoBehaviour {
	
	public static int manaPoints = 10;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		guiText.text = manaPoints.ToString();
	}
}
