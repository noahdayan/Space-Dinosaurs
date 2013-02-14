using UnityEngine;
using System.Collections;

public class BackgroundGUI : MonoBehaviour {
	
	public GUISkin backgroundSkin;
	public int guiDepth = 0;
	
	private int leafOffset;
	private int frameOffset;
	private int skullOffset;
	
	private int RibbonOffsetX;
	private int FrameOffsetX;
	private int SkullOffsetX;
	private int RibbonOffsetY;
	private int FrameOffsetY;
	private int SkullOffsetY;
	
	private int WSwaxOffsetX;
	private int WSwaxOffsetY;
	private int WSribbonOffsetX;
	private int WSribbonOffsetY;
		
	private int spikeCount;
	
	public Rect windowArea;
	public Rect mapArea;
	Rect windowAreaNormalized;
	Rect mapAreaNormalized;
	
	// Use this for initialization
	void Start () {
		windowAreaNormalized = new Rect(windowArea.x * Screen.width - (windowArea.width * 0.5f), windowArea.y * Screen.height - (windowArea.height * 0.5f), windowArea.width, windowArea.height);
		mapAreaNormalized = new Rect(mapArea.x * Screen.width - (mapArea.width * 0.5f), mapArea.y * Screen.height - (mapArea.height * 0.5f), mapArea.width, mapArea.height);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		GUI.skin = backgroundSkin;
		GUI.depth = guiDepth;
		
		GUI.Window(0, windowAreaNormalized, ProgressWindow, "");
		GUI.Window(1, mapAreaNormalized, MapWindow, "");
		
		GameObject.Find("HUD Mini Map Camera").camera.Render();
	}
	
	void ProgressWindow(int id) {
		AddSpikes(windowAreaNormalized.width);
	}
	
	void MapWindow(int id) {
		AddSpikes(mapAreaNormalized.width);
	}
	
	void AddSpikes(float winX)
	{
		spikeCount = Mathf.FloorToInt(winX - 152)/22;
		GUILayout.BeginHorizontal();
		GUILayout.Label ("", "SpikeLeft");//-------------------------------- custom
		for (int i = 0; i < spikeCount; i++)
        {
			GUILayout.Label ("", "SpikeMid");//-------------------------------- custom
        }
		GUILayout.Label ("", "SpikeRight");//-------------------------------- custom
		GUILayout.EndHorizontal();
	}
	
	void FancyTop(int topX)
	{
		leafOffset = (topX/2)-64;
		frameOffset = (topX/2)-27;
		skullOffset = (topX/2)-20;
		GUI.Label(new Rect(leafOffset, 18, 0, 0), "", "GoldLeaf");//-------------------------------- custom	
		GUI.Label(new Rect(frameOffset, 3, 0, 0), "", "IconFrame");//-------------------------------- custom	
		GUI.Label(new Rect(skullOffset, 12, 0, 0), "", "Skull");//-------------------------------- custom	
	}
	
	void WaxSeal(int x, int y)
	{
		WSwaxOffsetX = x - 120;
		WSwaxOffsetY = y - 115;
		WSribbonOffsetX = x - 114;
		WSribbonOffsetY = y - 83;
		
		GUI.Label(new Rect(WSribbonOffsetX, WSribbonOffsetY, 0, 0), "", "RibbonBlue");//-------------------------------- custom	
		GUI.Label(new Rect(WSwaxOffsetX, WSwaxOffsetY, 0, 0), "", "WaxSeal");//-------------------------------- custom	
	}
	
	void DeathBadge(int x, int y)
	{
		RibbonOffsetX = x;
		FrameOffsetX = x+3;
		SkullOffsetX = x+10;
		RibbonOffsetY = y+22;
		FrameOffsetY = y;
		SkullOffsetY = y+9;
		
		GUI.Label(new Rect(RibbonOffsetX, RibbonOffsetY, 0, 0), "", "RibbonRed");//-------------------------------- custom	
		GUI.Label(new Rect(FrameOffsetX, FrameOffsetY, 0, 0), "", "IconFrame");//-------------------------------- custom	
		GUI.Label(new Rect(SkullOffsetX, SkullOffsetY, 0, 0), "", "Skull");//-------------------------------- custom	
	}
}
