using UnityEngine;
using System.Collections;

public class KeyboardInput : Input_BB {
	public override bool GetButton_Accel() {		return Input.GetKey(KeyCode.Space);}  
	public override bool GetButton_Debug() {		return true;}  
	public override bool GetButton_Brake() {		return Input.GetKey(KeyCode.LeftControl);}  
	
	public override bool GetButton_Forward() {		return Input.GetKey(KeyCode.W);}  
	public override bool GetButton_Backward() {		return Input.GetKey(KeyCode.S);}  
	public override bool GetButton_Left() {			return Input.GetKey(KeyCode.A);}  
	public override bool GetButton_Right() {		return Input.GetKey(KeyCode.D);}  
	
	public override bool GetButton_RotateBodyLeft(){	return Input.GetKey(KeyCode.Q);}  
	public override bool GetButton_RotateBodyRight() {	return Input.GetKey(KeyCode.E);}  
	
	public override bool GetButtonDown_Jump() {		return Input.GetKeyDown(KeyCode.LeftShift);}  
	
	public override bool GetButtonDown_Pause() {					return Input.GetKeyDown(KeyCode.P);}
	public override bool GetButtonDown_SwitchCameraMode() {			return Input.GetKeyDown(KeyCode.B);}
	public override bool GetButtonDown_CycleActiveEnvironment() {	return Input.GetKeyDown(KeyCode.V);}
	public override bool GetButtonDown_CycleActivePlayer() {		return Input.GetKeyDown(KeyCode.C);}
	public override float GetAxis_MouseX() {						return Input.GetAxis("Mouse X");}
	
	public override bool DummyButton() { return false;}  
}
