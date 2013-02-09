using UnityEngine;
using System.Collections;

public class DinosaurUnitFunctionalityAndStats : MonoBehaviour {
	
	public int healthPoints, defensePoints, attackPoints;
	public float tamePoints;
	public int moveCost;
	public float tameRate;
	public int tameTickAmount;
	public bool tamed = false;
	
	
	/**
	 * Has this unit take damage, usually called by another unit's "AttackUnit" function
	 * 
	 * @param dmg the ammount of damage to be dealt
	 * @return returns the remaining health of this unit after the attack.
	 */
	public int TakeDamage(int dmg)
	{
		if (dmg - defensePoints > 0)
		{
			healthPoints -= dmg - defensePoints;
		}
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
		unit.SendMessage("TakeDamage", attackPoints);		
	}
	
	public float AddTamePointsByRate (int tameAmount, string teamToSwitchTo)
	{
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
	
}