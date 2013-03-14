using UnityEngine;
using System.Collections;

public class PlayerFunctionalityAndStats : MonoBehaviour {

	private int mana;
	public int manaPerTurn = 8;
	public int startingMana = 10;
	public int bonus = 0;
	
	public static bool isLegalMove = true;
	
	public bool isTurn = false;
	
	void Start()
	{
		isTurn = true;
		mana = startingMana;
		UpdateGuiMana();
	}
	
	void Update()
	{
		if (mana <= 0 && isTurn)
		{
			GameObject.Find("Character").SendMessage("endTurn");
		}
	}
	
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
	
	public void SetOriginalMana()
	{
		CharacterManager.aOriginalMana = mana;
	}
	
	public int RestoreMana ()
	{
		mana = CharacterManager.aOriginalMana;
		UpdateGuiMana();
		return mana;
	}
	
	public void StartTurn()
	{
		if (CharacterManager.aTurnCount > 3)
		{
			mana += manaPerTurn + bonus;
			isTurn = true;
			UpdateGuiMana();
		}
	}
	
	public void UpdateGuiMana()
	{
		ManaPointsGUI.manaPoints = mana;
	}
	
	public void CheckLegalMove (int manaCost)
	{
		if (manaCost > mana)
		{
			isLegalMove = false;
		}
		else
		{
			isLegalMove = true;
		}
	}
}
