using UnityEngine;
using System.Collections;

public class ProgressBarGUI : MonoBehaviour {
	
	public GUISkin hudSkin;
	public int guiDepth = 0;
	public static float healthBar = 0.5f;
	public static float tamenessBar = 0.5f;
	Rect barAreaNormalized;
	public Rect barArea;
	public Vector2 healthPos;
	public Vector2 healthSize;
	public Vector2 tamenessPos;
	public Vector2 tamenessSize;
	public Texture2D barEmpty;
	public Texture2D barFull;
	
	// Use this for initialization
	void Start () {
		barAreaNormalized = new Rect(barArea.x * Screen.width - (barArea.width * 0.5f), barArea.y * Screen.height - (barArea.height * 0.5f), barArea.width, barArea.height);
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	void OnGUI() {
		GUI.skin = hudSkin;
		GUI.depth = guiDepth;
		GUI.BeginGroup(barAreaNormalized);
		
			GUI.BeginGroup(new Rect(healthPos.x, healthPos.y, healthSize.x, healthSize.y));
			GUI.Box(new Rect(0,0, healthSize.x, healthSize.y), barEmpty);
				GUI.BeginGroup(new Rect(0,0, healthSize.x * healthBar, healthSize.y));
				GUI.Box(new Rect(0,0, healthSize.x, healthSize.y), barFull);
				GUI.EndGroup();
			GUI.EndGroup();
		
			GUI.BeginGroup(new Rect(tamenessPos.x, tamenessPos.y, tamenessSize.x, tamenessSize.y));
			GUI.Box(new Rect(0,0, tamenessSize.x, tamenessSize.y), barEmpty);
				GUI.BeginGroup(new Rect(0,0, tamenessSize.x * tamenessBar, tamenessSize.y));
				GUI.Box(new Rect(0,0, tamenessSize.x, tamenessSize.y), barFull);
				GUI.EndGroup();
			GUI.EndGroup();
		
		GUI.EndGroup();
    }
}
