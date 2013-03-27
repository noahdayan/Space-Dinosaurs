using UnityEngine;
using System.Collections;

public class MinigameMenu : MonoBehaviour {

	public AudioClip click;
	public GUISkin menuSkin;
	public int guiDepth = 0;
	public Rect menuArea;
	public Rect instructionArea;
	public Rect resumeButton;
	Rect menuAreaNormalized;
	public static bool isPausedForInstructions = false;
	public static string gameInstructions;
	
	// Use this for initialization
	void Start () {
		menuAreaNormalized = new Rect(menuArea.x * Screen.width - (menuArea.width * 0.5f), menuArea.y * Screen.height - (menuArea.height * 0.8f), menuArea.width, menuArea.height);
	}
	
	// Update is called once per frame
	void Update () {
		if (isPausedForInstructions)
		{
			Time.timeScale = 0.0f;
		}
	}
	
	
	void OnGUI() 
	{
		GUI.skin = menuSkin;
		GUI.depth = guiDepth;
		if(isPausedForInstructions)
		{
		GUI.BeginGroup(menuAreaNormalized);
			GUI.Label(instructionArea, gameInstructions);
			if(GUI.Button(new Rect(resumeButton), "Start!"))
			{
				audio.PlayOneShot(click);
				Time.timeScale = 1.0f;
				isPausedForInstructions = false;
			}			
		GUI.EndGroup();
		}
	}
}
