using UnityEngine;
using System.Collections;

public class DebugGui : MenuGui {
	
	private static int    	startX_Header			= 100;
	private static int    	startY_Header			= 100;
	
	private static int    	startX					= 260;
	private static int    	startY					= 200;
	private static int    	width_label				= 200;
	private static int    	width_labelWithOffset	= width_label + 2;
	private static int    	height_label			= 30;
	private static int    	height_labelWithOffset	= height_label + 2;
	private static int		sideButtonOffsetX 		= 30;
	
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
		
		// Title
		_GuiUtilsOR.GUIStereoButton (startX_Header, startY_Header, width_label, height_label, "Ethereal", Color.yellow);
		if(InputManager.IsXboxInput()){
			_GuiUtilsOR.GUIStereoButton (startX_Header, startY_Header + height_labelWithOffset, 
				width_label, height_label, "Xbox", Color.cyan);	
		}
		else if(InputManager.IsKeyboardInput()){
			_GuiUtilsOR.GUIStereoButton (startX_Header, startY_Header + height_labelWithOffset, 
				width_label, height_label, "Keyboard", Color.cyan);	
		}
		_GuiUtilsOR.GUIStereoButton (startX_Header + width_labelWithOffset, startY_Header + height_labelWithOffset, 30, 30, "i", Color.green);
		
		// Need to change this for ease of use
		for(int i = 0; i < ButtonsList.Count; i++){
			float holder = debugData.GetValue(i);
			((Button) (ButtonsList[i])).DynamicDisplay(holder);
		}
		
		_GuiUtilsOR.GUIStereoButton (startX - 32, startY + height_labelWithOffset * SelectedIndex, 30, 30, "z", Color.green);
		_GuiUtilsOR.GUIStereoButton (startX + 202, startY + height_labelWithOffset * SelectedIndex, 30, 30, "x", Color.green);		
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
			AddButton (startX, startY + height_labelWithOffset * i, width_label, height_label, debugData.GetKey(i), Color.white);
		}
	}
}
