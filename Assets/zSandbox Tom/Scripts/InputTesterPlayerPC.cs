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
public class InputTesterPlayerPC : PlayerController {
	// These variables are for adjusting in the inspector how the object behaves 
	public float maxSpeed  = 7;
	public float moveSpeed = 6.0f;
	public float force     = 8;
	public float jumpSpeed = 5;
 
	// These variables are there for use by the script and don't need to be edited
	private int state = 0;
	private bool grounded = false;
	private float jumpLimit = 0;
	
	protected OVRCameraController 	CameraController 	= null;

	public float RotationAmount  = 1.5f;
	private Quaternion OrientationOffset = Quaternion.identity;			// Initial direction of controller (passed down into CameraController)
	private float 	YRotation 	 = 0.0f;								// Rotation amount from inputs (passed down into CameraController)
	private float RotationScaleMultiplier = 1.0f; 						// We can adjust these to influence speed and rotation of player controller
	
	private Queue<Vector3> forwardDirHistory = new Queue<Vector3>();
	public int driftFactor = 15;
	
	// Transfom used to point player in a given direction; 
	// We should attach objects to this if we want them to rotate 
	// separately from the head (i.e. the body)
	protected Transform DirXform = null;
	//
	// STATIC VARIABLES
	//
	public static bool  AllowMouseRotation      = true;
	
	private Input_BB input = null;
 	
	// * * * * * * * * * * * * *
	
	// Awake
	new public virtual void Awake()
	{
		base.Awake();
		
		// Don't let the Physics Engine rotate this physics object so it doesn't fall over when running
		rigidbody.freezeRotation = true;
		
		collider.material.dynamicFriction = .2f;
		collider.material.dynamicFriction2 = .2f;
		collider.material.staticFriction = .2f;
		collider.material.staticFriction2 = .2f;
		collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
					
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
		
		Vector3 currForward = Camera.main.transform.TransformDirection(Vector3.forward);		
		currForward.y = 0;
		currForward = currForward.normalized;
		for(int i=0; i<driftFactor; i++){
			forwardDirHistory.Enqueue(currForward);
		}
		
		getVariance();
	}

	// Start
	new public virtual void Start(){
		base.Start();
		
		InitializeInputs();	
		SetCameras();
		
		input = GetComponent<Input_BB>();
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
	private Vector3 currForward;
	private Vector3 driftForward;
	private Vector3 forward;
	
	static float sDeltaRotationOld = 0.0f;
	public virtual void UpdateMovement() {	
		
		// Controls the Ball's movement
		currForward = Camera.main.transform.TransformDirection(Vector3.forward);		
		currForward.y = 0;
		currForward = currForward.normalized;
		forwardDirHistory.Enqueue(currForward);
		
		driftForward = forwardDirHistory.Dequeue();
		forward = driftForward;
		
		Vector3 forwardForce = new Vector3();
		if(input.GetButton_Accel()){
			if (getVariance().magnitude < .5f && getVariance().magnitude > .2f){
				forwardForce = -3f * (forward * moveSpeed);				
			}
			else if (getVariance().magnitude > .5f){
				forwardForce = -6f * (forward * moveSpeed);
			}
			else{
				forwardForce = forward * moveSpeed;
			}
		}
		rigidbody.AddForce(forwardForce);
		
		// Controls the Camera rotation
		float rotateInfluence = DeltaTime * RotationAmount * RotationScaleMultiplier;
		
		// Rotate
		float deltaRotation = 0.0f;
		if(AllowMouseRotation == false)
			deltaRotation = input.GetAxis_MouseX() * rotateInfluence * 3.25f;
			
		float filteredDeltaRotation = (sDeltaRotationOld * 0.0f) + (deltaRotation * 1.0f);
		YRotation += filteredDeltaRotation;
		sDeltaRotationOld = filteredDeltaRotation;
			
		// Rotate
		YRotation += OVRGamepadController.GetAxisRightX() * rotateInfluence;    
		
		// Update cameras direction and rotation
		SetCameras();
		
		//print(getVariance().magnitude);
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
	
	// This part detects whether or not the object is grounded and stores it in a variable
	void OnCollisionEnter (){
		state ++;
		if(state > 0){
			grounded = true;
		}
	}
	
	 
	void OnCollisionExit (){
		state --;
		if(state < 1){
			grounded = false;
			state = 0;
		}
	}

	// This is called every physics frame
	void FixedUpdate (){
 
		if(input.GetButton_Accel()){
			Vector3 forceVector = CameraController.transform.rotation * Vector3.forward * 5;
			print(forceVector);
			rigidbody.AddForce (forceVector);
		}
		
		//Brake
		if(input.GetButton_Brake()){
			rigidbody.AddForce(rigidbody.velocity * -3);
		}
		
 	}
	
	public override string GetControllerName() {
    	return "Input Tester Player";
   	}  
	
	private Vector3 getVariance(){
		
		Vector3 result = Vector3.zero;
		Vector3 average = getAverage();
		
		foreach(Vector3 vector in forwardDirHistory){
			result += Vector3.Scale(vector - average, vector - average);
		}
		
		result = result / (forwardDirHistory.Count - 1);
		
		return result;
	}
	
	private Vector3 getAverage(){
		Vector3 sum = Vector3.zero;
		foreach(Vector3 vector in forwardDirHistory){
			sum += vector;
		}
		
		Vector3 average = sum / forwardDirHistory.Count;
		return average;
	}
}