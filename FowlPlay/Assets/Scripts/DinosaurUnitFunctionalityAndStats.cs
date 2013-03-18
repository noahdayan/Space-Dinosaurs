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
	//Colors
	public Color player1Color = Color.green;
	public Color player2Color = Color.blue;
	public Color enemyColor = Color.red;
	public Color selectColor = Color.yellow;
	
	public Color unitColor;
	public Color currentColor;

	
	void Start()
	{
		UpdateColor();
	}
	/*void OnMouseEnter()
	{
		UpdateColor();
	}*/
	
	public void AttackUnit (GameObject unit)
	{
		unit.SendMessage("TakeAttackDamage", attackPoints);
		RemoveTamePoints(bloodlust);
		
		if (gameObject.tag == "Player1")
		{
			CharacterManager.bird1.SendMessage("RemoveMana", attackCost);
		}
		else if (gameObject.tag == "Player2")
		{
			CharacterManager.bird2.SendMessage("RemoveMana", attackCost);
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
			actualDamageTaken = 0;
		}
		
		if (healthPoints <= 0)
		{
			StartCoroutine("Die");
		}
		gameObject.BroadcastMessage("showDamageText", "-" + actualDamageTaken.ToString());
		//Debug.Log ("Current HP of " + gameObject + " is: " + healthPoints + "\n");
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
		//Calculating distance from the commander.
		//xDist = (commanderPosition.x - gameObject.transform.position.x);
		//zDist = (commanderPosition.z - gameObject.transform.position.z);
		//commanderDistance = Mathf.Sqrt((xDist * xDist) + (zDist * zDist));
		
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
		ProgressBarGUI.healthBar = (float)healthPoints / (float)maxHealthPoints;
		ProgressBarGUI.healthPoints = healthPoints;
	}
	
	public void UpdateGuiTameBar()
	{
		ProgressBarGUI.tamenessBar = tamePoints/maxTamePoints;
		ProgressBarGUI.tamePoints = (int)tamePoints;
		ProgressBarGUI.isBird = false;
	}
	
	public void UpdateGuiHealthBarDynamic(int newValue)
	{
		ProgressBarGUI.healthBar = (float)newValue / (float)maxHealthPoints;
		ProgressBarGUI.healthPoints = newValue;
	}
	
	public void UpdateGuiTameBarDynamic(float newValue)
	{
		ProgressBarGUI.tamenessBar = newValue/maxTamePoints;
		ProgressBarGUI.tamePoints = (int)newValue;
		ProgressBarGUI.isBird = false;
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
	
	public void UpdateColor()
	{
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
	}
	
	public void SelectedColor()
	{
		//ENTIRELY INCOMPLETE, MAYBE HAVE SELECTED UNITS LERP BETWEEEN WHITE AND UNITCOLOR... OR SOMETHING.
		gameObject.transform.FindChild("model").FindChild("body").renderer.material.color = selectColor;
	}
	
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
		gameObject.BroadcastMessage("showTameText", "-" + showTP.ToString());
	}
	
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
	}
	
}