using UnityEngine;
using System.Collections;

public class DinosaurUnitFunctionalityAndStats : MonoBehaviour {
	
	public int healthPoints = 100;
	public int maxHealthPoints = 100;
	public int defensePoints = 10;
	public int attackPoints = 15;
	public float tamePoints  = 100.0f;
	public float maxTamePoints = 100.0f;
	public int moveCost = 1;
	public int moveRange = 3;
	public int attackRange = 2;
	public float tameRate = 1.0f;
	public int tameTickAmount = 10;
	public bool tamed = false;
	public string species;
	public GameObject deathParticle;
	
	
	/*void OnMouseEnter()
	{
		Die ();
	}*/
	
	/**
	 * Has this unit take damage, usually called by another unit's "AttackUnit" function
	 * 
	 * @param dmg the ammount of damage to be dealt
	 * @return returns the remaining health of this unit after the attack.
	 */
	public int TakeAttackDamage(int dmg)
	{
		if (dmg - defensePoints > 0)
		{
			healthPoints -= dmg - defensePoints;
		}
		
		if (healthPoints <= 0)
		{
			Die ();
		}
		Debug.Log ("Current HP of " + gameObject + " is: " + healthPoints + "\n");
		UpdateGuiHealthBar();
		return healthPoints;
	}
	
	public float EndTurnTickUntame (int commanderDistance)
	{
		tamePoints -= (tameTickAmount * commanderDistance);
		CheckTamePoints();
		return tamePoints;
	}
	
	public void AttackUnit (GameObject unit)
	{
		unit.SendMessage("TakeAttackDamage", attackPoints);
		unit.BroadcastMessage("showDamageText", "-" + attackPoints.ToString());
	}
	
	/**
	 * dis function adds the points of taming to dis unit, 
	 * ASSUMES THAT THE CURRENTLY SELECTED UNIT IS THE UNIT THAT IS TAMING THIS ONE!
	 * theParams is the amount of tame points that this dino will change by.
	*/
	public float AddTamePointsByRate (int tameAmount)// tameAmount, string teamToSwitchTo)
	{
		string teamToSwitchTo = CharacterManager.aCurrentlySelectedUnit.tag;
		
		//Make a better pokemonlike formular here
		tamePoints += tameAmount * tameRate;
		
		if (tamePoints > 50 && tamed == false)
		{
			SwitchTeams (teamToSwitchTo);
		}
		return tamePoints;
	}
	
	public void SwitchTeams(string team)
	{
		gameObject.tag = team;
	}
	
	public void CheckTamePoints()
	{
		if (tamePoints <= 0)
		{
			SwitchTeams("Enemy");
			//animation
			gameObject.renderer.material.color = Color.red;
			tamed = false;
		}
	}
	
	public void OnSelectedSetHud()
	{
		//Set the attributes of the GUI via send message and sending this units current stats and max stats.
	}
	
	public void Die()
	{
		GameObject instance = Instantiate(deathParticle, transform.position, deathParticle.transform.rotation) as GameObject;
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
		
	
}