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
	
	static private int NumButtons = 4;
	private Color[] ButtonColors = new Color[NumButtons];
	
	private ArrayList ButtonsList = new ArrayList();
	
	private float accel = 0;
	private float brake = -200;
	private float maxSpeed = 2000;
	private float sens = 20;
	
	// Use this for initialization
	void Start () {
		
		// Add Buttons Here
		AddButton (StartX, StartY, WidthX, WidthY, "Acceleration: ", Color.white);
		AddButton (StartX, StartY + ButtonOffsetY, WidthX, WidthY, "Max Speed: ", Color.white);
		AddButton (StartX, StartY + ButtonOffsetY * 2, WidthX, WidthY, "Sensitivity: ", Color.white);
		AddButton (StartX, StartY + ButtonOffsetY * 3, WidthX, WidthY, "Brake: ", Color.white);
		
		AddButton (StartX - 10, StartY + ButtonOffsetY * 5, 100, (int)(WidthY * 1.5f), "Resume ", Color.magenta);
		AddButton (StartX + 110, StartY + ButtonOffsetY * 5, 100, (int)(WidthY * 1.5f), "Main Menu ", Color.magenta);	
		
		((ButtonsNW)ButtonsList[0]).ButtonSelected();
	}
	
	
	// Update is called once per frame
	void Update () {
		KeyboardMenuSelection();
		accel++;
		maxSpeed--;
		sens -= .001f;
		
		if(brake < 20)
			brake++;
		else
			brake = -20;
	}
	
	void OnGUI(){
		if(!isGuiOn){
			return;	
		}
		
		// Title
		GuiUtilsNW.GUIStereoButton (StartX, StartY - 60, WidthX, WidthY, "Name of controller here", Color.cyan);
		
		// Need to change this for ease of use
		for(int i = 0; i < ButtonsList.Count; i++){
			
			float holder;
			switch(i)
			{
			case 0:
				holder = accel;
				break;
			case 1:
				holder = maxSpeed;
				break;
			case 2:
				holder = sens;
				break;
			case 3:
				holder = brake;
				break;
			default:
				holder = 999999999999.999f; // Shouldn't get this
				break;
			}
			if (i >= 4){  // Change this, i = 5 or 6, means it is not a dynamic button.
				((ButtonsNW)(ButtonsList[i])).Display();
			}
			else{
				((ButtonsNW)(ButtonsList[i])).DynamicDisplay(holder);
			}
		}
		
		// Buttons appear next to currently selected button
		if(SelectedIndex < 4){
			GuiUtilsNW.GUIStereoButton (StartX - 32, StartY + ButtonOffsetY * SelectedIndex, 30, 30, "Z", Color.green);
			GuiUtilsNW.GUIStereoButton (StartX + 202, StartY + ButtonOffsetY * SelectedIndex, 30, 30, "X", Color.green);
		}	
		
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
		
		SelectedIndex = GuiUtilsNW.GUIKeyboardUpDown(SelectedIndex, ButtonsList);
	}
	
	void AddButton(int X, int Y, int wX, int wY, string text, Color color){
		ButtonsList.Add (new ButtonsNW(X, Y, wX, wY, text, color));
	}
		
	void AddBox(){
		
	}	

}
