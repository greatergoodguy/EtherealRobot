using UnityEngine;
using System.Collections;

public class StartGui : MonoBehaviour {
	
	public Font 	FontReplaceSmall	= null;
	public Font 	FontReplaceLarge	= null;
	private int    	StereoSpreadX 	= -40;
	
	// Spacing for scenes menu
	private int    	StartX			= 200;
	private int    	StartY			= 200;
	private int    	WidthX			= 300;
	private int    	WidthY			= 50;
	
	private bool 	isGuiOn			= true;
	
	private string start_s = "Start";
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void OnGUI(){
		if(!isGuiOn){
			return;	
		}
		
		
		//GUI.Button (new Rect (10,10,150,100), "I am a button");				
		GUIStereoBox (StartX, StartY, WidthX, WidthY, ref start_s, Color.yellow);
	}
	
	// GUIStereoBox - Values based on pixels in DK1 resolution of W: (1280 / 2) H: 800
	void GUIStereoBox(int X, int Y, int wX, int wY, ref string text, Color color)
	{
		float ploLeft = 0, ploRight = 0;
		float sSX = (float)Screen.width / 1280.0f;
		
		float sSY = ((float)Screen.height / 800.0f);
		OVRDevice.GetPhysicalLensOffsets(ref ploLeft, ref ploRight); 
		int xL = (int)((float)X * sSX);
		int sSpreadX = (int)((float)StereoSpreadX * sSX);
		int xR = (Screen.width / 2) + xL + sSpreadX
			      // required to adjust for physical lens shift
			      - (int)(ploLeft * (float)Screen.width / 2);
		int y = (int)((float)Y * sSY);
		
		GUI.contentColor = color;
		
		int sWX = (int)((float)wX * sSX);
		int sWY = (int)((float)wY * sSY);
		
		// Change font size based on screen scale
		if(Screen.height > 800)
			GUI.skin.font = FontReplaceLarge;
		else
			GUI.skin.font = FontReplaceSmall;
		
		GUI.Box(new Rect(xL, y, sWX, sWY), text);
		GUI.Box(new Rect(xR, y, sWX, sWY), text);		
		
		//print("xL: " + xL + "       xR: " + xR);
	}
}
