using UnityEngine;
using System.Collections;

public class CharacterManager : MonoBehaviour {

	public GameObject aCurrentlySelectedUnit;
	
	public ClickAndMove aObjectMovement;
	
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
}
