using UnityEngine;
using System.Collections;

public class PauseGui : MonoBehaviour {
	
	// Spacing for scenes menu
	private int    	StartX			= 300;
	private int    	StartY			= 170;
	private int    	WidthX			= 100;
	private int    	WidthY			= 50;
	private int 	ButtonOffsetY 	= 50;	// Distance each new button is created from previous
	
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
	
	}
	
	// Update is called once per frame
	void Update () {
		KeyboardMenuSelection();
	}
	
	void OnGUI(){
		if(!isGuiOn){
			return;	
		}
		
		// Creates Buttons
		// When adding new buttons, make sure to increase 'NumButtons' variable at top
		GuiUtils.GUIStereoButton (StartX, StartY, WidthX, WidthY, "Resume", ButtonColors[0]);
		GuiUtils.GUIStereoButton (StartX, StartY + ButtonOffsetY, WidthX, WidthY, "Quit", ButtonColors[1]);
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
