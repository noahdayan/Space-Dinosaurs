using UnityEngine;
using System.Collections;

public class ProgressBarGUI : MonoBehaviour {
	
	public float healthBar;
	Rect barAreaNormalized;
	public Rect barArea;
	public Vector2 pos;
	public Vector2 size;
	public GUISkin hudSkin;
	
	// Use this for initialization
	void Start () {
		barAreaNormalized = new Rect(barArea.x * Screen.width - (barArea.width * 0.5f), barArea.y * Screen.height - (barArea.height * 0.5f), barArea.width, barArea.height);
	}
	
	// Update is called once per frame
	void Update () {
		healthBar = Time.time * 0.05f;
	}
	
	void OnGUI() {
		GUI.skin = hudSkin;
		GUI.BeginGroup(barAreaNormalized);
			GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
			GUI.Box(new Rect(0,0, size.x, size.y), "");
				GUI.BeginGroup(new Rect(0,0, size.x * healthBar, size.y));
				GUI.Box(new Rect(0,0, size.x, size.y), "");
				GUI.EndGroup();
			GUI.EndGroup();
		GUI.EndGroup();
    }
}
