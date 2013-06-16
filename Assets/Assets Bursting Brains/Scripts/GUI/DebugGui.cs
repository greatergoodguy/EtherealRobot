using UnityEngine;
using System.Collections;

public class DebugGui : MenuGui {
	
	private static int    	startX_Header			= 300;
	private static int    	startY_Header			= 200;
	
	private static int    	startX					= 300;
	private static int    	startY					= 330;
	private static int    	width_label				= 160;
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
	}
	
	
	// Update is called once per frame
	void Update () {
		SelectedIndex = GuiUtils.GUIKeyboardUpDown(SelectedIndex, ButtonsList);
		if(SelectedIndex == 0 && ButtonsList.Count > 2){
			((Button) ButtonsList[0]).ButtonDeselected();
			((Button) ButtonsList[1]).ButtonSelected();
			SelectedIndex = 1;
		}
		
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
		
		// Turn Debug On/Off
		_GuiUtilsOR.GUIStereoButton (startX_Header, startY_Header + height_labelWithOffset * 2, width_label, height_label, "Turn Debug On/Off", Color.cyan);	
		_GuiUtilsOR.GUIStereoButton (startX_Header + width_labelWithOffset, startY_Header + height_labelWithOffset * 2, 30, 30, "o", Color.green);
		
		// Toggle Environment
		_GuiUtilsOR.GUIStereoButton (startX_Header, startY_Header + height_labelWithOffset * 3, width_label, height_label, "Toggle Environment", Color.cyan);	
		_GuiUtilsOR.GUIStereoButton (startX_Header + width_labelWithOffset, startY_Header + height_labelWithOffset * 3, 30, 30, "v", Color.green);
		
		// Need to change this for ease of use
		for(int i = 0; i < ButtonsList.Count; i++){
			string holder = debugData.GetValue(i);
			((Button) (ButtonsList[i])).DynamicDisplay(holder);
		}
		
		_GuiUtilsOR.GUIStereoButton (startX - 32, startY + height_labelWithOffset * SelectedIndex, 30, 30, "z", Color.green);
		_GuiUtilsOR.GUIStereoButton (startX + width_labelWithOffset, startY + height_labelWithOffset * SelectedIndex, 30, 30, "x", Color.green);		
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
		
		// Special Case for first item
		AddButton (startX, startY, width_label, height_label, debugData.GetKey(0), Color.gray);
		for(int i=1; i<debugData.GetSize(); i++){
			AddButton (startX, startY + height_labelWithOffset * i, width_label, height_label, debugData.GetKey(i), Color.white);
		}
		
		if(ButtonsList.Count > 2)
			((Button) ButtonsList[1]).ButtonSelected();
	}
}
