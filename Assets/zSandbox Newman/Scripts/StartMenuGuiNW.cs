using UnityEngine;
using System.Collections;

public class StartMenuGuiNW : MonoBehaviour {
	
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
	
	private ArrayList ButtonsList = new ArrayList();
	
	// Use this for initialization
	void Start () {
		
		DebugUtils.Assert(startTexture != null);
		
		AddButton (StartX, StartY, WidthX, WidthY, "Start", Color.white);
		AddButton (StartX, StartY + ButtonOffsetY, WidthX, WidthY, "Exit", Color.white);
	
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
		
		// Renders GUI textures
		GuiUtilsNW.GUIStereoTexture(200, 150, 500, 500, startTexture);		
		
		// Renders Buttons
		for(int i = 0; i < ButtonsList.Count; i++){
			((ButtonsNW)(ButtonsList[i])).Display();	
		}
		
	}
	
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
		if(Input.GetKeyDown(KeyCode.DownArrow) && SelectedIndex < ButtonsList.Count - 1){
			((ButtonsNW)ButtonsList[SelectedIndex]).ButtonDeselected();
			SelectedIndex++;
			((ButtonsNW)ButtonsList[SelectedIndex]).ButtonSelected();
		}
		if(Input.GetKeyDown(KeyCode.UpArrow) && SelectedIndex > 0){
			((ButtonsNW)ButtonsList[SelectedIndex]).ButtonDeselected();
			SelectedIndex--;
			((ButtonsNW)ButtonsList[SelectedIndex]).ButtonSelected();
		}
	}
	
	void AddButton(int X, int Y, int wX, int wY, string text, Color color){
		ButtonsList.Add (new ButtonsNW(X, Y, wX, wY, text, color));
	}	
}
