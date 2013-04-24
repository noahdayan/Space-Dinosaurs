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
	
	public Rect barArea;
	public Rect mapArea;
	public Rect faceCamArea;
	public Rect miniGameArea;
	public Rect mugshotArea;
	Rect barAreaNormalized;
	Rect mapAreaNormalized;
	Rect faceCamAreaNormalized;
	Rect miniGameAreaNormalized;
	Rect mugshotAreaNormalized;
	
	public Texture chickenMug;
	public Texture turkeyMug;
	
	public static bool inMiniGame = false;
	public bool newHUD = false;
	
	// Use this for initialization
	void Start () {
		barAreaNormalized = new Rect(barArea.x, barArea.y, barArea.width, barArea.height);
		mapAreaNormalized = new Rect(Screen.width - mapArea.width + mapArea.x, Screen.height - mapArea.height + mapArea.y, mapArea.width, mapArea.height);
		faceCamAreaNormalized = new Rect(faceCamArea.x, Screen.height - faceCamArea.height + faceCamArea.y, faceCamArea.width, faceCamArea.height);
		miniGameAreaNormalized = new Rect(miniGameArea.x * Screen.width - (miniGameArea.width * 0.5f), miniGameArea.y * Screen.height - (miniGameArea.height * 0.5f), miniGameArea.width, miniGameArea.height);
		mugshotAreaNormalized = new Rect(Screen.width - mugshotArea.width + mugshotArea.x, mugshotArea.y, mugshotArea.width, mugshotArea.height);
		
		//Camera cam = GameObject.Find("Mini Game Camera").camera;
		//cam.pixelRect = miniGameAreaNormalized;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		GUI.skin = backgroundSkin;
		GUI.depth = guiDepth;
		
		GUI.Window(0, barAreaNormalized, ProgressWindow, "");
		GUI.Window(1, mapAreaNormalized, MapWindow, "");
		GUI.Window(2, faceCamAreaNormalized, FaceCamWindow, "");
		if (inMiniGame)
			GUI.Window(3, miniGameAreaNormalized, MiniGameWindow, "");
		GUI.Window(4, mugshotAreaNormalized, MugshotWindow, "");
		
		GameObject.Find("HUD Mini Map Camera").camera.Render();
		foreach(Camera c in Camera.allCameras)
		{
			if(c.camera.enabled && c != Camera.main)
			{
				c.camera.Render();
			}
		}
	}
	
	void ProgressWindow(int id) {
		if(!newHUD)
		{
			AddSpikes(barAreaNormalized.width);
		}
	}
	
	void MapWindow(int id) {
		if(!newHUD)
		{
			AddSpikes(mapAreaNormalized.width);
		}
	}
	
	void FaceCamWindow(int id) {
		if(!newHUD)
		{
			AddSpikes(faceCamAreaNormalized.width);
		}
	}
	
	void MiniGameWindow(int id) {
		if(!newHUD)
		{
			AddSpikes(miniGameAreaNormalized.width);
			FancyTop((int)miniGameAreaNormalized.width);
		}
	}
	
	void MugshotWindow(int id) {		
		if(CharacterManager.aTurn == 1)
		{
			GUI.DrawTexture(new Rect(50, 80, 100, 100), chickenMug);
		}
		else if(CharacterManager.aTurn == 3)
		{
			GUI.DrawTexture(new Rect(50, 80, 100, 100), turkeyMug);
		}
		if(!newHUD)
		{
			AddSpikes(mugshotAreaNormalized.width);
			leafOffset = ((int)mugshotAreaNormalized.width/2)-64;
			GUI.Label(new Rect(leafOffset, 168, 0, 0), "", "GoldLeaf");
		}
		frameOffset = ((int)mugshotAreaNormalized.width/2)-27;
		GUI.Label(new Rect(frameOffset, 153, 0, 0), "", "IconFrame");
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
