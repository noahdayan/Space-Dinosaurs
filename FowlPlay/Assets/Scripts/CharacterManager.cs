using UnityEngine;
using System.Collections;

public class CharacterManager : MonoBehaviour {

	public static GameObject aCurrentlySelectedUnit;

	public static bool aSingleUnitIsSelected = false;
	
	private GameObject[] allUnits;
	
	// Use this for initialization
	void Start () {
		allUnits = GameObject.FindGameObjectsWithTag("GameUnit");
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
