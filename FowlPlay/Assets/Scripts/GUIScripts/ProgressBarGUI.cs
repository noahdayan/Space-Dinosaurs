using UnityEngine;
using System.Collections;

public class ProgressBarGUI : MonoBehaviour {
	
	public Font font;
	public int fontSize;
	GUIStyle style;
	public int guiDepth = 0;
	public Color tpColor;
	public Color hpColor;
	public Color apColor;
	public Color dpColor;
	public Color mrColor;
	public Color arColor;
	public Color vsColor;
	public static int healthPoints1 = 10;
	public static int tamePoints1 = 10;
	public static int maxHealthPoints1 = 10;
	public static int maxTamePoints1 = 10;
	public static int attackPoints1 = 10;
	public static int defensePoints1 = 10;
	public static int moveRange1 = 10;
	public static int attackRange1 = 10;
	public static int healthPoints2 = 10;
	public static int tamePoints2 = 10;
	public static int maxHealthPoints2 = 10;
	public static int maxTamePoints2 = 10;
	public static int attackPoints2 = 10;
	public static int defensePoints2 = 10;
	public static int moveRange2 = 10;
	public static int attackRange2 = 10;
	Rect areaNormalized;
	public Rect area;
	Rect barAreaNormalized;
	public Rect barArea;
	Rect barAreaNormalized1;
	public Rect barArea1;
	Rect barAreaNormalized2;
	public Rect barArea2;
	public Rect vsArea;
	public Rect healthPointsArea;
	public Rect tamePointsArea;
	public Rect attackPointsArea;
	public Rect defensePointsArea;
	public Rect moveRangeArea;
	public Rect attackRangeArea;
	public Rect healthPointsArea1;
	public Rect tamePointsArea1;
	public Rect attackPointsArea1;
	public Rect defensePointsArea1;
	public Rect moveRangeArea1;
	public Rect attackRangeArea1;
	public Rect healthPointsArea2;
	public Rect tamePointsArea2;
	public Rect attackPointsArea2;
	public Rect defensePointsArea2;
	public Rect moveRangeArea2;
	public Rect attackRangeArea2;
	public static bool show1 = false;
	public static bool isBird1 = false;
	public static bool show2 = false;
	public static bool isBird2 = false;
	
	// Use this for initialization
	void Start () {
		areaNormalized = new Rect(area.x, area.y, area.width, area.height);
		barAreaNormalized = new Rect(barArea.x, barArea.y, barArea.width, barArea.height);
		barAreaNormalized1 = new Rect(barArea1.x, barArea1.y, barArea1.width, barArea1.height);
		barAreaNormalized2 = new Rect(barArea2.x, barArea2.y, barArea2.width, barArea2.height);
		
		style = new GUIStyle();
		style.font = font;
		style.fontSize = fontSize;
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	void OnGUI() {
		GUI.depth = guiDepth;
		GUI.BeginGroup(areaNormalized);
		
			GUI.BeginGroup(barAreaNormalized);
			style.normal.textColor = tpColor;
			GUI.Label(new Rect(tamePointsArea), "TP:", style);
			style.normal.textColor = hpColor;
			GUI.Label(new Rect(healthPointsArea), "HP:", style);
			style.normal.textColor = apColor;
			GUI.Label(new Rect(attackPointsArea), "AP:", style);
			style.normal.textColor = dpColor;
			GUI.Label(new Rect(defensePointsArea), "DP:", style);
			style.normal.textColor = mrColor;
			GUI.Label(new Rect(moveRangeArea), "MR:", style);
			style.normal.textColor = arColor;
			GUI.Label(new Rect(attackRangeArea), "AR:", style);
			GUI.EndGroup();
		
		if(!PauseMenuGUI.isPaused)
		{
			if(show1)
			{
				GUI.BeginGroup(barAreaNormalized1);
				if (!isBird1)
				{
					style.normal.textColor = tpColor;
					GUI.Label(new Rect(tamePointsArea1), tamePoints1.ToString() + "/" + maxTamePoints1.ToString(), style);
				}
				style.normal.textColor = hpColor;
				GUI.Label(new Rect(healthPointsArea1), healthPoints1.ToString() + "/" + maxHealthPoints1.ToString(), style);
				style.normal.textColor = apColor;
				GUI.Label(new Rect(attackPointsArea1), attackPoints1.ToString(), style);
				style.normal.textColor = dpColor;
				GUI.Label(new Rect(defensePointsArea1), defensePoints1.ToString(), style);
				style.normal.textColor = mrColor;
				GUI.Label(new Rect(moveRangeArea1), moveRange1.ToString(), style);
				style.normal.textColor = arColor;
				GUI.Label(new Rect(attackRangeArea1), attackRange1.ToString(), style);
				GUI.EndGroup();
			}
			if(show1 || show2)
			{
				style.normal.textColor = vsColor;
				GUI.Label(new Rect(vsArea), "VS", style);
			}
			if(show2)
			{
				GUI.BeginGroup(barAreaNormalized2);
				if (!isBird2)
				{
					style.normal.textColor = tpColor;
					GUI.Label(new Rect(tamePointsArea2), tamePoints2.ToString() + "/" + maxTamePoints2.ToString(), style);
				}
				style.normal.textColor = hpColor;
				GUI.Label(new Rect(healthPointsArea2), healthPoints2.ToString() + "/" + maxHealthPoints2.ToString(), style);
				style.normal.textColor = apColor;
				GUI.Label(new Rect(attackPointsArea2), attackPoints2.ToString(), style);
				style.normal.textColor = dpColor;
				GUI.Label(new Rect(defensePointsArea2), defensePoints2.ToString(), style);
				style.normal.textColor = mrColor;
				GUI.Label(new Rect(moveRangeArea2), moveRange2.ToString(), style);
				style.normal.textColor = arColor;
				GUI.Label(new Rect(attackRangeArea2), attackRange2.ToString(), style);
				GUI.EndGroup();
			}
		}
		GUI.EndGroup();
    }
}
