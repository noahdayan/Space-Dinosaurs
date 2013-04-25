using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioSource))]

public class MainMenuGUI : MonoBehaviour {
	
	public AudioClip click;
	public GUISkin menuSkin;
	public int guiDepth = 0;
	public Rect menuArea;
	public Rect playButton;
	public Rect networkButton;
	public Rect creditsButton;
	public Rect quitButton;
	Rect menuAreaNormalized;
	string menuPage = "main";
	public Rect credits;
	public string levelName;
	public string networkLevel;
	public static bool network = false;
	
	// Use this for initialization
	void Start () {
		menuAreaNormalized = new Rect(menuArea.x * Screen.width - (menuArea.width * 0.5f), menuArea.y * Screen.height - (menuArea.height * 0.5f), menuArea.width, menuArea.height);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		GUI.skin = menuSkin;
		GUI.depth = guiDepth;
		GUI.BeginGroup(menuAreaNormalized);
		if(menuPage == "main")
		{
			if(Application.CanStreamedLevelBeLoaded(levelName))
			{
				if(GUI.Button(new Rect(playButton), "Play HotSeat"))
				{
					StartCoroutine("ButtonAction", levelName);
				}
			}
			else
			{
				float percentLoaded = Application.GetStreamProgressForLevel(levelName) * 100;
				GUI.Box(new Rect(playButton), "Loading.. " + percentLoaded.ToString("f0") + "% Loaded");
			}
			if(Application.CanStreamedLevelBeLoaded(networkLevel))
			{
				if(GUI.Button(new Rect(networkButton), "Play Network"))
				{
					audio.PlayOneShot(click);
					menuPage = "network";
				}
			}
			else
			{
				float percentLoaded = Application.GetStreamProgressForLevel(networkLevel) * 100;
				GUI.Box(new Rect(networkButton), "Loading.. " + percentLoaded.ToString("f0") + "% Loaded");
			}
			if(GUI.Button(new Rect(creditsButton), "Credits"))
			{
				audio.PlayOneShot(click);
				menuPage = "credits";
			}
			if(Application.platform != RuntimePlatform.OSXWebPlayer && Application.platform != RuntimePlatform.WindowsWebPlayer)
			{
				if(GUI.Button(new Rect(quitButton), "Quit"))
				{
					StartCoroutine("ButtonAction", "quit");
				}
			}
		}
		else if(menuPage == "credits")
		{
			GUI.Label(new Rect(credits), "Matt Adler\nNoah Dayan\nJavier Perez\nSimon Cruz\nJean-Felix Vallee\nNicolas Chalifoux\nRichard Atlas");
			if(GUI.Button(new Rect(quitButton), "Back"))
			{
				audio.PlayOneShot(click);
				menuPage = "main";
			}
		}
		else if(menuPage == "network")
		{
			network = true;
			if(GUI.Button(new Rect(quitButton), "Back"))
			{
				audio.PlayOneShot(click);
				menuPage = "main";
				network = false;
			}
			GUI.enabled = (Network.connections.Length >= 1 && Network.isServer);
			if(GUI.Button(new Rect(playButton), "Play"))
			{
				audio.PlayOneShot(click);
				networkView.RPC("PlayNetwork", RPCMode.All, networkLevel);
			}
		}
		GUI.EndGroup();
	}
	
	IEnumerator ButtonAction(string levelName) {
		audio.PlayOneShot(click);
		yield return new WaitForSeconds(0.35f);
		if(levelName != "quit")
		{
			Application.LoadLevel(levelName);
			PauseMenuGUI.isPaused = false;
			ProgressBarGUI.show1 = false;
			ProgressBarGUI.show2 = false;
			CharacterManager.aTurn = 1;
			CharacterManager.aTurnIsCompleted = false;
			CharacterManager.aMidTurn = false;
			CharacterManager.aSingleUnitIsSelected = false;
			TileManager.aSingleTileIsSelected = false;
			CharacterManager.aCurrentlySelectedUnit = null;
			TileManager.aCurrentlySelectedTile = null;
			CharacterManager.aCurrentlySelectedUnitOriginalPosition = Vector3.zero;
			CharacterManager.aCurrentlySelectedUnitOriginalRotation = Quaternion.identity;
			CharacterManager.aInteractUnit = null;
			CharacterManager.aInteractiveUnitIsSelected = false;
			ClickAndMove.aIsObjectMoving = false;
			ClickAndMove.aIsObjectRotating = false;
			PauseMenuGUI.gameOver = false;
			network = false;
		}
		else
		{
			Application.Quit();
			Debug.Log("Quit!");
		}
	}
	
	[RPC]
	void PlayNetwork(string levelName)
	{
		Application.LoadLevel(levelName);
	}
}
