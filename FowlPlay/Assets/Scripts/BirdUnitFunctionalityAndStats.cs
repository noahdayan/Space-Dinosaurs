using UnityEngine;
using System.Collections;

public class BirdUnitFunctionalityAndStats : MonoBehaviour {

	public int healthPoints, defensePoints, attackPoints;
	public int moveCost;
	public int tamePower;
	
	
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
	
	public void MovementCost (GameObject player)
	{
		player.SendMessage("RemoveAp", moveCost);
	}
	
	public void AttackUnit (GameObject unit)
	{
		unit.SendMessage("TakeDamage", attackPoints);
		//maybe remove AP here as well based on an attack cost?
	}
	
	public void TameUnit (GameObject unit)
	{
		//unit.SendMessage("AddTamePointsByRate", tamePower, gameObject.tag);
		//Maybe remove AP from the player here as well based on the tame cost?
	}
	
	public void onSelectSetHud()
	{
		//Set the attributes of the GUI via send message and sending this units current stats and max stats.
	}
	
}
