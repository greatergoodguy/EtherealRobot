using UnityEngine;
using System.Collections;

public class MovementPC : PlayerController {
	
	public float moveSpeed = 0.25f;
	private float angle;
	
	private Vector3 cube;
	private Vector3 sphere;
	private Vector3 cubeForward;
	private Vector3 sphereForward;
	private Transform head;
	
	private Vector3 forward;
	protected CameraController_BB 	CameraController 	= null;
	public float RotationAmount  = 1.5f;
	private Quaternion OrientationOffset = Quaternion.identity;			// Initial direction of controller (passed down into CameraController)
	private float 	YRotation 	 = 0.0f;								// Rotation amount from inputs (passed down into CameraController)
	private float RotationScaleMultiplier = 1.0f;
	static float sDeltaRotationOld = 0.0f;
	
	// Transfom used to point player in a given direction; 
	// We should attach objects to this if we want them to rotate 
	// separately from the head (i.e. the body)
	protected Transform DirXform = null;
	//
	// STATIC VARIABLES
	//
	public static bool  AllowMouseRotation      = true;
	
	// Use this for initialization
	void Start () {
		
		CameraController_BB[] CameraControllers;
		CameraControllers = gameObject.GetComponentsInChildren<CameraController_BB>();
		
		if(CameraControllers.Length == 0)
			Debug.LogWarning("OVRPlayerController: No OVRCameraController attached.");
		else if (CameraControllers.Length > 1)
			Debug.LogWarning("OVRPlayerController: More then 1 OVRCameraController attached.");
		else
			CameraController = CameraControllers[0];
		
		cube = transform.position;
		sphereForward = transform.position;
		head = transform.FindChild("Sphere");
		SetCameras();
		
		AllowMouseRotation = false;
	}
	
	// Update is called once per frame
	void Update () {
		
		//Gets forward Vector
		cubeForward = transform.forward;
		sphereForward = head.forward;
		float angle = Vector3.Angle (cubeForward,sphereForward);
		
		//Basic Movement
		if(Input.GetKey(KeyCode.W)){
			Vector3 tempPos = transform.position;
			tempPos += cubeForward * moveSpeed;
			transform.position = tempPos;
		}
		if(Input.GetKey(KeyCode.S)){
			Vector3 tempPos = transform.position;
			tempPos -= cubeForward * moveSpeed;
			transform.position = tempPos;
		}
		print (angle);
		
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
		SetCameras();
	}
	
	// InitializeInputs
	public void InitializeInputs()
	{
		// Get our start direction
		OrientationOffset = transform.rotation;
		// Make sure to set y rotation to 0 degrees
		YRotation = 0.0f;
	}
	
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
	
	public override string GetControllerName() {
    	return "Ball";
   	}
}
