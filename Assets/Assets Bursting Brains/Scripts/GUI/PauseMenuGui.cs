using UnityEngine;
using System.Collections;

public class PauseMenuGui : MenuGui {
	
	// Spacing for scenes menu
	private int    	StartX			= 300;
	private int    	StartY			= 170;
	private int    	WidthX			= 100;
	private int    	WidthY			= 50;
	private int 	ButtonOffsetY 	= 50;	// Distance each new button is created from previous
	
	private bool 	isGuiOn			= true;
	
	private int SelectedIndex = 0;	// Indicates what key is currently selected by Keyboard
	
	private ArrayList ButtonsList = new ArrayList();
	
	// Use this for initialization
	void Start () {
		
		// Add Buttons here
		AddButton (StartX, StartY, WidthX, WidthY, "Resume", Color.white);
		AddButton (StartX, StartY + ButtonOffsetY, WidthX, WidthY, "Debug", Color.white);
		AddButton (StartX, StartY + ButtonOffsetY * 2, WidthX, WidthY, "Quit", Color.white);
	
		// Makes first button appear yellow by default
		((Button) ButtonsList[0]).ButtonSelected();
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
			((Button) (ButtonsList[i])).Display();	
	}
	
	// Determines what each button does when pressed
	void KeyboardMenuSelection() {
		if(InputManager.activeInput.GetButtonDown_SelectMenuItem()){
			if (SelectedIndex == 0){
				// Resume Game	
				ExitGui();
			}
			if(SelectedIndex == 1){
				// Go to Debug Menu	
			}
			if(SelectedIndex == 2){
				// Quit game	
				Application.Quit();
			}	
		}
		
		// Moves between buttons with arrows keys
		SelectedIndex = GuiUtils.GUIKeyboardUpDown(SelectedIndex, ButtonsList);
	}

	void AddButton(int X, int Y, int wX, int wY, string text, Color color){
		ButtonsList.Add (new Button(X, Y, wX, wY, text, color));
	}	
	
	public override bool IsGuiOn(){
		return isGuiOn;
	}
	
	public override void EnterGui(){
		Time.timeScale = 0;
		isGuiOn = true;	
		enabled = true;
	}
	
	public override void ExitGui(){
		Time.timeScale = 1;
		isGuiOn = false;
		enabled = false;
	}
}
