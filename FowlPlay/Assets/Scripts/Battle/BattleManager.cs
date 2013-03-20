using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour {
	
	// The seconds the timer will run for.
	float aSeconds = 5;
	
	// Whether the timer is active.
	bool aCountdownIsActive = true;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		// Count-down to appear at the start of each battle.
		// During the count-down, the player is to perform the mini-game.
		if (aCountdownIsActive)
		{
			if(aSeconds >= 0)
				GameObject.Find("GUI Countdown").guiText.text = aSeconds.ToString("f0");
			
			else if (aSeconds < -1)
			{
				GameObject.Destroy(GameObject.Find("GUI Countdown"));
				aCountdownIsActive = false;
			}
			
			else if (aSeconds < 0)
				GameObject.Find("GUI Countdown").guiText.text = "FIGHT!";
			
			aSeconds -= Time.deltaTime;
		}
		
	}

}
