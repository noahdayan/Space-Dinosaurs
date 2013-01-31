using UnityEngine;
using System.Collections;

public class CharacterManager : MonoBehaviour {

	public GameObject aCurrentlySelectedUnit;
	
	public GameObject aPlayer;
	
	public GameObject aEnemy;
	
	private bool aSingleUnitIsSelected = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void selectUnit(GameObject pUnit)
	{
		aCurrentlySelectedUnit = pUnit;
		aSingleUnitIsSelected = true;
	}
	
	public void deselect()
	{
		aCurrentlySelectedUnit = null;
		aSingleUnitIsSelected = false;
	}
	
	public bool UnitIsSelected()
	{
		return aSingleUnitIsSelected;
	}
	
	public Vector3 unitPosition(int pPlayer)
	{
		if (pPlayer == 0)
		{
			return aPlayer.transform.position;
		}
		
		else
		{
			return aEnemy.transform.position;
		}
	}
}
