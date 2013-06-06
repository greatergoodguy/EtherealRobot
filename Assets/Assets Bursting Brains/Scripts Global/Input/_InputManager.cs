using UnityEngine;
using System.Collections;

public static class InputManager {
	
	private static Input_BB keyboardInput = new KeyboardInput();
	private static Input_BB xboxInput = new XboxInput();
	
	public static Input_BB activeInput = xboxInput;
	
	public static void SwitchToXboxInput(){
		activeInput = xboxInput;
	}
	
	public static void SwitchToKeyboardInput(){
		activeInput = xboxInput;
	}
	
	public static void ToggleInput(){
		if(activeInput == keyboardInput)
			activeInput = xboxInput;
		else 
			activeInput = keyboardInput;
	}
}
