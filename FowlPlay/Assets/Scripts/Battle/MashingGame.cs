using UnityEngine;
using System.Collections;

public class MashingGame : MonoBehaviour {
	
	// The seconds the timer will run for.
	float aSeconds = 5;
	float aSecondsOriginal;
	
	// Whether the timer is active.
	static bool aIsMiniGameInProgress = true;
	
	// The timer
	GameObject aTimer;
	
	// Count the button mashes
	public static int aMashes = 0;

	
	// Use this for initialization
	void Start () 
	{
		aTimer = GameObject.Find("GUI Countdown");
		aSecondsOriginal = aSeconds;
		aSeconds += 5; // Done so we can allow some time for the instructions to display.
	}
	
	// Update is called once per frame
	void Update () {
		
		if(aIsMiniGameInProgress)
		{
			// First we display the instructions.
			if (aSeconds > aSecondsOriginal+1)
			{
				aTimer.guiText.text = "Mash the 'A' key!";
			}
			
			// Count-down to appear at the start of each battle.
			// During the count-down, the player is to perform the mini-game.
			else if (aSeconds > aSecondsOriginal)
			{
				aTimer.guiText.text = "GO!";
			}
	
			else if(aSeconds >= 4)
			{
				aTimer.guiText.text = aSeconds.ToString("f0");
			}
			
			else if (aSeconds >= 0)
			{
				aTimer.guiText.text = aSeconds.ToString("f0");
			}
			
			else if (aSeconds < -1)
			{
				GameObject.Destroy(aTimer);
				aIsMiniGameInProgress = false;
				DealDamage();
			}
			
			else if (aSeconds < 0)
			{
				if (aMashes > 30)
					aTimer.guiText.text = "GREAT!";
				else if (aMashes > 20)
					aTimer.guiText.text = "GOOD!";
				else if (aMashes > 15)
					aTimer.guiText.text = "OK!";
				else if (aMashes < 15)
					aTimer.guiText.text = "LAME!";
			}
			
			if (aSeconds >= 0 && aSeconds <= aSecondsOriginal)
			{
				if(Input.GetKeyDown(KeyCode.A))
					aMashes++;
			}
			
			aSeconds -= Time.deltaTime;		
		}
	}
	
	void DealDamage()
	{
		// damage dealing code goes here.
		// animations for attacking/being attacked go here.
	}

}
