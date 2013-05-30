using UnityEngine;
using System.Collections;

public class DebugGuiNW : MonoBehaviour {
	
	private int    	StartX			= 260;
	private int    	StartY			= 200;
	private int    	WidthX			= 200;
	private int    	WidthY			= 30;
	private int    	ButtonOffsetY	= 32;
	private int		SideButtonOffsetX = 30;
	
	private bool 	isGuiOn			= true;
	
	private int SelectedIndex = 0;	// Indicates what key is currently selected by Keyboard
	
	static private int NumButtons = 4;  //
	private Color[] ButtonColors = new Color[NumButtons];
	
	// Use this for initialization
	void Start () {
		// Initializes GUI buttons to white, except for the selectedIndex
		ButtonColors[SelectedIndex] = Color.yellow;
		for(int i = 1; i < ButtonColors.Length; i++)
			ButtonColors[i] = Color.white;
	}
	
	// Update is called once per frame
	void Update () {
		KeyboardMenuSelection();
	}
	
	void OnGUI(){
		if(!isGuiOn){
			return;	
		}
		
		GuiUtilsNW.GUIStereoButton (StartX, StartY - 60, WidthX, WidthY, "Name of controller here", Color.cyan);
		
		// Creates Buttons
		// When adding new buttons, make sure to increase 'NumButtons' variable at top
		GuiUtilsNW.GUIStereoButton (StartX, StartY, WidthX, WidthY, "Acceleration: ", ButtonColors[0]);
		GuiUtilsNW.GUIStereoButton (StartX, StartY + ButtonOffsetY, WidthX, WidthY, "Max Speed: ", ButtonColors[1]);
		GuiUtilsNW.GUIStereoButton (StartX, StartY + ButtonOffsetY * 2, WidthX, WidthY, "Sensitivity: ", ButtonColors[2]);
		GuiUtilsNW.GUIStereoButton (StartX, StartY + ButtonOffsetY * 3, WidthX, WidthY, "Brake: ", ButtonColors[3]);
		
		// Buttons appear next to currently selected button
		GuiUtilsNW.GUIStereoButton (StartX - 32, StartY + ButtonOffsetY * SelectedIndex, 30, 30, "Z", Color.green);
		GuiUtilsNW.GUIStereoButton (StartX + 202, StartY + ButtonOffsetY * SelectedIndex, 30, 30, "X", Color.green);
		
	}
	
		// Determines what each button does when pressed
	void KeyboardMenuSelection() {
		if(Input.GetKeyDown(KeyCode.Return)){
			if (SelectedIndex == 0){
				// Resume Game	
			}
			if(SelectedIndex == 1){
				// Quit game	
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
