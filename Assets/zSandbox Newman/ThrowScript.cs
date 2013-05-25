using UnityEngine;
using System.Collections;

public class ThrowScript : MonoBehaviour {
	
	// Used for keyboard control. Each index indicates a different menu button.
	// 0 = Start
	// 1 = Exit
	int selectedIndex = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
		//KeyboardInput();		
		KeyboardMenuSelections();
	}
	
	void KeyboardMenuSelections(){
		if(Input.GetKeyDown(KeyCode.DownArrow) && selectedIndex < 1){
			selectedIndex++;	
			//GUI.Box( new Rect (10, 100, 50, 30), "Start Halo");
		}
		if(Input.GetKeyDown(KeyCode.UpArrow) && selectedIndex > 0){
			selectedIndex--;	
		}
		if(Input.GetKeyDown(KeyCode.Return)){
			if(selectedIndex == 0){
				Application.LoadLevel (0);	
			}
		}
		
	}
	
	void OnGUI () { 
		// Menu Box
		GUI.Box( new Rect (5, 5, 60, 200), "Menu"); 
		// Start
		if(selectedIndex == 0){
			GUI.Box( new Rect (8, 28, 54, 34), "");
		}
		if(selectedIndex == 1){
			GUI.Box( new Rect (8, 68, 54, 34), "");
		}
		
		
        if (GUI.Button (new Rect (10,30,50,30), "Start")){
            print ("Start the Game");
			Application.LoadLevel(0);
		}
		// Exit
		if (GUI.Button (new Rect (10,70,50,30), "Exit")){
            print ("Exit the Game");
		
		}
		// GUI.Box(new Rect, "Content")
    }
	
	/*private void KeyboardInput(){
		
	}*/	
}
