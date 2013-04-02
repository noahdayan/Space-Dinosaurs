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
	
	public GameObject trexPrefab;
	public GameObject anquiloPrefab;
	public GameObject tricePrefab;
	public GameObject pteroPrefab;
	public GameObject veloPrefab;
	public GameObject chickenPrefab;
	public GameObject turkeyPrefab;
	
	public static GameObject theAttacker;
	public Vector3 attackerPlace;
	public static GameObject theDefender;
	public Vector3 defenderPlace;
	public static GameObject previousInteractUnit;
	
	public static bool attackAnimStart = false;
	public static bool damageAnimStart = false;
	
	private string miniGame0Inst = "Left Click or press the Spacebar when the sliding bar is lined up with the green block"; 
	private string miniGame1Inst = "Mash the Spacebar or Left Click!!";
	
	public AudioClip backgroundMusic;
	
	
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
	
	public void BeginMiniGame(int originalDamage)
	{
		RandomTile.isMiniGame = true;
		StartCoroutine("RunMiniGame", originalDamage);
		GameObject.Find("Main Camera").SendMessage("PauseMusic");
		audio.Play();
	}
	
	IEnumerator RunMiniGame(int originalDamage)
	{
		string attacker = CharacterManager.aCurrentlySelectedSpecies;
		string defender = CharacterManager.aInteractSpecies;
		int bonusDamage = 0;
		
		//Activate mini game stuff and camera
		BackgroundGUI.inMiniGame = true;
		DamageTextGUI.isMiniGame = true;
		TextManagerGUI.isMiniGame = true;
		PlayerFunctionalityAndStats.isMiniGame = true;
		TameTextGUI.isMiniGame = true;
			
		
			//Instantiate the units depending on the attacker and defender strings.
		//Instantiate the attacker.
		switch (CharacterManager.aCurrentlySelectedSpecies)
		{
		case "Tyrannosaur":
			theAttacker = (GameObject) Instantiate (trexPrefab , attackerPlace, GameObject.Find("tile1").transform.rotation);
			break;
		case "Anquilosaurus":
			theAttacker = (GameObject) Instantiate (anquiloPrefab , attackerPlace, GameObject.Find("tile1").transform.rotation);
			break;
		case "Pterodactyl":
			theAttacker = (GameObject) Instantiate (pteroPrefab , attackerPlace, GameObject.Find("tile1").transform.rotation);
			break;
		case "Triceratops":
			theAttacker = (GameObject) Instantiate (tricePrefab , attackerPlace, GameObject.Find("tile1").transform.rotation);
			break;
		case "Velociraptor":
			theAttacker = (GameObject) Instantiate (veloPrefab , attackerPlace, GameObject.Find("tile1").transform.rotation);
			break;
		case "Chicken":
			theAttacker =  (GameObject) Instantiate (chickenPrefab , attackerPlace, GameObject.Find("tile1").transform.rotation);
			break;
		case "Turkey":
			theAttacker = (GameObject) Instantiate (turkeyPrefab , attackerPlace, GameObject.Find("tile1").transform.rotation);
			break;	
		}
		
		//Now the defender
		switch (CharacterManager.aInteractSpecies)
		{
		case "Tyrannosaur":
			theDefender =  (GameObject) Instantiate (trexPrefab , defenderPlace, GameObject.Find("tile2").transform.rotation);
			break;
		case "Anquilosaurus":
			theDefender = (GameObject) Instantiate (anquiloPrefab , defenderPlace, GameObject.Find("tile2").transform.rotation);
			break;
		case "Pterodactyl":
			theDefender = (GameObject) Instantiate (pteroPrefab , defenderPlace, GameObject.Find("tile2").transform.rotation);
			break;
		case "Triceratops":
			theDefender = (GameObject) Instantiate (tricePrefab , defenderPlace, GameObject.Find("tile2").transform.rotation);
			break;
		case "Velociraptor":
			theDefender = (GameObject) Instantiate (veloPrefab , defenderPlace, GameObject.Find("tile2").transform.rotation);
			break;
		case "Chicken":
			theDefender = (GameObject) Instantiate (chickenPrefab , defenderPlace, GameObject.Find("tile2").transform.rotation);
			break;
		case "Turkey":
			theDefender = (GameObject) Instantiate (turkeyPrefab , defenderPlace, GameObject.Find("tile2").transform.rotation);
			break;	
		}
		
		
		
		
			//Need to somehow link the damage text to that of the interact and currently selected units.
			//Change these battle units HP to that of the unis that they are based off of.
			
			GameObject.Find("Mini Game Camera").camera.enabled = true; 
			
			//Determine which minigame is going to run
			int miniGameNum = Random.Range(0, 2);
			//Bar Grow and Hit Mini Game
			if (miniGameNum == 0)
			{
				GameObject.Find("BlockManagerObj").GetComponent<BlockManager>().enabled = true;
				GameObject.Find("Meter").GetComponent<BarGrowAndHit>().enabled = true;
				GameObject.Find("MeterCube").GetComponent<MeshRenderer>().enabled = true;
				gameInstructions = miniGame0Inst;
			}
			//Button Mash Mini Game
			else if (miniGameNum == 1)
			{
				GameObject.Find("MeterCube").GetComponent<MeshRenderer>().enabled = true;
				GameObject.Find("GUI Countdown").GetComponent<GUIText>().enabled = true;
				GameObject.Find("Meter").GetComponent<mattsMash>().enabled = true;
				gameInstructions = miniGame1Inst;
			}
			
			//Can turn instructions on or off, if on pause and display the menu
			if (PauseMenuGUI.instructionsOn)
				MinigameMenu.isPausedForInstructions = true;
			//else just start running the minigame
			else
				MinigameMenu.minigameIsRunning = true;
			
			//Start the mini game with 11 seconds, just in case we're keeping track of the time for now
			MinigameMenu.aSeconds = 11;
			//float initTime = Time.time;
			yield return new WaitForSeconds(11);
			
			//Make sure to add the bonus to attackPoints
			if (miniGameNum == 0)
			{
				bonusDamage = BarGrowAndHit.counter;
				BarGrowAndHit.counter = 0;
			}
			else if (miniGameNum == 1)
			{
				bonusDamage = mattsMash.theMashes / 8;
				mattsMash.theMashes = 0;
			}
			
			
			//Dealing damage to the unit that we are attacking.
			theAttacker.transform.FindChild("model").animation.wrapMode = WrapMode.Once;
			AnimationManager.hold = true;
			int attackType =  Random.Range(0, 2);
			if (CharacterManager.aCurrentlySelectedSpecies != "Chicken" && CharacterManager.aCurrentlySelectedSpecies != "Turkey" && attackType == 0)
			{
				theAttacker.transform.FindChild("model").animation.Play("gun");
			}
			else
			{
				theAttacker.transform.FindChild("model").animation.Play("attack");
			}
			previousInteractUnit.SendMessage("TakeAttackDamage", (originalDamage + bonusDamage));
			
			//Also do the untame text of the battle dino here.
			bonusDamage = 0;
	
			
			MinigameMenu.minigameIsRunning = false;
			yield return new WaitForSeconds(2);
			
			//Reseting things back to where they were before the mini game.
			BackgroundGUI.inMiniGame = false;

			GameObject.Find("Mini Game Camera").camera.enabled = false;
			
			if (miniGameNum == 0)
			{
				BlockManager.HideBlocks();
				GameObject.Find("MeterCube").GetComponent<MeshRenderer>().enabled = false;
				GameObject.Find("BlockManagerObj").GetComponent<BlockManager>().enabled = false;
				GameObject.Find("Meter").GetComponent<BarGrowAndHit>().enabled = false;
				BarGrowAndHit.counter = 0;
			}
			else if (miniGameNum == 1)
			{
			
				GameObject.Find("MeterCube").GetComponent<MeshRenderer>().enabled = false;
				GameObject.Find("GUI Countdown").GetComponent<GUIText>().enabled = false;
				GameObject.Find("Meter").GetComponent<mattsMash>().enabled = false;
				mattsMash.theMashes = 0;
			}
			MinigameMenu.attackAnimStart = false;
			MinigameMenu.damageAnimStart = false;
			Destroy(theAttacker);
			Destroy(theDefender);
			//~~~~~~~MINI GAME END HERE
		audio.Stop();
		GameObject.Find("Main Camera").SendMessage("PlayMusic");
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
