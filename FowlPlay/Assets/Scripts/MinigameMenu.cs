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
	
	public static GameObject theAttacker;
	public static GameObject theDefender;
	public static GameObject previousInteractUnit;
	
	public static bool attackAnimStart = false;
	public static bool damageAnimStart = false;
	
	private string miniGame0Inst = "Left Click or press the Spacebar when the sliding bar is lined up with the green block"; 
	private string miniGame1Inst = "Mash the Spacebar or Left Click!!";
	
	
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
			if (aSeconds >= 5.6f)
			{
				//loop wrap thing and make attacker and defender just stand for now.
			}
			if (aSeconds >= 5.0f && aSeconds <= 5.5f && !attackAnimStart)
			{
				//Set the animation of the attack to wrap mode once for its attack. animation.wrapMode = WrapMode.Once;
				if (CharacterManager.aCurrentlySelectedIsTame && (CharacterManager.aCurrentlySelectedSpecies != "Chicken" && CharacterManager.aCurrentlySelectedSpecies != "Turkey"))
				{
					//attack should play his gun animation.animation.Play("gun");
				}
				else
				{
					//attacker should play his attack animation because he's untamed .animation.Play("attack");
				}
				attackAnimStart = true;
			}
			if (aSeconds >= 4.9f && aSeconds < 5.1f && !damageAnimStart)
			{
				//set the defenders thing to animate only once .animation.wrapMode = WrapMode.Once;
				//Defender should play his take damage animation.animation.Play("damage");
				damageAnimStart = true;
			}
			if (aSeconds < 2.3f && aSeconds > 1.0f && !damageAnimStart && !attackAnimStart)
			{
				damageAnimStart = false;
				attackAnimStart = false;
				//Defender and attacker go back to standing loop mode.animation.wrapMode = WrapMode.Loop;
				//.animation.wrapMode = WrapMode.Loop;
				//.animation.Play("standing");
				//.animation.Play("standing");
			}
		}
	}
	
	public void BeginMiniGame(int originalDamage)
	{
		StartCoroutine("RunMiniGame", originalDamage);
	}
	
	IEnumerator RunMiniGame(int originalDamage)
	{
		string attacker = CharacterManager.aCurrentlySelectedSpecies;
		string defender = CharacterManager.aInteractSpecies;
		int bonusDamage = 0;
		
			//Activate mini game stuff and camera
			BackgroundGUI.inMiniGame = true;
			
			//Instantiate the units depending on the attacker and defender strings.
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
			
			
			// Play SFX
			//CharacterManager.aCurrentlySelectedUnit.audio.PlayOneShot(soundAttack); //THE SOUND ATTACK WILL COME FROM THE UNIT THAT WE INSTANTIATE IN THIS ROUTINE
			
			//Dealing damage to the unit that we are attacking.
			previousInteractUnit.SendMessage("TakeAttackDamage", (originalDamage + bonusDamage));
			//Also do the untame text of the battle dino here.
			bonusDamage = 0;
	
			
			MinigameMenu.minigameIsRunning = false;
			yield return new WaitForSeconds(3);
			
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
			//~~~~~~~MINI GAME END HERE
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
