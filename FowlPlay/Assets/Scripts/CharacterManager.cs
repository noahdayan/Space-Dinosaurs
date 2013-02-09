using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CharacterManager : MonoBehaviour {

	public static GameObject aCurrentlySelectedUnit;
	public static Vector3 aCurrentlySelectedUnitOriginalPosition;
	public static bool aSingleUnitIsSelected = false;
	private static List<GameObject> player1Units;
	private static List<GameObject> player2Units;
	public static Vector3 startPos;
	public static Vector3 startRot;
	
	public static Hashtable unitsHT;
	
	// Can be either 1 or 2 or 3 or 4
	public static int aTurn = 1;
	
	// Use this for initialization
	void Start () {
		
		player1Units = new List<GameObject>();
		player2Units = new List<GameObject>();
		unitsHT = new Hashtable();
		
		GameObject[] temp1 = GameObject.FindGameObjectsWithTag("Player1");
		
		Debug.Log("Position of first object: " + temp1[0].transform.position);
		Debug.Log("Position of second object: " + temp1[1].transform.position);
		
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
		aCurrentlySelectedUnitOriginalPosition = pUnit.transform.position;
		pUnit.renderer.material.color = Color.yellow;
		aSingleUnitIsSelected = true;
		
		Debug.Log("Selected unit's vector3: " + pUnit.transform.position);
		
		// highlight tiles in range
		if (!ClickAndMove.aIsObjectMoving && (aTurn == 1 || aTurn ==3))
			GameObject.Find("Character").SendMessage("highlightRange", pUnit);
	}
	
	public static void deselect()
	{
		aCurrentlySelectedUnit.renderer.material.color = Color.blue;
		
		// if the unit has moved
		if(aCurrentlySelectedUnit.transform.position != aCurrentlySelectedUnitOriginalPosition)
		{
				unitsHT.Remove(aCurrentlySelectedUnitOriginalPosition);
				unitsHT.Add(aCurrentlySelectedUnit.transform.position, aCurrentlySelectedUnit);
		}
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
			/*foreach (GameObject unit in player1Units)
			{
				unit.SendMessage("EndTurnTickUntame", 1);
			}*/
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
}
