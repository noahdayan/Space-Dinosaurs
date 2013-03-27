using UnityEngine;
using System.Collections;

public class MinigameMenu : MonoBehaviour {
	
	public static float aSeconds = 11;
	
	public AudioClip click;
	public GUISkin menuSkin;
	public int guiDepth = 0;
	public Rect menuArea;
	public Rect instructionArea;
	public Rect resumeButton;
	Rect menuAreaNormalized;
	public static bool isPausedForInstructions = false;
	public static string gameInstructions;
	public static bool minigameIsRunning = false;
	
	// Use this for initialization
	void Start () {
		menuAreaNormalized = new Rect(menuArea.x * Screen.width - (menuArea.width * 0.5f), menuArea.y * Screen.height - (menuArea.height * 0.8f), menuArea.width, menuArea.height);
	}
	
	// Update is called once per frame
	void Update () {
		//Pauses the game for instructions
		if (isPausedForInstructions)
		{
			Time.timeScale = 0.0f;
		}
		//Mini game is running so take away seconds for the countdown
		else if (minigameIsRunning)
		{
			aSeconds -= Time.deltaTime;		
		}
	}
	
	
	void OnGUI() 
	{
		GUI.skin = menuSkin;
		GUI.depth = guiDepth;
		if(isPausedForInstructions && PauseMenuGUI.instructionsOn)
		{
		//Instructions and Mini game start button
		GUI.BeginGroup(menuAreaNormalized);
			GUI.Label(instructionArea, gameInstructions);
			if(GUI.Button(new Rect(resumeButton), "Start!"))
			{
				audio.PlayOneShot(click);
				Time.timeScale = 1.0f;
				minigameIsRunning = true;
				isPausedForInstructions = false;
			}
		GUI.EndGroup();
		}
		//Else if the mini game is running then do the countdown!
		else if (minigameIsRunning)
		{
			GUI.BeginGroup(menuAreaNormalized);
			if (aSeconds >= 10)
				GUI.Label(instructionArea, "3");
			else if (aSeconds >= 9)
				GUI.Label(instructionArea, "2");
			else if (aSeconds >= 8)
				GUI.Label(instructionArea, "1");
			else if (aSeconds >= 7)
			{
				GUI.Label(instructionArea, "GO!");
				BarGrowAndHit.counter = 0;
				mattsMash.theMashes = 0;
			}
			else
				GUI.Label(instructionArea, aSeconds.ToString("f0"));
			GUI.EndGroup();
		}
	}
}
