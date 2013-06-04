using UnityEngine;
using System.Collections;

public abstract class CameraController_BB : OVRComponent_BB {	
	// Use this to turn on/off Prediction
	public bool			PredictionOn 	= true;
	
	public abstract void GetOrientationOffset(ref Quaternion orientationOffset);
	public abstract void SetOrientationOffset(Quaternion orientationOffset);
	public abstract void SetYRotation(float yRotation);
	public abstract void GetYRotation(ref float yRotation);	
	public abstract void SetSharedOrientation(Quaternion camRotation);
}
