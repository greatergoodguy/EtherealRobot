using UnityEngine;
using System.Collections;

public abstract class CameraController_BB : OVRComponent_BB {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public abstract void GetOrientationOffset(ref Quaternion orientationOffset);
	public abstract void SetOrientationOffset(Quaternion orientationOffset);
	public abstract void SetYRotation(float yRotation);
	public abstract void GetYRotation(ref float yRotation);	
}
