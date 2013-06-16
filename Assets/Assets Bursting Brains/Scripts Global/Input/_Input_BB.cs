using UnityEngine;
using System.Collections;

public abstract class Input_BB {
	
	public abstract bool GetButton_Accel();
	public abstract bool GetButton_Debug();
	public abstract bool GetButton_Brake();
	public abstract bool GetButton_Look();
	
	public abstract bool GetButton_Forward();
	public abstract bool GetButton_Backward();
	public abstract bool GetButton_Left();
	public abstract bool GetButton_Right();
	
	public abstract bool GetButton_RotateBodyLeft();
	public abstract bool GetButton_RotateBodyRight();
	
	public abstract bool GetButton_DebugAccelerator();
	public abstract bool GetButton_DebugIncreaseAttribute();
	public abstract bool GetButton_DebugDecreaseAttribute();
	
	public abstract bool GetButtonDown_MenuUp();
	public abstract bool GetButtonDown_MenuDown();
	public abstract bool GetButtonDown_SelectMenuItem();
	public abstract bool GetButtonDown_Jump();
	public abstract bool GetButtonDown_SwitchCameraMode();
	public abstract bool GetButtonDown_ToggleInputControls();	
	
	public abstract bool GetButtonDown_Pause();
	public abstract bool GetButtonDown_Debug();
	public abstract bool GetButtonDown_CycleActiveEnvironment();
	public abstract bool GetButtonDown_CycleActivePlayer();
	
	public abstract float GetAxis_MouseX();
	public abstract float GetAxis_MouseY();
	
	public abstract bool DummyButton();
}
