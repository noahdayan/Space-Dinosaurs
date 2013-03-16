using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioSource))]

public class ManaPointsGUI : MonoBehaviour {
	
	public static int manaPoints = 10;
	
	public AudioClip error;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		guiText.text = manaPoints.ToString();
	}
	
	void ShakeText()
	{
		audio.PlayOneShot(error);
		iTween.ShakePosition(gameObject, new Vector3(0.01f, 0.01f, 0.0f), 0.5f);
	}
}
