using UnityEngine;
using System.Collections;

public class StandardCameraController : CameraController_BB {
	
	private Quaternion OrientationOffset = Quaternion.identity;	
	private float YRotation = 0.0f;
	
	// Update 
	new void LateUpdate(){
		base.Update();		
		UpdateCameras();
	}
	
	// InitCameras
	void UpdateCameras(){
	}
	
	public override void GetOrientationOffset(ref Quaternion orientationOffset){
		orientationOffset = OrientationOffset;
	}
	
	public override void SetOrientationOffset(Quaternion orientationOffset){
		OrientationOffset = orientationOffset;
	}
	
	public override void GetYRotation(ref float yRotation){
		yRotation = YRotation;
	}
	
	public override void SetYRotation(float yRotation){
		YRotation = yRotation;
	}
}
