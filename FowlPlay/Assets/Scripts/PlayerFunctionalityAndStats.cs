using UnityEngine;
using System.Collections;

public class PlayerFunctionalityAndStats : MonoBehaviour {

	public int AbilityPower;
	public int APPerTurn;
	
	
	public void PlayerEndTurn()
	{
		//SendMessage End turn
	}
	
	public int RemoveAp (int amountToRemove)
	{
		AbilityPower -= amountToRemove;
		return AbilityPower;
	}
	
	public void StartTurn()
	{
		AbilityPower += APPerTurn;
	}
}
