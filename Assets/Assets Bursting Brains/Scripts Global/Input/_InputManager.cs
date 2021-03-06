using UnityEngine;
using System.Collections;

public static class InputManager {
	
	/* CLASS VARS */
	private static Input_BB keyboardInput = new KeyboardInput();
	private static Input_BB xboxInput = new XboxInput();
	//public static Input_BB activeInput = xboxInput;
	public static Input_BB activeInput = keyboardInput;

	
	/* CLASS METHODS */
	public static bool IsXboxInput() { return activeInput == xboxInput; }
	
	public static bool IsKeyboardInput() { return activeInput == keyboardInput; }
	
	public static void SwitchToXboxInput() { activeInput = xboxInput; }
	
	public static void SwitchToKeyboardInput() { activeInput = keyboardInput; }
	
	public static void ToggleInput() {
		if (activeInput == keyboardInput) activeInput = xboxInput;
		else activeInput = keyboardInput;
	}
}
