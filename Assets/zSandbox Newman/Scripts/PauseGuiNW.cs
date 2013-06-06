using UnityEngine;
using System.Collections;

public class PauseGuiNW : MonoBehaviour {
	
	// Spacing for scenes menu
	private int    	StartX			= 300;
	private int    	StartY			= 170;
	private int    	WidthX			= 100;
	private int    	WidthY			= 50;
	private int 	ButtonOffsetY 	= 50;	// Distance each new button is created from previous
	
	private bool 	isGuiOn			= true;
	
	private int SelectedIndex = 0;	// Indicates what key is currently selected by Keyboard
	
	private ArrayList ButtonsList = new ArrayList();
	
	private ArrayList InputList = new ArrayList() {"Keyboard", "XBox"};
	
	// Use this for initialization
	void Start () {
		
		// Add Buttons here
		AddButton (StartX, StartY, WidthX, WidthY, "Resume", Color.white);
		AddButton (StartX, StartY + ButtonOffsetY, WidthX, WidthY, "Debug", Color.white);
		AddButton (StartX, StartY + ButtonOffsetY, WidthX, WidthY, "Current Input\n", Color.white);
		AddButton (StartX, StartY + ButtonOffsetY * 2, WidthX, WidthY, "Quit", Color.white);
	
		// Makes first button appear yellow by default
		((ButtonsNW)ButtonsList[0]).ButtonSelected();
	
	}
	
	// Update is called once per frame
	void Update () {
		KeyboardMenuSelection();
	}
	
	void OnGUI(){
		if(!isGuiOn){
			return;	
		}	
		for(int i = 0; i < ButtonsList.Count; i++)
			((ButtonsNW)(ButtonsList[i])).Display();	
	}
	
	// Determines what each button does when pressed
	void KeyboardMenuSelection() {
		if(Input.GetKeyDown(KeyCode.Return)){
			if (SelectedIndex == 0){
				// Resume Game	
			}
			if(SelectedIndex == 1){
				// Go to Debug Menu	
			}
			if(SelectedIndex == 3){
				// Quit game	
			}	
		}
		
		/*if(Input.GetKeyDown (KeyCode.LeftArrow && SelectedIndex == 2 )){
			
		}*/
		
		// Moves between buttons with arrows keys
		SelectedIndex = GuiUtilsNW.GUIKeyboardUpDown(SelectedIndex, ButtonsList);
	}

	void AddButton(int X, int Y, int wX, int wY, string text, Color color){
		ButtonsList.Add (new ButtonsNW(X, Y, wX, wY, text, color));
	}	
}
