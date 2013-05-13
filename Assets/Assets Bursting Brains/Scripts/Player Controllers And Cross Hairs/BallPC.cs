/************************************************************************************

Filename    :   OVRPlayerController.cs
Content     :   Player controller interface. 
				This script drives OVR camera as well as controls the locomotion
				of the player, and handles physical contact in the world.	
Created     :   January 8, 2013
Authors     :   Peter Giokaris

Copyright   :   Copyright 2013 Oculus VR, Inc. All Rights reserved.

Use of this software is subject to the terms of the Oculus LLC license
agreement provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.

************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

//-------------------------------------------------------------------------------------
// ***** OVRPlayerController
//
// OVRPlayerController implements a basic first person controller for the Rift. It is 
// attached to the OVRPlayerController prefab, which has an OVRCameraController attached
// to it. 
// 
// The controller will interact properly with a Unity scene, provided that the scene has
// collision assigned to it. 
//
// The OVRPlayerController prefab has an empty GameObject attached to it called 
// ForwardDirection. This game object contains the matrix which motor control bases it
// direction on. This game object should also house the body geometry which will be seen
// by the player.
//
public class BallPC : OVRComponent {
	
	public Vector3 movement;
	public float moveSpeed = 6.0f;
	public float jumpSpeed = 2.0f;
	public float drag = 2;
	private bool canJump = true;
	
	protected OVRCameraController 	CameraController 	= null;

	public float RotationAmount  = 1.5f;
	private Quaternion OrientationOffset = Quaternion.identity;			// Initial direction of controller (passed down into CameraController)
	private float 	YRotation 	 = 0.0f;								// Rotation amount from inputs (passed down into CameraController)
	private float RotationScaleMultiplier = 1.0f; 						// We can adjust these to influence speed and rotation of player controller
	
	// Transfom used to point player in a given direction; 
	// We should attach objects to this if we want them to rotate 
	// separately from the head (i.e. the body)
	protected Transform DirXform = null;
	//
	// STATIC VARIABLES
	//
	public static bool  AllowMouseRotation      = true;
 	
	// * * * * * * * * * * * * *
	
	// Awake
	new public virtual void Awake()
	{
		base.Awake();
					
		// We use OVRCameraController to set rotations to cameras, 
		// and to be influenced by rotation
		OVRCameraController[] CameraControllers;
		CameraControllers = gameObject.GetComponentsInChildren<OVRCameraController>();
		
		if(CameraControllers.Length == 0)
			Debug.LogWarning("OVRPlayerController: No OVRCameraController attached.");
		else if (CameraControllers.Length > 1)
			Debug.LogWarning("OVRPlayerController: More then 1 OVRCameraController attached.");
		else
			CameraController = CameraControllers[0];	
	
		// Instantiate a Transform from the main game object (will be used to 
		// direct the motion of the PlayerController, as well as used to rotate
		// a visible body attached to the controller)
		DirXform = null;
		Transform[] Xforms = gameObject.GetComponentsInChildren<Transform>();
		
		for(int i = 0; i < Xforms.Length; i++)
		{
			if(Xforms[i].name == "ForwardDirection")
			{
				DirXform = Xforms[i];
				break;
			}
		}
		
		if(DirXform == null)
			Debug.LogWarning("OVRPlayerController: ForwardDirection game object not found. Do not use.");
	}

	// Start
	new public virtual void Start()
	{
		base.Start();
		
		InitializeInputs();	
		SetCameras();
	}
		
	// Update 
	new public virtual void Update() {
		base.Update();
		
		UpdateMovement();

		UpdatePlayerForwardDirTransform();
	}
		
	// UpdateMovement
	//
	// COnsolidate all movement code here
	//
	static float sDeltaRotationOld = 0.0f;
	public virtual void UpdateMovement() {
		
		// Controls the Ball's movement
		Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
		forward.y = 0;
		forward = forward.normalized;
		
		Vector3 forwardForce = new Vector3();
		if (Application.platform == RuntimePlatform.Android){
			float tmpSpeed = 0; //moveJoy.GetComponent<Joystick>().position.y;
			forwardForce = forward * tmpSpeed * 1f * moveSpeed;
		}
		else{
			//float verticalInput = Input.GetAxis("Vertical");
			//if(verticalInput)
				//forwardForce = forward * Input.GetAxis("Vertical") * moveSpeed;
			if(Input.GetKey(KeyCode.Space))
				forwardForce = forward * moveSpeed;
		}
		rigidbody.AddForce(forwardForce);
		
		Vector3 right= Camera.main.transform.TransformDirection(Vector3.right);
		right.y = 0;
		right = right.normalized;
		
		Vector3 rightForce = new Vector3();
		if (Application.platform == RuntimePlatform.Android){
			float tmpSpeed = 0; //moveJoy.GetComponent<Joystick>().position.x;
			rightForce = right * tmpSpeed * 0.8f * moveSpeed;
		}
		else{
			//rightForce = right * Input.GetAxis("Horizontal") * moveSpeed;
			rightForce = Vector3.zero;
		}		
		rigidbody.AddForce(rightForce);
				
		if (canJump && Input.GetKeyDown(KeyCode.LeftShift)){
			rigidbody.AddForce(Vector3.up * jumpSpeed * 100);
			canJump = false;
		}
		
		// Controls the Camera rotation
		float rotateInfluence = DeltaTime * RotationAmount * RotationScaleMultiplier;
		
		// Rotate
		float deltaRotation = 0.0f;
		if(AllowMouseRotation == false)
			deltaRotation = Input.GetAxis("Mouse X") * rotateInfluence * 3.25f;
			
		float filteredDeltaRotation = (sDeltaRotationOld * 0.0f) + (deltaRotation * 1.0f);
		YRotation += filteredDeltaRotation;
		sDeltaRotationOld = filteredDeltaRotation;
			
		// Rotate
		YRotation += OVRGamepadController.GetAxisRightX() * rotateInfluence;    
		
		// Update cameras direction and rotation
		SetCameras();
	}

	// UpdatePlayerControllerRotation
	// This function will be used to 'slide' PlayerController rotation around based on 
	// CameraController. For now, we are simply copying the CameraController rotation into 
	// PlayerController, so that the PlayerController always faces the direction of the 
	// CameraController. When we add a body, this will change a bit..
	public virtual void UpdatePlayerForwardDirTransform()
	{
		if ((DirXform != null) && (CameraController != null))
			DirXform.rotation = CameraController.transform.rotation;
	}
	
	///////////////////////////////////////////////////////////
	// PUBLIC FUNCTIONS
	///////////////////////////////////////////////////////////
	
	// InitializeInputs
	public void InitializeInputs()
	{
		// Get our start direction
		OrientationOffset = transform.rotation;
		// Make sure to set y rotation to 0 degrees
		YRotation = 0.0f;
	}
	
	// SetCameras
	public void SetCameras()
	{
		if(CameraController != null)
		{
			// Make sure to set the initial direction of the camera 
			// to match the game player direction
			CameraController.SetOrientationOffset(OrientationOffset);
			CameraController.SetYRotation(YRotation);
		}
	}
	
	void OnCollisionEnter(Collision collision){
		print ("OnCollisionEnter");
		if (!canJump){
			canJump = true;
		}
    }
}