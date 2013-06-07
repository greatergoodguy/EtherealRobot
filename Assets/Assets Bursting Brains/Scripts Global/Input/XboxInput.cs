using UnityEngine;
using System.Collections;


public class XboxInput : Input_BB {
	
	bool isDown = true;
	
	public override bool GetButton_Accel() {		return (Input.GetAxis("R_Trigger") > 0.25);}  
	public override bool GetButton_Debug() {		return true;}  
	public override bool GetButton_Brake() {		return Input.GetKey(KeyCode.JoystickButton1);}  
	
	public override bool GetButton_Forward() {		return Input.GetKey(KeyCode.W);}  
	public override bool GetButton_Backward() {		return Input.GetKey(KeyCode.S);}  
	public override bool GetButton_Left() {			return Input.GetKey(KeyCode.A);}  
	public override bool GetButton_Right() {		return Input.GetKey(KeyCode.D);}  
	
	public override bool GetButton_RotateBodyLeft(){	return Input.GetKey(KeyCode.Q);}  
	public override bool GetButton_RotateBodyRight() {	return Input.GetKey(KeyCode.E);}
	
	public override bool GetButton_DebugIncreaseAttribute(){	return Input.GetKey(KeyCode.JoystickButton5);}  
	public override bool GetButton_DebugDecreaseAttribute() {	return Input.GetKey(KeyCode.JoystickButton4);} 
	
	// public override bool GetButtonDown_MenuDown() {		return Input.GetKeyDown(KeyCode.JoystickButton3);}  
	// public override bool GetButtonDown_MenuUp() {		return Input.GetKeyDown(KeyCode.JoystickButton0);}
	//public override bool GetButtonDown_MenuDown() {		return (Input.GetAxis("Menu Axis") == -1);}
	// public override bool GetButtonDown_MenuUp() {		return (Input.GetAxis("Menu Axis") == 1);}
	
	//_InputManagerHelper.LeftStick();
	
	public override bool GetButtonDown_MenuDown() {
		if(!_InputManagerHelper.IsReadyToBeDown() && (Input.GetAxis("Menu Axis") == -1)){
			_InputManagerHelper.SetIsReady(false);
			return true;
		}
		else{return false;}
	}
	
	public override bool GetButtonDown_MenuUp() {
		if(!_InputManagerHelper.IsReadyToBeDown() && (Input.GetAxis("Menu Axis") == 1)){
			_InputManagerHelper.SetIsReady(false);
			return true;
		}
		else{return false;}
	}
		
	public override bool GetButtonDown_SelectMenuItem() {return Input.GetKeyDown(KeyCode.JoystickButton0);}
	public override bool GetButtonDown_Jump() {		return (Input.GetAxis("L_Trigger") > 0.5);}   
		// I do not have a controller on me so I'm not sure if this works. - Kris
	public override bool GetButtonDown_SwitchCameraMode() {			return Input.GetKeyDown(KeyCode.JoystickButton9);}
	public override bool GetButtonDown_ToggleInputControls() { return Input.GetKeyDown(KeyCode.I);}
	
	public override bool GetButtonDown_Pause() {					return Input.GetKeyDown(KeyCode.JoystickButton7);}
	public override bool GetButtonDown_Debug() {					return Input.GetKeyDown(KeyCode.JoystickButton6);}
	public override bool GetButtonDown_CycleActiveEnvironment() {	return Input.GetKeyDown(KeyCode.JoystickButton8);} 
	public override bool GetButtonDown_CycleActivePlayer() {		return Input.GetKeyDown(KeyCode.C);}
	public override float GetAxis_MouseX() {						return Input.GetAxis("Xbox_Horizontal");}
	
	public override bool DummyButton() { return false;}  
}
