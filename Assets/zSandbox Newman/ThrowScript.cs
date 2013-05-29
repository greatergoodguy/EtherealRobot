using UnityEngine;
using System.Collections;

public class ThrowScript : MonoBehaviour {
	
	static public Font 	FontReplaceSmall	= null;
	static public Font 	FontReplaceLarge	= null;
	static private int    	StereoSpreadX 	= -40;	
	
	// Spacing for scenes menu
	private int    	StartX			= 300;
	private int    	StartY			= 170;
	private int    	WidthX			= 50;
	private int    	WidthY			= 30;
	private int 	ButtonOffsetY 	= 30;	// Distance each new button is created from previous
	
	private bool 	isGuiOn			= true;
	
	private int SelectedIndex = 1;	// Indicates what key is currently selected by Keyboard
	private int NumButtons = 0;		// Total # of buttons. Gets updated in 'AddButton' inefficiently
	
	private string start_s = "Start";
	
	// Use this for initialization
	void Start () {
		AddButton ("Start", 1);
		AddButton ("Exit", 2);
		AddButton ("Extra" , 3);

	}
	
	// Update is called once per frame
	void Update () {
		KeyboardMenuSelection();
	}
	
	void OnGUI(){
		if(!isGuiOn){
			return;	
		}
		
		/* ADD NEW BUTTONS HERE (The int determines where each button is placed) */

		//GUIStereoBox (StartX, StartY, WidthX, WidthY, ref start_s, Color.yellow);
	}
	
		// Create buttons that highlight when selected via keyboard
	public static void GUIStereoButton(Rect rect, string text, bool curButton){
		int X = (int) rect.x;
		int Y = (int) rect.y;
		int wX = (int) rect.width; 
		int wY = (int) rect.height;
		
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
		
		//GUI.contentColor = color;
		
		int sWX = (int)((float)wX * sSX);
		int sWY = (int)((float)wY * sSY);
		
		// Change font size based on screen scale
		if(Screen.height > 800)
			GUI.skin.font = FontReplaceLarge;
		else
			GUI.skin.font = FontReplaceSmall;
		
		// Change color if selected by keyboard
		if(curButton){//SelectedIndex == index){
			GUI.color = Color.yellow;	
		}
		GUI.Button(new Rect(xL, y, sWX, sWY), text);
		GUI.Button(new Rect(xR, y, sWX, sWY), text);		
		
		GUI.color = Color.white;
		
		//print("xL: " + xL + "       xR: " + xR);
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
	
	// Determines what each button does when pressed
	void KeyboardMenuSelection() {
		if(Input.GetKeyDown(KeyCode.Return)){
			if (SelectedIndex == 1){
				Application.LoadLevel(0);	
			}
			if(SelectedIndex == 2){
				// Exit game	
			}	
		}
		
		// Moves between buttons with arrows keys
		if(Input.GetKeyDown(KeyCode.DownArrow) && SelectedIndex < NumButtons){
			SelectedIndex++;
		}
		if(Input.GetKeyDown(KeyCode.UpArrow) && SelectedIndex > 1){
			SelectedIndex--;
		}
	}
	
	void AddButton(string text, int index){
		// Should change this. (Makes it easier to add new butttons, but adds unneeded computations
		if(index > NumButtons){
			NumButtons = index;
		}
		//GUIStereoButton(new Rect(StartX, StartY + (index * ButtonOffsetY), WidthX, WidthY), text, index);
		bool curButton = false;
		if (SelectedIndex == index){
			curButton = true;	
		}
		
		GUIStereoButton(new Rect(StartX, StartY + (index * ButtonOffsetY), WidthX, WidthY), text, curButton);
	}	

}
