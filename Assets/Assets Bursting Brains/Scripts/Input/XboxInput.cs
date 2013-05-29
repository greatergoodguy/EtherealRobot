using UnityEngine;
using System.Collections;

public class XboxInput : Input_BB {
	public override bool GetButton_Accel() {
    	return Input.GetKey(KeyCode.Space);
   	}  
	
	
	public override bool GetButton_Jump() {
    	return true;
   	}  
	
	
	public override bool GetButton_Debug() {
    	return true;
   	}  
	
	public override bool GetButton_Brake() {
    	return Input.GetKey(KeyCode.LeftControl);
   	}  
	
	public override bool GetButtonDown_Pause() {
    	return Input.GetKey(KeyCode.P);
   	}
	
	public override bool GetButtonDown_CycleActiveEnvironment() {
    	return Input.GetKeyDown(KeyCode.V);
   	} 
	
	public override bool GetButtonDown_CycleActivePlayer() {
    	return Input.GetKeyDown(KeyCode.B);
   	}
	
	public override float GetAxis_MouseX() {
		return Input.GetAxis("Mouse X");
	}
}
