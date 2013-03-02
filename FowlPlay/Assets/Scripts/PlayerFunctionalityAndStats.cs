using UnityEngine;
using System.Collections;

public class PlayerFunctionalityAndStats : MonoBehaviour {

	private int mana;
	public int manaPerTurn = 3;
	public int startingMana = 10;
	public int bonus = 0;
	
	public bool isTurn = false;
	
	void Start()
	{
		mana = startingMana;
		UpdateGuiMana();
	}
	
	/*void Update()
	{
		if (mana <= 0 && isTurn)
		{
			isTurn = false;
			EndTurn();
		}
	}*/
	
	public void PlayerEndTurn()
	{
		isTurn = false;
	}
	
	public int RemoveMana (int amountToRemove)
	{
		CharacterManager.aOriginalMana = mana;
		mana -= amountToRemove;
		UpdateGuiMana();
		return mana;
	}
	
	public int RestoreMana ()
	{
		mana = CharacterManager.aOriginalMana;
		UpdateGuiMana();
		return mana;
	}
	
	public void StartTurn()
	{
		mana += manaPerTurn + bonus;
		isTurn = true;
		UpdateGuiMana();
	}
	
	public void UpdateGuiMana()
	{
		ManaPointsGUI.manaPoints = mana;
	}
	
	public bool CheckLegalMove (int manaCost)
	{
		if (manaCost > mana)
		{
			return false;
		}
		else
		{
			return true;
		}
	}
}
