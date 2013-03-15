using UnityEngine;
using System.Collections;

public class AnimationManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
		if(CharacterManager.aCurrentlySelectedUnit == gameObject)
		{
			if(!ClickAndMove.aIsObjectMoving && !CharacterManager.aInteractiveUnitIsSelected)
				transform.FindChild("model").animation.Play("standing");
			
			else if(ClickAndMove.aIsObjectMoving)
				transform.FindChild("model").animation.Play("walking");
		
			else if(CharacterManager.aMidTurn && CharacterManager.aInteractiveUnitIsSelected)
				transform.FindChild("model").animation.Play("attack");
		}
			
		else if(CharacterManager.aMidTurn)
		{
			if(!CharacterManager.aInteractiveUnitIsSelected)
				transform.FindChild("model").animation.Play("standing");
			
			else if(CharacterManager.aInteractiveUnitIsSelected && CharacterManager.aInteractUnit == gameObject)
				transform.FindChild("model").animation.Play("damage");
		}
		
		else
			transform.FindChild("model").animation.Play("standing");

	}
}
