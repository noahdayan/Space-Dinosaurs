using UnityEngine;
using System.Collections;

public class BirdUnitFunctionalityAndStats : MonoBehaviour {

	public int tamePower = 50;
	public int healthPoints = 100;
	public int maxHealthPoints = 100;
	public int defensePoints = 10;
	public int attackPoints = 15;
	public int tameCost = 1;
	public int attackCost = 1;
	public int moveCost = 1;
	public int moveRange = 3;
	public int attackRange = 2;
	public GameObject deathParticle;
	//Colors
	public Color player1Color = Color.green;
	public Color player2Color = Color.blue;
	public Color enemyColor = Color.red;
	public Color selectColor = Color.yellow;
		
	public string miniGame0Inst = "Left Click or press 'Z' when the sliding bar is lined up with the green block"; 
	public string miniGame1Inst = "Mash the 'Z' button!!";
			
	void Start()
	{
		UpdateColor();
	}
	
	/*void OnMouseEnter()
	{
		StartCoroutine("Die");
	}*/
	
	/**
	 * Has this unit take damage, usually called by another unit's "AttackUnit" function
	 * 
	 * @param dmg the ammount of damage to be dealt
	 * @return returns the remaining health of this unit after the attack.
	 */
	public int TakeAttackDamage(int dmg)
	{
		int temp = healthPoints;
		//Need to make a better formula.
		int actualDamageTaken = dmg - defensePoints;
		if (dmg - defensePoints > 0)
		{
			healthPoints -= actualDamageTaken;
		}
		else
		{
			actualDamageTaken = 1;
		}
		
		if (healthPoints <= 0)
		{
			StartCoroutine("Die");
		}
		gameObject.BroadcastMessage("showDamageText", "-" + actualDamageTaken.ToString());
		Debug.Log ("Current HP of " + gameObject + " is: " + healthPoints + "\n");
		iTween.ValueTo(gameObject, iTween.Hash("from", temp, "to", healthPoints, "onupdate", "UpdateGuiHealthBarDynamic"));
		return healthPoints;
	}
	
	// Adds pAmount to this unit's HP.
	// If the recovery amount would result in having more HP than the max, just make it the max.
	public void RecoverHP(int pAmount)
	{
		int temp = healthPoints;
		healthPoints += pAmount;
		
		if (healthPoints > maxHealthPoints)
			healthPoints = maxHealthPoints;
		
		iTween.ValueTo(gameObject, iTween.Hash("from", temp, "to", healthPoints, "onupdate", "UpdateGuiManaDynamic"));
	}
	
	// Stores whether the unit has full HP in a temporary register in ClickAndMove & UntamedManager for the purpose of items.
	public void CheckHP()
	{
		ClickAndMove.fullHP = healthPoints == maxHealthPoints;
		UntamedManager.fullHP = healthPoints == maxHealthPoints;	
	}
	
	public void MovementCost (GameObject player)
	{
		player.SendMessage("RemoveAp", moveCost);
	}
	
	public IEnumerator AttackUnit (GameObject unit)
	{
		int bonusDamage = 0;
		gameObject.SendMessage("CheckLegalMove", attackCost);
		
		if (PlayerFunctionalityAndStats.isLegalMove)
		{
			//~~~~~~~MINI GAME START HERE
			//Activate mini game stuff and camera
			BackgroundGUI.inMiniGame = true;
			GameObject.Find("Mini Game Camera").camera.enabled = true;
			
			int miniGameNum = Random.Range(0, 2);
			
			
			
			if (miniGameNum == 0)
			{
				GameObject.Find("BlockManagerObj").GetComponent<BlockManager>().enabled = true;
				GameObject.Find("Meter").GetComponent<BarGrowAndHit>().enabled = true;
				GameObject.Find("MeterCube").GetComponent<MeshRenderer>().enabled = true;
				MinigameMenu.gameInstructions = miniGame0Inst;
			}
			else if (miniGameNum == 1)
			{
				GameObject.Find("GUI Countdown").GetComponent<GUIText>().enabled = true;
				GameObject.Find("Plane").GetComponent<mattsMash>().enabled = true;
				MinigameMenu.gameInstructions = miniGame1Inst;
				//GameObject.Find("Plane").GetComponent<MashingGame>().enabled = true;
			}
			
			MinigameMenu.isPausedForInstructions = true;
			
			
			//Start coroutine, that will modify the variable bonusDamage
			//busy loop that runs till the coroutine is done.
			
			float initTime = Time.time;
			
			//Let the bar game run for 7 seconds, as of now the straight up add the number of greens hit to attack damage.
			yield return new WaitForSeconds(10);
			
			float endTime = Time.time;
			
			Debug.Log("It took: " + (endTime - initTime) + " seconds \n");
			
			
			//Reseting things back to where they were before the mini game.
			BackgroundGUI.inMiniGame = false;
			GameObject.Find("Mini Game Camera").camera.enabled = false;
			if (miniGameNum == 0)
			{
				BlockManager.HideBlocks();
				GameObject.Find("MeterCube").GetComponent<MeshRenderer>().enabled = false;
				GameObject.Find("BlockManagerObj").GetComponent<BlockManager>().enabled = false;
				GameObject.Find("Meter").GetComponent<BarGrowAndHit>().enabled = false;
				bonusDamage = BarGrowAndHit.counter;
				BarGrowAndHit.counter = 0;
			}
			else if (miniGameNum == 1)
			{
				GameObject.Find("GUI Countdown").GetComponent<GUIText>().enabled = false;
				GameObject.Find("Plane").GetComponent<mattsMash>().enabled = false;
				//GameObject.Find("Plane").GetComponent<MashingGame>().enabled = false;
				bonusDamage = mattsMash.theMashes / 8;
				mattsMash.theMashes = 0;
			}
			//~~~~~~~MINI GAME END HERE
			//Make sure to add the bonus to attackPoints
			
			
			//Dealing damage to the unit that we are attacking.
			unit.SendMessage("TakeAttackDamage", attackPoints + bonusDamage);
			bonusDamage = 0;
			UpdateColor();
		
			if (gameObject.tag == "Player1")
			{
				CharacterManager.bird1.SendMessage("RemoveMana", attackCost);
			}
			else if (gameObject.tag == "Player2")
			{
				CharacterManager.bird2.SendMessage("RemoveMana", attackCost);
			}
		}
		else
		{
			if (!PlayerFunctionalityAndStats.isLegalMove)
				GameObject.Find("GUI Hot Seat").SendMessage("showText", "Insufficient Mana");
			else
				GameObject.Find("GUI Hot Seat").SendMessage("showText", "Can't Attack Again");
		}	
	}
	
	public void RemoveMoveMana()
	{
		if (gameObject.tag == "Player1")
		{
			CharacterManager.bird1.SendMessage("RemoveMana", moveCost);
		}
		else if (gameObject.tag == "Player2")
		{
			CharacterManager.bird2.SendMessage("RemoveMana", moveCost);
		}
	}
	
	//Want to make sure that this unit is the currently selected one.
	public void TameUnit (GameObject unit)
	{
		if (gameObject == CharacterManager.aCurrentlySelectedUnit)
		{
			unit.SendMessage("AddTamePointsByRate", tamePower);
			//Maybe remove AP from the player here as well based on the tame cost?
		}
		if (gameObject.tag == "Player1")
		{
			CharacterManager.bird1.SendMessage("RemoveMana", tameCost);
		}
		else if (gameObject.tag == "Player2")
		{
			CharacterManager.bird2.SendMessage("RemoveMana", tameCost);
		}
	}
	
	public void OnSelectedSetHud()
	{
		//Set the attributes of the GUI via send message and sending this units current stats and max stats.
	}
	
	public IEnumerator Die()
	{
		yield return new WaitForSeconds(1.0f);
		Instantiate(deathParticle, transform.position, deathParticle.transform.rotation);
		Destroy(gameObject);
		if(gameObject == CharacterManager.bird1)
		{
			GameObject.Find("GUI Hot Seat").SendMessage("showTextEnd", "Player 2 Wins!");
		}
		else if(gameObject == CharacterManager.bird2)
		{
			GameObject.Find("GUI Hot Seat").SendMessage("showTextEnd", "Player 1 Wins!");
		}
		//yield return new WaitForSeconds(1.0f);
		PauseMenuGUI.gameOver = true;
		Time.timeScale = 0.0f;
	}
	
	public void UpdateGuiHealthBar()
	{
		if (CharacterManager.aSingleUnitIsSelected && CharacterManager.aCurrentlySelectedUnit == gameObject)
			ProgressBarGUI.healthPoints1 = healthPoints;
		else if (CharacterManager.aInteractiveUnitIsSelected && CharacterManager.aInteractUnit == gameObject)
			ProgressBarGUI.healthPoints2 = healthPoints;
	}
	
	public void UpdateGuiMaxHealth()
	{		
		if (CharacterManager.aSingleUnitIsSelected && CharacterManager.aCurrentlySelectedUnit == gameObject)
			ProgressBarGUI.maxHealthPoints1 = maxHealthPoints;
		else if (CharacterManager.aInteractiveUnitIsSelected && CharacterManager.aInteractUnit == gameObject)
			ProgressBarGUI.maxHealthPoints2 = maxHealthPoints;
	}	
	
	public void UpdateGuiTameBar()
	{
		if (CharacterManager.aSingleUnitIsSelected && CharacterManager.aCurrentlySelectedUnit == gameObject)
			ProgressBarGUI.isBird1 = true;
		else if (CharacterManager.aInteractiveUnitIsSelected && CharacterManager.aInteractUnit == gameObject)
			ProgressBarGUI.isBird2 = true;
	}
	
	public void UpdateGuiAttackDamage()
	{
		if (CharacterManager.aSingleUnitIsSelected && CharacterManager.aCurrentlySelectedUnit == gameObject)
			ProgressBarGUI.attackPoints1 = attackPoints;
		else if (CharacterManager.aInteractiveUnitIsSelected && CharacterManager.aInteractUnit == gameObject)
			ProgressBarGUI.attackPoints2 = attackPoints;

	}
	
	public void UpdateGuiDefensePoints()
	{
		if (CharacterManager.aSingleUnitIsSelected && CharacterManager.aCurrentlySelectedUnit == gameObject)
			ProgressBarGUI.defensePoints1 = defensePoints;
		else if (CharacterManager.aInteractiveUnitIsSelected && CharacterManager.aInteractUnit == gameObject)
			ProgressBarGUI.defensePoints2 = defensePoints;
	}
	
	public void UpdateGuiAttackRange()
	{
		if (CharacterManager.aSingleUnitIsSelected && CharacterManager.aCurrentlySelectedUnit == gameObject)
			ProgressBarGUI.attackRange1 = attackRange;
		else if (CharacterManager.aInteractiveUnitIsSelected && CharacterManager.aInteractUnit == gameObject)
			ProgressBarGUI.attackRange2 = attackRange;
	}
	
	public void UpdateGuiMoveRange()
	{
		if (CharacterManager.aSingleUnitIsSelected && CharacterManager.aCurrentlySelectedUnit == gameObject)
			ProgressBarGUI.moveRange1 = moveRange;
		else if (CharacterManager.aInteractiveUnitIsSelected && CharacterManager.aInteractUnit == gameObject)
			ProgressBarGUI.moveRange2 = moveRange;
	}
	
	public void UpdateGuiHealthBarDynamic(int newValue)
	{
		if (CharacterManager.aSingleUnitIsSelected && CharacterManager.aCurrentlySelectedUnit == gameObject)
			ProgressBarGUI.healthPoints1 = newValue;
		else if (CharacterManager.aInteractiveUnitIsSelected && CharacterManager.aInteractUnit == gameObject)
			ProgressBarGUI.healthPoints2 = newValue;
	}
	
	public void UpdateGuiStats()
	{
		UpdateGuiHealthBar();
		UpdateGuiTameBar();
		UpdateGuiAttackDamage();
		UpdateGuiDefensePoints();
		UpdateGuiAttackRange();
		UpdateGuiMoveRange();
		UpdateGuiMaxHealth();
	}
	
	
	public void UpdateGuiTameButton()
	{
		PlayMenuGUI.untamed = false;
	}	
	
	public void UpdateGuiCosts()
	{
		PlayMenuGUI.tameCost = tameCost;
		PlayMenuGUI.attackCost = attackCost;
		PlayMenuGUI.moveCost = moveCost;
	}
	
	public void UpdateColor()
	{
		if (tag == "Player1")
		{
			gameObject.transform.FindChild("model").transform.FindChild("body").renderer.material.color = player1Color;
			gameObject.transform.FindChild("HUD Point").renderer.material.color = player1Color;
		}
		else if (tag == "Player2")
		{
			gameObject.transform.FindChild("model").transform.FindChild("body").renderer.material.color = player2Color;
			gameObject.transform.FindChild("HUD Point").renderer.material.color = player2Color;
		}
		else if (tag == "Enemy")
		{
			gameObject.transform.FindChild("model").transform.FindChild("body").renderer.material.color = enemyColor;
		}
	}
	
	public float EndTurnTickUntame (Vector3 commanderPosition)
	{
		return 0.0f;
	}
	
	
	//Resetting the spent values and recoloring unit.
	public void StartTurnUpdateSpent()
	{
		UpdateColor();
	}
	
	public void SpendMovement()
	{
		//moveSpent = true;
		UpdateColor();
	}
	
	public void UnspendMovement()
	{
		//moveSpent = false;
		;
	}
	
	public void SendMoveSpentStatus()
	{
		TileSelection.moveIsSpent = false;
	}
	
	public void SelectedColor()
	{
		//ENTIRELY INCOMPLETE, MAYBE HAVE SELECTED UNITS LERP BETWEEEN WHITE AND UNITCOLOR... OR SOMETHING.
		gameObject.transform.FindChild("model").FindChild("body").renderer.material.color = selectColor;
	}
	
}
