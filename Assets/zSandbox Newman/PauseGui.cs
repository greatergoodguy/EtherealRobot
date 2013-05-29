using UnityEngine;
using System.Collections;

public class PauseGui : MonoBehaviour {
	
	// Spacing for scenes menu
	private int    	StartX			= 300;
	private int    	StartY			= 170;
	private int    	WidthX			= 50;
	private int    	WidthY			= 30;
	private int 	ButtonOffsetY 	= 30;	// Distance each new button is created from previous

	private int SelectedIndex = 1;	// Indicates what key is currently selected by Keyboard
	//private int NumButtons = 0;		// Total # of buttons. Gets updated in 'AddButton' inefficiently	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	/*
	void OnGUI(){
		if(!isGuiOn){
			return;	
		}
		Rect buttonRect = new Rect ();
		GUIStereoBox()
		
		GUIStereoBox (StartX, StartY, WidthX, WidthY, ref start_s, Color.yellow);
	}
	*/
}
