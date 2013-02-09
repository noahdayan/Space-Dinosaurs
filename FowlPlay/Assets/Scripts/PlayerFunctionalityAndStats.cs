using UnityEngine;
using System.Collections;

public class PlayerFunctionalityAndStats : MonoBehaviour {

	public int AbilityPower;
	
	
	public int RemoveAp (int amountToRemove)
	{
		AbilityPower -= amountToRemove;
		return AbilityPower;
	}
}
