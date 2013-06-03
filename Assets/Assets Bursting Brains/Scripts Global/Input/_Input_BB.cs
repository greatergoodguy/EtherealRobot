using UnityEngine;
using System.Collections;

public abstract class Input_BB {
	public abstract bool GetButton_Accel();
	public abstract bool GetButton_Debug();
	public abstract bool GetButton_Brake();
	
	public abstract bool GetButton_Forward();
	public abstract bool GetButton_Backward();
	public abstract bool GetButton_Left();
	public abstract bool GetButton_Right();
	
	public abstract bool GetButton_RotateBodyLeft();
	public abstract bool GetButton_RotateBodyRight();
	
	public abstract bool GetButtonDown_Jump();
	
	public abstract bool GetButtonDown_Pause();
	public abstract bool GetButtonDown_SwitchCameraMode();
	public abstract bool GetButtonDown_CycleActiveEnvironment();
	public abstract bool GetButtonDown_CycleActivePlayer();
	
	public abstract float GetAxis_MouseX();
	
	public abstract bool DummyButton();
}
