using UnityEngine;
using System.Collections;

public class StartMenuGui : MenuGui {
	
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
	
	private ArrayList InputList = new ArrayList() {};
	
	// Use this for initialization
	void Start () {
		
		
		//DebugUtils.Assert(startTexture != null);
		
		// Add Buttons here
		AddButton (StartX, StartY, WidthX, WidthY, "Start", Color.white);
		AddButton (StartX, StartY + ButtonOffsetY, WidthX, WidthY, "Exit", Color.white);
	
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
		
		// Renders GUI textures

		//_GuiUtilsStandard.GUIStereoTexture(200, 150, 500, 500, startTexture);
		_GuiUtilsOR.GUIStereoTexture(200, 150, 500, 500, startTexture);
		
		// Renders Buttons
		for(int i = 0; i < ButtonsList.Count; i++){
			((Button) (ButtonsList[i])).Display();	
		}
		
	}
	
	// Determines what each button does when pressed
	void KeyboardMenuSelection() {
		if(InputManager.activeInput.GetButtonDown_SelectMenuItem()){
			if (SelectedIndex == 0){
				// Start game
				ExitGui();
			}
			if(SelectedIndex == 1){
				// Exit game
				Application.Quit();
			}	
		}
			
		// Change this
		SelectedIndex = _GuiUtilsOR.GUIKeyboardUpDown(SelectedIndex, ButtonsList);
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
