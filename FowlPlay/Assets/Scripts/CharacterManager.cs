using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CharacterManager : MonoBehaviour {

	public static GameObject aCurrentlySelectedUnit;
	public static bool aSingleUnitIsSelected = false;
	private List<GameObject> player1Units;
	private List<GameObject> player2Units;
	public static Vector3 startPos;
	public static Vector3 startRot;
	
	public static Hashtable unitsHT;
	
	// Can be either 1 or 2 or 3 or 4
	public static int aTurn = 1;
	
	// Use this for initialization
	void Start () {
		
		player1Units = new List<GameObject>();
		player2Units = new List<GameObject>();
		GameObject[] temp1 = GameObject.FindGameObjectsWithTag("Player1");
		foreach (GameObject unit in temp1)
		{
			player1Units.Add(unit);
			unitsHT.Add(unit.transform.position, unit);
		}
		GameObject[] temp2 = GameObject.FindGameObjectsWithTag("Player2");
		foreach (GameObject unit in temp2)
		{
			player2Units.Add(unit);
			unitsHT.Add(unit.transform.position, unit);
		}
		GameObject.Find("GUI Hot Seat").SendMessage("showText", "Player 1's Turn");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void selectUnit(GameObject pUnit)
	{
		startPos = pUnit.transform.position;
		startRot = pUnit.transform.rotation.eulerAngles;
		aCurrentlySelectedUnit = pUnit;
		pUnit.renderer.material.color = Color.yellow;
		aSingleUnitIsSelected = true;
		
		// highlight tiles in range
		if (!ClickAndMove.aIsObjectMoving && (aTurn == 1 || aTurn ==3))
			GameObject.Find("Character").SendMessage("highlightRange", pUnit);
	}
	
	public static void deselect()
	{
		aCurrentlySelectedUnit.renderer.material.color = Color.blue;
		aCurrentlySelectedUnit = null;
		aSingleUnitIsSelected = false;
		
		// un-highlight tiles in range
		if (!ClickAndMove.aIsObjectMoving && (aTurn == 1 || aTurn ==3))
			GameObject.Find("Character").SendMessage("unhighlightRange");
	}
	
	public static void switchTurn()
	{
		if (aTurn == 1)
		{
			GameObject.Find("GUI Hot Seat").SendMessage("showText", "Untamed Turn");
			GameObject.Find("Character").SendMessage("pickRandomTile");
			aTurn = 2;
		}
		else if (aTurn == 2)
		{
			GameObject.Find("GUI Hot Seat").SendMessage("showText", "Player 2's Turn");
			aTurn = 3;
		}
		else if (aTurn == 3)
		{
			GameObject.Find("GUI Hot Seat").SendMessage("showText", "Untamed's Turn");
			GameObject.Find("Character").SendMessage("pickRandomTile");
			aTurn = 4;
		}
		else if (aTurn == 4)
		{
			GameObject.Find("GUI Hot Seat").SendMessage("showText", "Player 1's Turn");
			aTurn = 1;
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
