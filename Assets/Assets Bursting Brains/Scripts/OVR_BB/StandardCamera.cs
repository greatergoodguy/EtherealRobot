/************************************************************************************

Filename    :   OVRCamera.cs
Content     :   Interface to camera class
Created     :   January 8, 2013
Authors     :   Peter Giokaris

Copyright   :   Copyright 2013 Oculus VR, Inc. All Rights reserved.

Use of this software is subject to the terms of the Oculus LLC license
agreement provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.

************************************************************************************/

// #define MSAA_ENABLED // Not available in Unity 4 as of yet

using UnityEngine;
using System.Runtime.InteropServices;

[RequireComponent(typeof(Camera))]

//-------------------------------------------------------------------------------------
// ***** OVRCamera
//
// OVRCamera is used to render into a Unity Camera class. 
// This component handles reading the Rift tracker and positioning the camera position
// and rotation. It also is responsible for properly rendering the final output, which
// also the final lens correction pass.
//
public class StandardCamera : OVRComponent_BB{		

	private StandardCameraController CameraController = null;
	
	// Start
	new void Start()
	{
		base.Start ();		
		
		// Get the OVRCameraController
		CameraController = transform.GetComponent<StandardCameraController>();
		
		if(CameraController == null)
			Debug.LogWarning("WARNING: OVRCameraController not found!");
	}
	
		// OnPreRender
	void OnPreRender(){
		SetCameraOrientation();
	}
	
	// Update
	new void Update(){
		base.Update ();
	}
	
	// SetCameraOrientation
	void SetCameraOrientation() {
		Quaternion q   = Quaternion.identity;
		Vector3    dir = Vector3.forward;		
		
		// Calculate the rotation Y offset that is getting updated externally
		// (i.e. like a controller rotation)
		float yRotation = 0.0f;
		CameraController.GetYRotation(ref yRotation);
		q = Quaternion.Euler(0.0f, yRotation, 0.0f);
		dir = q * Vector3.forward;
		q.SetLookRotation(dir, Vector3.up);
	
		// Multiply the camera controllers offset orientation (allow follow of orientation offset)
		Quaternion orientationOffset = Quaternion.identity;
		CameraController.GetOrientationOffset(ref orientationOffset);
		q = orientationOffset * q;
		
	
		// * * *
		// Update camera rotation
		gameObject.camera.transform.rotation = q;
		
		// * * *
		// Update camera position (first add Offset to parent transform)
		gameObject.camera.transform.position = gameObject.camera.transform.parent.transform.position;		
	}
	
}
