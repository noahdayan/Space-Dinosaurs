using UnityEngine;
using System.Collections;

public class DinosaurUnitFunctionalityAndStats : MonoBehaviour {
	
	public int healthPoints = 100;
	public int maxHealthPoints = 100;
	public int defensePoints = 10;
	public int attackPoints = 15;
	public float tamePoints  = 100.0f;
	public float maxTamePoints = 100.0f;
	public int attackCost = 1;
	public int moveCost = 1;
	public int moveRange = 3;
	public int attackRange = 2;
	public float tameRate = 1.0f;
	public int tameTickAmount = 10;
	public bool tamed = false;
	public string species;
	public GameObject deathParticle;
	//Colors
	public Color player1Color = Color.green;
	public Color player2Color = Color.blue;
	public Color enemyColor = Color.red;
	
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
		//Need to make a better formula.
		int actualDamageTaken = dmg - defensePoints;
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
		Debug.Log ("Current HP of " + gameObject + " is: " + healthPoints + "\n");
		UpdateGuiHealthBar();
		return healthPoints;
	}
	
	// Adds pAmount to this unit's HP.
	// If the recovery amount would result in having more HP than the max, just make it the max.
	public void RecoverHP(int pAmount)
	{
		healthPoints += pAmount;
		
		if (healthPoints > maxHealthPoints)
			healthPoints = maxHealthPoints;
		
		UpdateGuiHealthBar();
	}
	
	// Stores whether the unit has full HP in a temporary register in ClickAndMove & UntamedManager for the purpose of items.
	public void CheckHP()
	{
		ClickAndMove.fullHP = healthPoints == maxHealthPoints;
		UntamedManager.fullHP = healthPoints == maxHealthPoints;	
	}
	
	public float EndTurnTickUntame (int commanderDistance)
	{
		tamePoints -= (tameTickAmount * commanderDistance);
		CheckTamePoints();
		return tamePoints;
	}
	
	/**
	 * dis function adds the points of taming to dis unit, 
	 * ASSUMES THAT THE CURRENTLY SELECTED UNIT IS THE UNIT THAT IS TAMING THIS ONE!
	 * theParams is the amount of tame points that this dino will change by.
	*/
	public float AddTamePointsByRate (int tameAmount)// tameAmount, string teamToSwitchTo)
	{
		string teamToSwitchTo = CharacterManager.aCurrentlySelectedUnit.tag;
		
		Debug.Log("Taming " + gameObject + "\n");
		//Make a better pokemonlike formular here
		tamePoints += tameAmount * tameRate;
		
		if (tamePoints > 50 && tamed == false)
		{
			Debug.Log("This unit has switched teams " + gameObject);
			tamed = true;
			SwitchTeams (teamToSwitchTo);
		}
		return tamePoints;
	}
	
	public void SwitchTeams(string team)
	{
		//Removing units from the list of the team it used to be in.
		switch(CharacterManager.aInteractUnit.tag)
		{
		case "Player1":
			CharacterManager.player1Units.Remove(CharacterManager.aInteractUnit);
			//CharacterManager.aInteractUnit.GetComponent<ObjectSelection>().enabled = false;
			break;
		case "Player2":
			CharacterManager.player2Units.Remove(CharacterManager.aInteractUnit);
			//CharacterManager.aInteractUnit.GetComponent<ObjectSelection>().enabled = false;
			break;
		case "Enemy":
			Debug.Log("Removing an enemy from the list!\n");
			Debug.Log("There are " + CharacterManager.untamedUnits.Count + " untamed units\n");
			CharacterManager.untamedUnits.Remove(CharacterManager.aInteractUnit);
			Debug.Log("There are " + CharacterManager.untamedUnits.Count + " untamed units\n");
			break;
		}	
		
		//Adding the unit to it's new team!
		gameObject.tag = team;
		switch(team)
		{
		case "Player1":
			Debug.Log("Adding to player one's team.\n");
			CharacterManager.player1Units.Add(CharacterManager.aInteractUnit);
			CharacterManager.aInteractUnit.transform.FindChild("HUD Point").renderer.material.color = Color.blue;
			//CharacterManager.aInteractUnit.GetComponent<ObjectSelection>().enabled = true;
			break;
		case "Player2":
			CharacterManager.player2Units.Add(CharacterManager.aInteractUnit);
			CharacterManager.aInteractUnit.transform.FindChild("HUD Point").renderer.material.color = Color.red;
			//CharacterManager.aInteractUnit.GetComponent<ObjectSelection>().enabled = true;
			break;
		case "Enemy":
			CharacterManager.untamedUnits.Add(CharacterManager.aInteractUnit);
			CharacterManager.aInteractUnit.transform.FindChild("HUD Point").renderer.material.color = Color.yellow;
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
			gameObject.transform.FindChild("model").renderer.material.color = Color.red;
			tamed = false;
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
		CharacterManager.killUnit(gameObject);
		Destroy(gameObject);
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
	}
	
	public void UpdateGuiTameButton()
	{
		PlayMenuGUI.untamed = !tamed;
	}
	
	public void UpdateColor()
	{
		if (tag == "Player1")
		{
			gameObject.transform.FindChild("model").renderer.material.color = player1Color;
			unitColor = player1Color;
		}
		else if (tag == "Player2")
		{
			gameObject.transform.FindChild("model").renderer.material.color = player2Color;
			unitColor = player2Color;
		}
		else if (tag == "Enemy")
		{
			gameObject.transform.FindChild("model").renderer.material.color = enemyColor;
			unitColor = enemyColor;
		}
	}
	
	public void SelectedColor()
	{
		//ENTIRELY INCOMPLETE, MAYBE HAVE SELECTED UNITS LERP BETWEEEN WHITE AND UNITCOLOR... OR SOMETHING.
		unitColor = gameObject.transform.FindChild("model").renderer.material.color;
		unitColor = Color.white;
		gameObject.transform.FindChild("model").renderer.material.color = unitColor;
	}
	
}