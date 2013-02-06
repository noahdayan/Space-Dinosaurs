using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CharacterManager : MonoBehaviour {

	public static GameObject aCurrentlySelectedUnit;
	public static bool aSingleUnitIsSelected = false;
	private List<GameObject> player1Units;
	private List<GameObject> player2Units;
	
	// Can be either 1 or 2
	public static int aTurn = 1;
	
	// Use this for initialization
	void Start () {
		//GameObject[] temp1 = GameObject.FindGameObjectsWithTag("Player1");
		//foreach (GameObject unit in temp1)
			//player1Units.Add(unit);
		
		//GameObject[] temp2 = GameObject.FindGameObjectsWithTag("Player2");
		//foreach (GameObject unit in temp2)
			//player2Units.Add(unit);
		
		GameObject.Find("GUI Hot Seat").SendMessage("showText", "Player 1's Turn");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public static void selectUnit(GameObject pUnit)
	{
		aCurrentlySelectedUnit = pUnit;
		pUnit.renderer.material.color = Color.yellow;
		aSingleUnitIsSelected = true;
	}
	
	public static void deselect()
	{
		aCurrentlySelectedUnit.renderer.material.color = Color.blue;
		aCurrentlySelectedUnit = null;
		aSingleUnitIsSelected = false;
	}
	
	public static void switchTurn()
	{
		if (aTurn == 1)
		{
			aTurn = 2;
			GameObject.Find("GUI Hot Seat").SendMessage("showText", "Player 2's Turn");
		}
		else if (aTurn == 2)
		{
			aTurn = 1;
			GameObject.Find("GUI Hot Seat").SendMessage("showText", "Player 1's Turn");
		}
	}
	
	/**public Vector3 unitPosition(int pPlayer)
	{
		if (pPlayer == 0)
		{
			return aPlayer.transform.position;
		}
		
		else
		{
			return aEnemy.transform.position;
		}
	} */
}
