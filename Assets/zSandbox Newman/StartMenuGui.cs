using UnityEngine;
using System.Collections;

public class StartMenuGui : MonoBehaviour {
	
	// textures
	public Texture startTexture;
	
	// Spacing for scenes menu
	private int    	StartX			= 300;
	private int    	StartY			= 170;
	private int    	WidthX			= 50;
	private int    	WidthY			= 30;
	private int 	ButtonOffsetY 	= 30;	// Distance each new button is created from previous
	
	private bool 	isGuiOn			= true;
	
	private int SelectedIndex = 0;	// Indicates what key is currently selected by Keyboard
	
	static private int NumButtons = 2;  //
	private Color[] ButtonColors = new Color[NumButtons];
	
	// Use this for initialization
	void Start () {
		// Initializes GUI buttons to white, except for the selectedIndex
		ButtonColors[SelectedIndex] = Color.yellow;
		for(int i = 1; i < ButtonColors.Length; i++)
			ButtonColors[i] = Color.white;
		
		DebugUtils.Assert(startTexture != null);
	}
	
	// Update is called once per frame
	void Update () {
		KeyboardMenuSelection();
	}
	
	void OnGUI(){
		if(!isGuiOn){
			return;	
		}
		
		//Creates GUI textures
		GuiUtils.GUIStereoTexture(80, 80, 500, 500, startTexture);		
		
		// Creates Buttons
		// When adding new buttons, make sure to increase 'NumButtons' variable at top
		GuiUtils.GUIStereoButton (StartX, StartY, WidthX, WidthY, "Start", ButtonColors[0]);
		GuiUtils.GUIStereoButton (StartX, StartY + ButtonOffsetY, WidthX, WidthY, "Exit", ButtonColors[1]);
		
	}
	
	/*
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
	}*/
	
	// Determines what each button does when pressed
	void KeyboardMenuSelection() {
		if(Input.GetKeyDown(KeyCode.Return)){
			if (SelectedIndex == 0){
				Application.LoadLevel(0);	
			}
			if(SelectedIndex == 1){
				// Exit game	
			}	
		}
		
		// Moves between buttons with arrows keys
		if(Input.GetKeyDown(KeyCode.DownArrow) && SelectedIndex < NumButtons - 1){
			SelectedIndex++;
			ChangeButtonColor(SelectedIndex - 1);
		}
		if(Input.GetKeyDown(KeyCode.UpArrow) && SelectedIndex > 0){
			SelectedIndex--;
			ChangeButtonColor(SelectedIndex + 1);
		}
	}
	
	// Changes the color of the GUI button if it is currently selected by the keyboard
	void ChangeButtonColor(int prevIndex){
		ButtonColors[prevIndex] = Color.white;
		ButtonColors[SelectedIndex] = Color.yellow;
	}
}
