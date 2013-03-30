using UnityEngine;
using System.Collections;

public class DinosaurUnitFunctionalityAndStats : MonoBehaviour {
	
	public int healthPoints = 100;
	public int maxHealthPoints = 100;
	public int defensePoints = 10;
	public int attackPoints = 15;
	//The current and max amount of how tamed this unit can be.
	public float tamePoints  = 100.0f;
	public float maxTamePoints = 100.0f;
	//Various mana costs of certain actions.
	public int attackCost = 1;
	public int moveCost = 1;
	public int moveRange = 3;
	public int attackRange = 2;
	public int abilityCost = -1;
	//How quickly this unit gets tamed
	public float tameRate = 1.0f;
	//The amount of tameness that goes away each turn.
	public int tameTickAmount = 10;
	//The distance that the Commander must be at for the tameTickAmount to go down by its actual cost
	//The higher this is the further a unit can safely be away from its commander.
	public float ObeyRange = 4.0f;
	//Amount that taking damage untames the dino.
	public float fury = 5.0f;
	//Amount that attacking untames the dino.
	public float bloodlust = 5.0f;
	public bool tamed = false;
	public string species;
	public GameObject deathParticle;
	//Active Variables
	public bool attackSpent = false;
	public bool moveSpent = false;
	//Colors
	public Color player1Color = Color.green;
	public Color player2Color = Color.blue;
	public Color enemyColor = Color.red;
	public Color selectColor = Color.yellow;
	public Color spentColor = Color.gray;
	
	public Color P1UntameColor;// = new Color(maxTamePoints/tamePoints, tamePoint/maxTamePoints, 0, 1);
	public Color P2UntameColor;// = new Color(tamePoints/maxTamePoints, 0, maxTamePoints/tamePoints, 1);
	
	public Color unitColor;
	float flashRate = 1.0f;
	
	private string miniGame0Inst = "Left Click or press the Spacebar when the sliding bar is lined up with the green block"; 
	private string miniGame1Inst = "Mash the Spacebar or Left Click!!";
	
	// SFX - Death of unit
	public AudioClip soundDeath;
	
	void Start()
	{
		UpdateColor();
	}
	/*void OnMouseEnter()
	{
		UpdateColor();
	}*/
	
	/*void Update()
	{
		float tpOverMtp = 1 - (tamePoints/maxTamePoints);
		float mtpOverTp = 1 - (maxTamePoints/tamePoints);
		
		P1UntameColor = new Color(mtpOverTp, 1.0f, 0.0f, 1.0f);
		P2UntameColor = new Color(tpOverMtp, 0.0f, 1.0f, 1.0f);
		player1Color = P1UntameColor;
		player2Color = P2UntameColor;
	}*/
	
	
	public IEnumerator AttackUnit (GameObject unit)
	{
		GameObject birdCommander;
		int bonusDamage = 0;
		
		//Checking for mana cost too!
		if (gameObject.tag == "Player1")
		{
			birdCommander = CharacterManager.bird1;
			birdCommander.SendMessage("CheckLegalMove", attackCost);
		}	
		else if (gameObject.tag == "Player2")
		{
			birdCommander = CharacterManager.bird2;
			birdCommander.SendMessage("CheckLegalMove", attackCost);
		}
		else
			PlayerFunctionalityAndStats.isLegalMove = true;
		
		if (!attackSpent && PlayerFunctionalityAndStats.isLegalMove)
		{
			//~~~~~~~MINI GAME START HERE
			//Activate mini game stuff and camera
			BackgroundGUI.inMiniGame = true;
			
			//Since this is a bird we know that tile 1 should be filled by a bird model.
			GameObject.Find("Tile2").transform.FindChild("battleBird").transform.FindChild("BoneMaster").transform.FindChild("Dummy003").transform.FindChild("dino-control").renderer.enabled = false;
			GameObject.Find("Tile1").transform.FindChild("battle" + species).transform.FindChild("body").renderer.enabled = true;
			GameObject.Find("Tile1").transform.FindChild("battleBird").transform.FindChild("BoneMaster").transform.FindChild("Dummy003").transform.FindChild("dino-control").renderer.enabled = false;
			CharacterManager.aInteractUnit.SendMessage("UpdateInteractSpecies");
			UpdateCurrentlySelectedSpecies();
			GameObject.Find("Tile2").transform.FindChild("battle" + CharacterManager.aInteractSpecies).transform.FindChild("body").renderer.enabled = true;
			if (CharacterManager.aInteractUnit == CharacterManager.bird2 || CharacterManager.aInteractUnit == CharacterManager.bird2)
				GameObject.Find("Tile2").transform.FindChild("battleBird").transform.FindChild("BoneMaster").transform.FindChild("Dummy003").transform.FindChild("dino-control").renderer.enabled = true;
			GameObject.Find("Mini Game Camera").camera.enabled = true;
			
			//Determine which minigame is going to run
			int miniGameNum = Random.Range(0, 2);
			//Bar Grow and Hit Mini Game
			if (miniGameNum == 0)
			{
				GameObject.Find("BlockManagerObj").GetComponent<BlockManager>().enabled = true;
				GameObject.Find("Meter").GetComponent<BarGrowAndHit>().enabled = true;
				GameObject.Find("MeterCube").GetComponent<MeshRenderer>().enabled = true;
				MinigameMenu.gameInstructions = miniGame0Inst;
			}
			//Button Mash Mini Game
			else if (miniGameNum == 1)
			{
				GameObject.Find("GUI Countdown").GetComponent<GUIText>().enabled = true;
				GameObject.Find("Plane").GetComponent<mattsMash>().enabled = true;
				MinigameMenu.gameInstructions = miniGame1Inst;
			}
			
			//Can turn instructions on or off, if on pause and display the menu
			if (PauseMenuGUI.instructionsOn)
				MinigameMenu.isPausedForInstructions = true;
			//else just start running the minigame
			else
				MinigameMenu.minigameIsRunning = true;
			
			//Start the mini game with 11 seconds, just in case we're keeping track of the time for now
			MinigameMenu.aSeconds = 11;
			yield return new WaitForSeconds(11);
			//minigameIsRunning = false used to be here.
			//Reseting things after mini game has been moved below so that damage text can be displayed during the mini game.
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
			unit.SendMessage("TakeAttackDamage", attackPoints + bonusDamage);
			bonusDamage = 0;
			BarGrowAndHit.counter = 0;
			//Remove tame points for attacking.
			RemoveTamePoints(bloodlust);
			//This unit has spent its attack for the turn.
			attackSpent = true;
			UpdateColor();
		
			if (gameObject.tag == "Player1")
			{
				CharacterManager.bird1.SendMessage("RemoveMana", attackCost);
			}
			else if (gameObject.tag == "Player2")
			{
				CharacterManager.bird2.SendMessage("RemoveMana", attackCost);
			}
			
			
			MinigameMenu.minigameIsRunning = false;
			
			//Reseting things back to where they were before the mini game.
			BackgroundGUI.inMiniGame = false;
			
			GameObject.Find("Tile1").transform.FindChild("battle" + species).transform.FindChild("body").renderer.enabled = false;
			GameObject.Find("Tile2").transform.FindChild("battle" + CharacterManager.aInteractSpecies).transform.FindChild("body").renderer.enabled = false;
			if (CharacterManager.aInteractUnit == CharacterManager.bird2 || CharacterManager.aInteractUnit == CharacterManager.bird2)
				GameObject.Find("Tile2").transform.FindChild("battleBird").transform.FindChild("BoneMaster").transform.FindChild("Dummy003").transform.FindChild("dino-control").renderer.enabled = false;
			
			GameObject.Find("Mini Game Camera").camera.enabled = false;
			yield return new WaitForSeconds(2);
			
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
				GameObject.Find("GUI Countdown").GetComponent<GUIText>().enabled = false;
				GameObject.Find("Plane").GetComponent<mattsMash>().enabled = false;
				mattsMash.theMashes = 0;
			}
			
			MinigameMenu.attackAnimStart = false;
			MinigameMenu.damageAnimStart = false;
			//~~~~~~~MINI GAME END HERE
			
			
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
		RemoveTamePoints(fury);
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
		if (CharacterManager.aInteractSpecies != "Bird")
			GameObject.Find("Tile2").transform.FindChild("battle" + CharacterManager.aInteractSpecies).transform.FindChild("Text").BroadcastMessage("showDamageText", "-" + actualDamageTaken.ToString());
		else
			GameObject.Find("Tile2").transform.FindChild("battle" + CharacterManager.aInteractSpecies).transform.FindChild("Damage Text").SendMessage("showDamageText", "-" + actualDamageTaken.ToString());
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
		
		iTween.ValueTo(gameObject, iTween.Hash("from", temp, "to", healthPoints, "onupdate", "UpdateGuiHealthBarDynamic"));
	}
	
	// Stores whether the unit has full HP in a temporary register in ClickAndMove & UntamedManager for the purpose of items.
	public void CheckHP()
	{
		ClickAndMove.fullHP = healthPoints == maxHealthPoints;
		UntamedManager.fullHP = healthPoints == maxHealthPoints;	
	}
	
	public float EndTurnTickUntame (Vector3 commanderPosition)
	{
		float commanderDistance, commanderRateBonus;//, xDist, zDist;
		float temp = tamePoints;
		
		if (tag == "Player1")
			commanderDistance = (float) TileManager.movementCost(gameObject, CharacterManager.bird1);
		else if (tag == "Player2")
			commanderDistance = (float) TileManager.movementCost(gameObject, CharacterManager.bird2);
		else
			commanderDistance = -1.0f;
		
		commanderRateBonus = commanderDistance / ObeyRange;
		
		RemoveTamePoints(tameTickAmount * commanderRateBonus);
		
		if (tamePoints < 1)
			tamePoints = 0.0f;
		
		iTween.ValueTo(gameObject, iTween.Hash("from", temp, "to", tamePoints, "onupdate", "UpdateGuiTameBarDynamic"));
		//CheckTamePoints();
		return tamePoints;
	}
	
	/**
	 * dis function adds the points of taming to dis unit, 
	 * ASSUMES THAT THE CURRENTLY SELECTED UNIT IS THE UNIT THAT IS TAMING THIS ONE!
	 * theParams is the amount of tame points that this dino will change by.
	*/
	public float AddTamePointsByRate (int tameAmount)// tameAmount, string teamToSwitchTo)
	{
		float tempTamePoints = tamePoints;
		float overTP;
		string teamToSwitchTo = CharacterManager.aCurrentlySelectedUnit.tag;
		float temp = tamePoints;
		//Debug.Log("Taming " + gameObject + "\n");
		//Make a better pokemonlike formular here
		tempTamePoints += tameAmount * tameRate;
		
		if (tempTamePoints > maxTamePoints)
		{
			overTP = tempTamePoints - maxTamePoints;
			AddTamePoints((tameAmount*tameRate) - overTP);
			//tempTamePoints = maxTamePoints;
		}
		else
			AddTamePoints(tameAmount*tameRate);
		
		if (tamePoints > 50 && tamed == false)
		{
			//Debug.Log("This unit has switched teams " + gameObject);
			tamed = true;
			SwitchTeams(teamToSwitchTo);
		}
		iTween.ValueTo(gameObject, iTween.Hash("from", temp, "to", tamePoints, "onupdate", "UpdateGuiTameBarDynamic"));
		return tamePoints;
	}
	
	public void SwitchTeams(string team)
	{
		//Removing units from the list of the team it used to be in.
		switch(gameObject.tag)
		{
		case "Player1":
			//CharacterManager.player1Units.Remove(CharacterManager.aInteractUnit);
			CharacterManager.player1Units.Remove(gameObject);
			//CharacterManager.aInteractUnit.GetComponent<ObjectSelection>().enabled = false;
			break;
		case "Player2":
			CharacterManager.player2Units.Remove(gameObject);
			//CharacterManager.aInteractUnit.GetComponent<ObjectSelection>().enabled = false;
			break;
		case "Enemy":
			//Debug.Log("Removing an enemy from the list!\n");
			//Debug.Log("There are " + CharacterManager.untamedUnits.Count + " untamed units\n");
			CharacterManager.untamedUnits.Remove(gameObject);
			//Debug.Log("There are " + CharacterManager.untamedUnits.Count + " untamed units\n");
			break;
		}	
		
		//Adding the unit to it's new team!
		gameObject.tag = team;
		switch(team)
		{
		case "Player1":
			//Debug.Log("Adding to player one's team.\n");
			CharacterManager.player1Units.Add(gameObject);
			UpdateColor();
			//CharacterManager.aInteractUnit.transform.FindChild("HUD Point").renderer.material.color = Color.blue;
			//CharacterManager.aInteractUnit.GetComponent<ObjectSelection>().enabled = true;
			break;
		case "Player2":
			CharacterManager.player2Units.Add(gameObject);
			UpdateColor();
			//CharacterManager.aInteractUnit.transform.FindChild("HUD Point").renderer.material.color = Color.red;
			//CharacterManager.aInteractUnit.GetComponent<ObjectSelection>().enabled = true;
			break;
		case "Enemy":
			CharacterManager.untamedUnits.Add(gameObject);
			UpdateColor();
			//CharacterManager.aInteractUnit.transform.FindChild("HUD Point").renderer.material.color = Color.yellow;
			break;
		}
	}
	
	public void CheckTamePoints()
	{
		if (tamePoints > 0 && tamePoints <=15 && tamed == true)
		{
			StartCoroutine("Flash");
		}
		else
		{
			StopCoroutine("Flash");
			StopCoroutine("Return");
		}
		if (tamePoints <= 0)
		{
			SwitchTeams("Enemy");
			//animation
			//gameObject.renderer.material.color = Color.red;
			UpdateColor();
			tamed = false;
		}
	}
	
	public void OnSelectedSetHud()
	{
		//Set the attributes of the GUI via send message and sending this units current stats and max stats.
	}
	
	public IEnumerator Die()
	{
		//removing dead unit from the team.
		switch(gameObject.tag)
		{
		case "Player1":
			CharacterManager.player1Units.Remove(gameObject);
			//CharacterManager.aInteractUnit.GetComponent<ObjectSelection>().enabled = false;
			break;
		case "Player2":
			CharacterManager.player2Units.Remove(gameObject);
			//CharacterManager.aInteractUnit.GetComponent<ObjectSelection>().enabled = false;
			break;
		case "Enemy":
			//Debug.Log("Removing an enemy from the list!\n");
			//Debug.Log("There are " + CharacterManager.untamedUnits.Count + " untamed units\n");
			CharacterManager.untamedUnits.Remove(gameObject);
			break;
		}
		CharacterManager.aUnitsAndTilesHT.Remove(gameObject);
		TileManager.occupiedTilesHT.Remove(TileManager.getTileAt(TileManager.getTileUnitIsStandingOn(gameObject)));
		TileManager.getTileAt(TileManager.getTileUnitIsStandingOn(gameObject)).tag = "Tile";
		
		audio.PlayOneShot(soundDeath);
		
		if (species.Equals("tyrannosaur"))
		{
			AnimationManager.hold = true;
			gameObject.transform.FindChild("model").animation.Play("death");
			yield return new WaitForSeconds(2.0f);
			CharacterManager.killUnit(gameObject);
			Destroy(gameObject);
		}
			
		else
		{	
			yield return new WaitForSeconds(1.0f);
			Instantiate(deathParticle, transform.position, deathParticle.transform.rotation);
			CharacterManager.killUnit(gameObject);
			Destroy(gameObject);
		}
		
		AnimationManager.hold = false;
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
	
	public void UpdateGuiMaxTame()
	{
		if (CharacterManager.aSingleUnitIsSelected && CharacterManager.aCurrentlySelectedUnit == gameObject)
		{
			ProgressBarGUI.maxTamePoints1 = (int)maxTamePoints;
			ProgressBarGUI.isBird1 = false;
		}
		else if (CharacterManager.aInteractiveUnitIsSelected && CharacterManager.aInteractUnit == gameObject)
		{
			ProgressBarGUI.maxTamePoints2 = (int)maxTamePoints;
			ProgressBarGUI.isBird2 = false;
		}
	}
	
	public void UpdateGuiTameBar()
	{
		if (CharacterManager.aSingleUnitIsSelected && CharacterManager.aCurrentlySelectedUnit == gameObject)
		{
			ProgressBarGUI.tamePoints1 = (int)tamePoints;
			ProgressBarGUI.isBird1 = false;
		}
		else if (CharacterManager.aInteractiveUnitIsSelected && CharacterManager.aInteractUnit == gameObject)
		{
			ProgressBarGUI.tamePoints2 = (int)tamePoints;
			ProgressBarGUI.isBird2 = false;
		}
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
	
	public void UpdateGuiTameBarDynamic(float newValue)
	{
		if (CharacterManager.aSingleUnitIsSelected && CharacterManager.aCurrentlySelectedUnit == gameObject)
		{
			ProgressBarGUI.tamePoints1 = (int)newValue;
			ProgressBarGUI.isBird1 = false;
		}
		else if (CharacterManager.aInteractiveUnitIsSelected && CharacterManager.aInteractUnit == gameObject)
		{
			ProgressBarGUI.tamePoints2 = (int)newValue;
			ProgressBarGUI.isBird2 = false;
		}
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
		UpdateGuiMaxTame();
	}
	
	public void UpdateGuiTameButton()
	{
		PlayMenuGUI.untamed = !tamed;
	}
	
	public void UpdateGuiCosts()
	{
		PlayMenuGUI.abilityCost = abilityCost;
		PlayMenuGUI.attackCost = attackCost;
		PlayMenuGUI.moveCost = moveCost;
	}
	
	//Changes the color of the unit according to which team it's on (or if it is spent).
	public void UpdateColor()
	{
		float tpOverMtp = 1 - (tamePoints/maxTamePoints);
		float mtpOverTp = 1 - (maxTamePoints/tamePoints);
		
		P1UntameColor = new Color(mtpOverTp, 1.0f, 0.0f, 1.0f);
		P2UntameColor = new Color(tpOverMtp, 0.0f, 1.0f, 1.0f);
		player1Color = P1UntameColor;
		player2Color = P2UntameColor;
		
		
		if (tag == "Player1")
		{
			gameObject.transform.FindChild("model").transform.FindChild("body").renderer.material.color = player1Color;
			gameObject.transform.FindChild("HUD Point").renderer.material.color = player1Color;
			unitColor = player1Color;
		}
		else if (tag == "Player2")
		{
			gameObject.transform.FindChild("model").transform.FindChild("body").renderer.material.color = player2Color;
			gameObject.transform.FindChild("HUD Point").renderer.material.color = player2Color;
			unitColor = player2Color;
		}
		else if (tag == "Enemy")
		{
			gameObject.transform.FindChild("model").transform.FindChild("body").renderer.material.color = enemyColor;
			gameObject.transform.FindChild("HUD Point").renderer.material.color = enemyColor;
			unitColor = enemyColor;
		}
		
		if (attackSpent && moveSpent)
		{
			unitColor = spentColor;
			gameObject.transform.FindChild("model").transform.FindChild("body").renderer.material.color = spentColor;
		}
		
	}
	
	//Resetting the spent values and recoloring unit.
	public void StartTurnUpdateSpent()
	{
		moveSpent = false;
		attackSpent = false;
		UpdateColor();
	}
	
	public void SpendMovement()
	{
		moveSpent = true;
		UpdateColor();
	}
	
	public void UnspendMovement()
	{
		moveSpent = false;
	}
	
	public void SendMoveSpentStatus()
	{
		TileSelection.moveIsSpent = moveSpent;
	}
	
	public void SelectedColor()
	{
		//ENTIRELY INCOMPLETE, MAYBE HAVE SELECTED UNITS LERP BETWEEEN WHITE AND UNITCOLOR... OR SOMETHING.
		gameObject.transform.FindChild("model").FindChild("body").renderer.material.color = selectColor;
	}
	
	//Remove a flat number of tame points.
	public void RemoveTamePoints(float tp)
	{
		int showTP;
		tamePoints -= tp;
		if (tamePoints < 0)
		{
			tp -= 0 - tamePoints;
			tamePoints = 0;
		}
		showTP = (int) tp;
		if (tamePoints > 0 && tamePoints <= 20 && tamed == true)
		{
			StartCoroutine("Flash");
		}
		else
		{
			StopCoroutine("Flash");
			StopCoroutine("Return");
		}
		gameObject.BroadcastMessage("showTameText", "-" + showTP.ToString());
		//GameObject.Find("Tile2").transform.FindChild("battle" + CharacterManager.aInteractSpecies).transform.FindChild("Text").BroadcastMessage("showTameText", "-" + showTP.ToString());
		UpdateColor();
	}
	
	//Adding flat amount of tame points.
	public void AddTamePoints(float tp)
	{
		int showTP;
		tamePoints += tp;
		if (tamePoints > maxTamePoints)
		{
			tp -= tamePoints - maxTamePoints;
			tamePoints = maxTamePoints;
		}
		showTP = (int) tp;
		gameObject.BroadcastMessage("showTameText", "+" + showTP.ToString());
		UpdateColor();
	}
	
	// When Dino is 
	// Update is called once per frame
	IEnumerator Flash()
	{
		float t = 0;
		while(t < flashRate)
		{
			gameObject.transform.FindChild("model").transform.FindChild("body").renderer.material.color = Color.Lerp(unitColor,Color.red,t/flashRate);
			t += Time.deltaTime;
			yield return null;
		}
		gameObject.transform.FindChild("model").transform.FindChild("body").renderer.material.color = Color.red;
		StartCoroutine("Return");
	}
	
	IEnumerator Return()
	{
		float t = 0;
		while(t < flashRate)
		{
			gameObject.transform.FindChild("model").transform.FindChild("body").renderer.material.color = Color.Lerp(Color.red,unitColor,t/flashRate);
			t += Time.deltaTime;
			yield return null;
		}
		gameObject.transform.FindChild("model").transform.FindChild("body").renderer.material.color = unitColor;
		StartCoroutine("Flash");
	}
	
	public void UpdateInteractSpecies()
	{
		CharacterManager.aInteractSpecies = species;
	}
	
	public void UpdateCurrentlySelectedSpecies()
	{
		CharacterManager.aCurrentlySelectedSpecies = species;
	}
	
}