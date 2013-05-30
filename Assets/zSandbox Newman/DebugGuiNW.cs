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
	
	private ArrayList ButtonsList = new ArrayList();
	
	private float accel = 0;
	private float brake = 0;
	private int maxSpeed = 98898;
	private float sens = 20;
	
	// Use this for initialization
	void Start () {
		// Initializes GUI buttons to white, except for the selectedIndex
		//ButtonColors[SelectedIndex] = Color.yellow;
		//for(int i = 1; i < ButtonColors.Length; i++)
		//	ButtonColors[i] = Color.white;
		      
		//ButtonsNW butt1 = new ButtonsNW(StartX, StartY, WidthX, WidthY, "Acceleration: ", Color.yellow);
		//ButtonsList.Add(butt1);
		//butt1.Display();
		
		ButtonsList.Add(new ButtonsNW(StartX, StartY, WidthX, WidthY, "Acceleration: " + accel, Color.yellow));
		ButtonsList.Add(new ButtonsNW(StartX, StartY + ButtonOffsetY, WidthX, WidthY, "Max Speed: " + maxSpeed, Color.white));
		ButtonsList.Add(new ButtonsNW(StartX, StartY + ButtonOffsetY * 2, WidthX, WidthY, "Sensitivity: " + sens, Color.white));		
		ButtonsList.Add(new ButtonsNW(StartX, StartY + ButtonOffsetY * 3, WidthX, WidthY, "Brake: " + brake, Color.white));
		
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
		
		// Creates Boxes
		// When adding new buttons, make sure to increase 'NumButtons' variable at top
		/*GuiUtilsNW.GUIStereoButton (StartX, StartY, WidthX, WidthY, "Acceleration: ", ButtonColors[0]);
		GuiUtilsNW.GUIStereoButton (StartX, StartY + ButtonOffsetY, WidthX, WidthY, "Max Speed: ", ButtonColors[1]);
		GuiUtilsNW.GUIStereoButton (StartX, StartY + ButtonOffsetY * 2, WidthX, WidthY, "Sensitivity: ", ButtonColors[2]);
		GuiUtilsNW.GUIStereoButton (StartX, StartY + ButtonOffsetY * 3, WidthX, WidthY, "Brake: ", ButtonColors[3]);
		*/
		

		for(int i = 0; i < ButtonsList.Count; i++){
			if(i == 0)
				((ButtonsNW)(ButtonsList[i])).UpdateText(accel);
			//else if (i = 1)
			
			((ButtonsNW)(ButtonsList[i])).Display();
			
		}
		
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
		if(Input.GetKeyDown(KeyCode.DownArrow) && SelectedIndex < ButtonsList.Count - 1){
			Color col = Color.white;
			((ButtonsNW)ButtonsList[SelectedIndex]).ChangeColor(col);
			SelectedIndex++;
			// ChangeButtonColor(SelectedIndex - 1);
			((ButtonsNW)ButtonsList[SelectedIndex]).ChangeColor(Color.yellow);
		}
		if(Input.GetKeyDown(KeyCode.UpArrow) && SelectedIndex > 0){
			((ButtonsNW)ButtonsList[SelectedIndex]).ChangeColor(Color.white);
			SelectedIndex--;
			((ButtonsNW)ButtonsList[SelectedIndex]).ChangeColor(Color.yellow);
			// ChangeButtonColor(SelectedIndex + 1);
		}
	}
	
	// Changes the color of the GUI button if it is currently selected by the keyboard
	/*void ChangeButtonColor(int prevIndex){
		ButtonColors[prevIndex] = Color.white;
		ButtonColors[SelectedIndex] = Color.yellow;
	}*/

}
