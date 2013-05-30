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
public class StandardCamera_Ethereal : OVRComponent_BB{		

	private StandardCameraController CameraController = null;
	
	// Start
	new void Start() {
		base.Start ();		
	}
	
		// OnPreRender
	void OnPreRender(){
	}
	
	// Update
	new void Update(){
		base.Update ();
	}
	
	// SetCameraOrientation
	void SetCameraOrientation() {
	}
	
}
