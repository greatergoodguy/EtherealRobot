using UnityEngine;
using System.Collections;

public abstract class Input_BB : MonoBehaviour {
	public abstract bool GetButton_Accel();
	public abstract bool GetButton_Jump();
	public abstract bool GetButton_Debug();
	public abstract bool GetButton_Brake();
	
	public abstract bool GetButtonDown_Pause();
	public abstract bool GetButtonDown_CycleActiveEnvironment();
	public abstract bool GetButtonDown_CycleActivePlayer();
	
	public abstract float GetAxis_MouseX();
}
