using UnityEngine;
using System.Collections;

public class ProgressBarGUI : MonoBehaviour {
	
	public GUISkin hudSkin;
	public int guiDepth = 0;
	public static int healthPoints = 10;
	public static int tamePoints = 10;
	public static int attackPoints = 10;
	public static int defensePoints = 10;
	public static int moveRange = 10;
	public static int attackRange = 10;
	Rect barAreaNormalized;
	public Rect barArea;
	public Rect healthPointsArea;
	public Rect tamePointsArea;
	public Rect attackPointsArea;
	public Rect defensePointsArea;
	public Rect moveRangeArea;
	public Rect attackRangeArea;
	public static bool show = false;
	public static bool isBird = false;
	
	// Use this for initialization
	void Start () {
		barAreaNormalized = new Rect(barArea.x, barArea.y, barArea.width, barArea.height);
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	void OnGUI() {
		GUI.skin = hudSkin;
		GUI.depth = guiDepth;
		if(show && !PauseMenuGUI.isPaused)
		{
			GUI.BeginGroup(barAreaNormalized);
			if (!isBird)
			{
				GUI.Label(new Rect(tamePointsArea), tamePoints.ToString());
			}
			GUI.Label(new Rect(healthPointsArea), healthPoints.ToString());
			GUI.Label(new Rect(attackPointsArea), attackPoints.ToString());
			GUI.Label(new Rect(defensePointsArea), defensePoints.ToString());
			GUI.Label(new Rect(moveRangeArea), moveRange.ToString());
			GUI.Label(new Rect(attackRangeArea), attackRange.ToString());
			GUI.EndGroup();
		}
    }
}
