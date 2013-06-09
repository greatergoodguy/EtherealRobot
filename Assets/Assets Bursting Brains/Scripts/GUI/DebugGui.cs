using UnityEngine;
using System.Collections;

public class DebugGui : MenuGui {
	
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
	
	private DebugData debugData;
	
	// Use this for initialization
	void Start () {
		
		// Add Buttons Here
		//AddButton (StartX, StartY, WidthX, WidthY, "Acceleration: ", Color.white);
		//AddButton (StartX, StartY + ButtonOffsetY, WidthX, WidthY, "Max Speed: ", Color.white);
		//AddButton (StartX, StartY + ButtonOffsetY * 2, WidthX, WidthY, "Sensitivity: ", Color.white);
		//AddButton (StartX, StartY + ButtonOffsetY * 3, WidthX, WidthY, "Brake: ", Color.white);
		
		//AddButton (StartX - 10, StartY + ButtonOffsetY * 5, 100, (int)(WidthY * 1.5f), "Resume ", Color.magenta);
		//AddButton (StartX + 110, StartY + ButtonOffsetY * 5, 100, (int)(WidthY * 1.5f), "Main Menu ", Color.magenta);	
		
		//((Button) ButtonsList[0]).ButtonSelected();
	}
	
	
	// Update is called once per frame
	void Update () {
		SelectedIndex = GuiUtils.GUIKeyboardUpDown(SelectedIndex, ButtonsList);
		
		if(InputManager.activeInput.GetButton_DebugDecreaseAttribute()){
			debugData.DebugDecrease(SelectedIndex);
		}
		if(InputManager.activeInput.GetButton_DebugIncreaseAttribute()){
			debugData.DebugIncrease(SelectedIndex);
		}
	}
	
	void OnGUI(){
		if(!isGuiOn){
			return;	
		}
		
		
		if(InputManager.IsXboxInput()){
			_GuiUtilsOR.GUIStereoButton (StartX, StartY - 60, WidthX, WidthY, "Xbox", Color.cyan);	
		}
		else if(InputManager.IsKeyboardInput()){
			_GuiUtilsOR.GUIStereoButton (StartX, StartY - 60, WidthX, WidthY, "Keyboard", Color.cyan);	
		}
		
		// Title
		_GuiUtilsOR.GUIStereoButton (StartX, StartY - 100, WidthX, WidthY, "Ethereal", Color.cyan);
		
		// Need to change this for ease of use
		for(int i = 0; i < ButtonsList.Count; i++){
			float holder = debugData.GetValue(i);
			((Button) (ButtonsList[i])).DynamicDisplay(holder);
		}
		
		_GuiUtilsOR.GUIStereoButton (StartX - 32, StartY + ButtonOffsetY * SelectedIndex, 30, 30, "Z", Color.green);
		_GuiUtilsOR.GUIStereoButton (StartX + 202, StartY + ButtonOffsetY * SelectedIndex, 30, 30, "X", Color.green);		
	}
	
	void AddButton(int X, int Y, int wX, int wY, string text, Color color){
		ButtonsList.Add (new Button(X, Y, wX, wY, text, color));
	}
		
	public override bool IsGuiOn(){
		return isGuiOn;
	}
	
	public override void EnterGui(){
		isGuiOn = true;
		enabled = true;
	}
	
	public override void ExitGui(){
		isGuiOn = false;
		enabled = false;
	}
	
	public void SetDebugData(DebugData debugData){
		this.debugData = debugData;
		ButtonsList.Clear();
		
		for(int i=0; i<debugData.GetSize(); i++){
			AddButton (StartX, StartY + ButtonOffsetY * i, WidthX, WidthY, debugData.GetKey(i), Color.white);
		}
	}
}
