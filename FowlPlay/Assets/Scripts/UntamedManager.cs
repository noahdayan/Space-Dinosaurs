using UnityEngine;
using System.Collections;

public class UntamedManager : MonoBehaviour {
	
	static GameObject charManager;
	
	// Use this for initialization
	void Start () {
		charManager = GameObject.Find("Character");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public static void RandomMovement()
	{
		foreach (GameObject untamedUnit in CharacterManager.untamedUnits)
		{
			ClickAndMove.aIsObjectMoving = true;	
			CharacterManager.aCurrentlySelectedUnit = untamedUnit;
			TileManager.aCurrentlySelectedTile = TileManager.pickRandomTile();
			TileManager.aSingleTileIsSelected = true;
			charManager.SendMessage("selectUnit", untamedUnit);
			charManager.SendMessage("move");
			charManager.SendMessage("deselectUnit");
			//yield return new WaitForSeconds(1.0f);
		}
	}
}
