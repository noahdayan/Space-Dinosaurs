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
	private Color player1Color = new Color (0.5f, 1.0f, 0.5f, 1.0f);
	private Color player2Color = new Color (0.5f, 0.5f, 1.0f, 1.0f);
	private Color enemyColor = new Color (1.0f, 0.5f, 0.5f, 1.0f);
	private Color selectColor = new Color (0.7f, 0.7f, 0.0f, 1.0f);
	private Color spentColor = Color.gray;
		
	
	// SFX
	public AudioClip soundDeath, soundAttack, soundTurkeyTame, soundChickenTame, soundTame;
			
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
		
		//Minigame animation damage
		if (MinigameMenu.minigameIsRunning)
		{
			if (healthPoints > 0)
			{
				MinigameMenu.theDefender.transform.FindChild("model").animation.wrapMode = WrapMode.Once;
				AnimationManager.hold = true;
				MinigameMenu.theDefender.transform.FindChild("model").animation.Play("damage");
			}
			MinigameMenu.theDefender.BroadcastMessage("showDamageText", "-" + actualDamageTaken.ToString());
		}
		gameObject.BroadcastMessage("showDamageText", "-" + actualDamageTaken.ToString());
		iTween.ValueTo(gameObject, iTween.Hash("from", temp, "to", healthPoints, "onupdate", "UpdateGuiHealthBarDynamic"));
		
		if (healthPoints <= 0)
		{
			StartCoroutine("Die");
		}
		
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
		
		gameObject.BroadcastMessage("showDamageText", "+" + pAmount.ToString());
		
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
	
	public void AttackUnit (GameObject unit)
	{
		gameObject.SendMessage("CheckLegalMove", attackCost);
		UpdatedTameStatusIfCurrentlySelected();
		
		
		if (PlayerFunctionalityAndStats.isLegalMove)
		{
			UpdateCurrentlySelectedSpecies();
			unit.SendMessage("UpdateInteractSpecies");
			MinigameMenu.previousInteractUnit = unit;
			GameObject.Find("MiniGameManager").SendMessage("BeginMiniGame", attackPoints);
			// Play SFX
			audio.PlayOneShot(soundAttack); //THE SOUND ATTACK WILL COME FROM THE UNIT THAT WE INSTANTIATE IN THIS ROUTINE
			
			
			//Removing Mana
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
	
	public void UpdateInteractSpecies()
	{
		if (tag == "Player1")
			CharacterManager.aInteractSpecies = "Chicken";
		else if (tag == "Player2")
			CharacterManager.aInteractSpecies = "Turkey";
	}
	
	public void UpdateCurrentlySelectedSpecies()
	{
		if (tag == "Player1")
			CharacterManager.aCurrentlySelectedSpecies = "Chicken";
		else if (tag == "Player2")
			CharacterManager.aCurrentlySelectedSpecies = "Turkey";
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
	public IEnumerator TameUnit (GameObject unit)
	{
		
		gameObject.SendMessage("CheckLegalMove", tameCost);
		
		if (PlayerFunctionalityAndStats.isLegalMove)
		{
			if (gameObject == CharacterManager.aCurrentlySelectedUnit)
			{
				unit.SendMessage("AddTamePointsByRate", tamePower);
				
				if(gameObject == CharacterManager.bird1)
				{
					gameObject.transform.FindChild("model").animation.wrapMode = WrapMode.Once;
					AnimationManager.hold = true;
					gameObject.transform.FindChild("model").animation.Play("taming");
					audio.PlayOneShot(soundChickenTame);
				}
				
				else if(gameObject == CharacterManager.bird2)
				{
					gameObject.transform.FindChild("model").animation.wrapMode = WrapMode.Once;
					AnimationManager.hold = true;
					gameObject.transform.FindChild("model").animation.Play("attack");
					audio.PlayOneShot(soundTurkeyTame);
				}
				
				yield return new WaitForSeconds(1.5f);
				audio.PlayOneShot(soundTame);
				AnimationManager.hold = false;
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
		else
			GameObject.Find("GUI Hot Seat").SendMessage("showText", "Insufficient Mana");
	}
	
	public void OnSelectedSetHud()
	{
		//Set the attributes of the GUI via send message and sending this units current stats and max stats.
	}
	
	public IEnumerator Die()
	{
		UntamedManager.unitJustDied = true;
		if (MinigameMenu.minigameIsRunning)
		{
			MinigameMenu.theDefender.transform.FindChild("model").animation.wrapMode = WrapMode.Once;
			MinigameMenu.theDefender.transform.FindChild("model").animation.Play("death");
		}
		
		transform.FindChild("model").animation.wrapMode = WrapMode.Once;
		transform.FindChild("model").animation.Play("death");
		audio.PlayOneShot(soundDeath);
		yield return new WaitForSeconds(2.0f);
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
		yield return null;
	}
	
	public void UpdateGuiHealthBar()
	{
		if (CharacterManager.aSingleUnitIsSelected && CharacterManager.aCurrentlySelectedUnit == gameObject)
			ProgressBarGUI.healthPoints1 = healthPoints;
		else// if (CharacterManager.aInteractiveUnitIsSelected && CharacterManager.aInteractUnit == gameObject)
			ProgressBarGUI.healthPoints2 = healthPoints;
	}
	
	public void UpdateGuiMaxHealth()
	{		
		if (CharacterManager.aSingleUnitIsSelected && CharacterManager.aCurrentlySelectedUnit == gameObject)
			ProgressBarGUI.maxHealthPoints1 = maxHealthPoints;
		else// if (CharacterManager.aInteractiveUnitIsSelected && CharacterManager.aInteractUnit == gameObject)
			ProgressBarGUI.maxHealthPoints2 = maxHealthPoints;
	}	
	
	public void UpdateGuiTameBar()
	{
		if (CharacterManager.aSingleUnitIsSelected && CharacterManager.aCurrentlySelectedUnit == gameObject)
			ProgressBarGUI.isBird1 = true;
		else// if (CharacterManager.aInteractiveUnitIsSelected && CharacterManager.aInteractUnit == gameObject)
			ProgressBarGUI.isBird2 = true;
	}
	
	public void UpdateGuiAttackDamage()
	{
		if (CharacterManager.aSingleUnitIsSelected && CharacterManager.aCurrentlySelectedUnit == gameObject)
			ProgressBarGUI.attackPoints1 = attackPoints;
		else// if (CharacterManager.aInteractiveUnitIsSelected && CharacterManager.aInteractUnit == gameObject)
			ProgressBarGUI.attackPoints2 = attackPoints;

	}
	
	public void UpdateGuiDefensePoints()
	{
		if (CharacterManager.aSingleUnitIsSelected && CharacterManager.aCurrentlySelectedUnit == gameObject)
			ProgressBarGUI.defensePoints1 = defensePoints;
		else// if (CharacterManager.aInteractiveUnitIsSelected && CharacterManager.aInteractUnit == gameObject)
			ProgressBarGUI.defensePoints2 = defensePoints;
	}
	
	public void UpdateGuiAttackRange()
	{
		if (CharacterManager.aSingleUnitIsSelected && CharacterManager.aCurrentlySelectedUnit == gameObject)
			ProgressBarGUI.attackRange1 = attackRange;
		else// if (CharacterManager.aInteractiveUnitIsSelected && CharacterManager.aInteractUnit == gameObject)
			ProgressBarGUI.attackRange2 = attackRange;
	}
	
	public void UpdateGuiMoveRange()
	{
		if (CharacterManager.aSingleUnitIsSelected && CharacterManager.aCurrentlySelectedUnit == gameObject)
			ProgressBarGUI.moveRange1 = moveRange;
		else// if (CharacterManager.aInteractiveUnitIsSelected && CharacterManager.aInteractUnit == gameObject)
			ProgressBarGUI.moveRange2 = moveRange;
	}
	
	public void UpdateGuiHealthBarDynamic(int newValue)
	{
		if (CharacterManager.aSingleUnitIsSelected && CharacterManager.aCurrentlySelectedUnit == gameObject)
			ProgressBarGUI.healthPoints1 = newValue;
		else// if (CharacterManager.aInteractiveUnitIsSelected && CharacterManager.aInteractUnit == gameObject)
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
			gameObject.transform.FindChild("HUD Point").renderer.material.color = Color.green;
		}
		else if (tag == "Player2")
		{
			gameObject.transform.FindChild("model").transform.FindChild("body").renderer.material.color = player2Color;
			gameObject.transform.FindChild("HUD Point").renderer.material.color = Color.blue;
		}
		else if (tag == "Enemy")
		{
			gameObject.transform.FindChild("model").transform.FindChild("body").renderer.material.color = enemyColor;
		}
	}
	
	public void UpdatedTameStatusIfCurrentlySelected()
	{
		CharacterManager.aCurrentlySelectedIsTame = true;
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
