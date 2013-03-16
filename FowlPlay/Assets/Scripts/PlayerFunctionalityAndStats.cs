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
		iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", startingMana, "onupdate", "UpdateGuiMana"));
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
		iTween.ValueTo(gameObject, iTween.Hash("from", CharacterManager.aOriginalMana, "to", mana, "onupdate", "UpdateGuiMana"));
		return mana;
	}
	
	public void SetOriginalMana()
	{
		CharacterManager.aOriginalMana = mana;
	}
	
	public int RestoreMana ()
	{
		iTween.ValueTo(gameObject, iTween.Hash("from", mana, "to", CharacterManager.aOriginalMana, "onupdate", "UpdateGuiMana"));
		mana = CharacterManager.aOriginalMana;
		return mana;
	}
	
	public void StartTurn()
	{
		int temp = mana;
		mana += manaPerTurn + bonus;
		isTurn = true;
		iTween.ValueTo(gameObject, iTween.Hash("from", temp, "to", mana, "onupdate", "UpdateGuiMana"));
	}
	
	public void UpdateGuiMana(int newValue)
	{
		ManaPointsGUI.manaPoints = newValue;
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
