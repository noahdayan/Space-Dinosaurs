using UnityEngine;
using System.Collections;

public class AutoMove : MonoBehaviour {
	
	public ObjectSelection aSelfObjectSelection;
	
	public TileManager aTileManager;
	
	public float aSpeedOfMovement = 4.0f;
	private bool aIsObjectMoving = false;
	public static bool aRobotsTurn = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (aRobotsTurn)
		{
			if (aTileManager.tileIsSelected())
			{
				aIsObjectMoving = true;
				
				Vector3 destination = aTileManager.pickRandomTile().transform.position;
				destination.y = transform.position.y;
				
				// slide to location
				transform.position += (destination - transform.position).normalized * aSpeedOfMovement * Time.deltaTime;
				
				// check to see if object has reached destination tile. if so, stop movement.
				if ( (Mathf.Abs(transform.position.x - destination.x) < 0.5) && (Mathf.Abs(transform.position.z - destination.z) < 0.5))
				{
					transform.position = destination;
					aSelfObjectSelection.deselectObject();
					aIsObjectMoving = false;
					aRobotsTurn = false;
				}
			}
		}
	}
	
	public bool isObjectMoving()
	{
		return aIsObjectMoving;
	}
}
