using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
	
	private Input_BB keyboardInput;
	private Input_BB xboxInput;
	
	public Input_BB activeInput;
	
	void Awake() {
		keyboardInput = GetComponentInChildren<KeyboardInput>();
		xboxInput = GetComponentInChildren<XboxInput>();
		
		activeInput = keyboardInput;
		
		DebugUtils.Assert(keyboardInput != null);
		DebugUtils.Assert(xboxInput != null);
		DebugUtils.Assert(activeInput != null);
	}
	
	void Start() {
	}
}
