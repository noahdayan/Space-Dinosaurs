using UnityEngine;
using System.Collections;

public class BirdUnitFunctionalityAndStats : MonoBehaviour {

	public int tamePower = 50;
	public int healthPoints = 100;
	public int maxHealthPoints = 100;
	public int defensePoints = 10;
	public int attackPoints = 15;
	public int moveCost = 1;
	public int moveRange = 3;
	public GameObject deathParticle;
	
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
	
	public void MovementCost (GameObject player)
	{
		player.SendMessage("RemoveAp", moveCost);
	}
	
	public void AttackUnit (GameObject unit)
	{
		unit.SendMessage("TakeAttackDamage", attackPoints);
		//maybe remove AP here as well based on an attack cost?
	}
	
	public void TameUnit (GameObject unit)
	{
		
		int[] theParamters = {tamePower, -1};
		//theParamters[0] = tamePower;
		//theParamters[1] = -1; //error value, no team to switch to.
		
		//Getting the tag to send to the unit being tamed. Stupid SendMessage only takes one parameter...
		if (gameObject.tag == "Player1")
		{
			theParamters[1] = 1;
		}
		else if (gameObject.tag == "Player2")
		{
			theParamters[1] = 2;
		}
		
		if (theParamters[1] != -1)
		{
			unit.SendMessage("AddTamePointsByRate", theParamters);
			//Maybe remove AP from the player here as well based on the tame cost?
		}
	}
	
	public void OnSelectedSetHud()
	{
		//Set the attributes of the GUI via send message and sending this units current stats and max stats.
	}
	
	public void Die()
	{
		GameObject instance = Instantiate(deathParticle, transform.position, deathParticle.transform.rotation) as GameObject;
		Destroy(gameObject);
	}
	
	public void UpdateGuiHealthBar()
	{
		ProgressBarGUI.healthBar = (float)healthPoints / (float)maxHealthPoints;
	}
	
}
