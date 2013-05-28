using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
	
	private Input_BB keyboardInput;
	private Input_BB xboxInput;
	
	private Input_BB activeInput;
	
	void Awake() {
		keyboardInput = GetComponentInChildren<KeyboardInput>();
		xboxInput = GetComponentInChildren<XboxInput>();
		
		activeInput = keyboardInput;
		
		DebugUtils.Assert(keyboardInput != null);
		DebugUtils.Assert(xboxInput != null);
	}
	
	void Start() {
	}

	public Input_BB GetActiveInput() {
		return activeInput;	
	}
}
