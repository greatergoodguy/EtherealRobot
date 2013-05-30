using UnityEngine;
using System.Collections;

public class Oculus_MovementPC : PlayerController {
	
	//private float force = 6.0f;
	//private float moveSpeed = 5.0f;
	
	public float turnSensitivity = 2.0f;
	public float acceleration = 1.0f;
	public float brakeSpeed = 1.0f;			//dont make larger than max speed
	public float maxSpeed = 30.0f;
	public float jumpPower = 5.0f;
	private float currSpeed = 0.0f;
	
	//jump variables
	private int state = 0;
	private bool grounded = false;
	
	private Vector3 cube;
	private Vector3 sphere;
	private Vector3 cubeForward;
	private Vector3 sphereForward;
	private Vector3 crossProd;
	private Vector3 forward;
	private Vector3 angDirection;
	private Transform head;
	private GameObject standardCam;
	private GameObject oculusCam;
	
	//Camera Variable
	//protected CameraController_BB 	CameraController 	= null;
	protected OVRCameraController 	CameraController 	= null;
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
		
		//Friction Fixes
		rigidbody.freezeRotation = true;
		
		collider.material.dynamicFriction = .2f;
		collider.material.dynamicFriction2 = .2f;
		collider.material.staticFriction = .2f;
		collider.material.staticFriction2 = .2f;
		collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
		
		/*
		CameraController_BB[] CameraControllers;
		CameraControllers = gameObject.GetComponentsInChildren<CameraController_BB>();
		
		if(CameraControllers.Length == 0)
			Debug.LogWarning("OVRPlayerController: No OVRCameraController attached.");
		else if (CameraControllers.Length > 1)
			Debug.LogWarning("OVRPlayerController: More then 1 OVRCameraController attached.");
		else
			CameraController = CameraControllers[0];
		*/
		OVRCameraController[] CameraControllers;
		CameraControllers = gameObject.GetComponentsInChildren<OVRCameraController>();
		
		if(CameraControllers.Length == 0)
			Debug.LogWarning("OVRPlayerController: No OVRCameraController attached.");
		else if (CameraControllers.Length > 1)
			Debug.LogWarning("OVRPlayerController: More then 1 OVRCameraController attached.");
		else
			CameraController = CameraControllers[0];
		
		cube = transform.position;
		sphereForward = transform.position;
		head = transform.FindChild("Head");
		standardCam = head.FindChild("Camera").gameObject;
		oculusCam = head.FindChild("OVRCameraController").gameObject;
		
		standardCam.SetActive(true);
		oculusCam.SetActive(false);
		
		SetCameras();
		
		AllowMouseRotation = false;
	}
	
	float contAngle = 0;
	// Update is called once per frame
	void Update () {
		
		//Gets forward Vector
		cubeForward = transform.forward;
		sphereForward = head.forward;
		
		float absoluteAngle = Vector3.Angle (cubeForward,sphereForward);
		
		contAngle = absoluteAngle * AngleDir(transform.forward, sphereForward, transform.up);
		//print("cubeForward: " + cubeForward.x + "       sphereForward: " + sphereForward.x);
		crossProd = Vector3.Cross(cubeForward, sphereForward);
		
		//Force Vectors
		Vector3 forwardForce = new Vector3();
		//Vector3 brakeForce = new Vector3();
		
		//Basic Movement Acceleration
		if(Input.GetKey(KeyCode.W)){
			
			Vector3 tempAngMove = transform.position;
			currSpeed += acceleration;
			currSpeed = Mathf.Clamp(currSpeed, -maxSpeed, maxSpeed);
			
			forwardForce = cubeForward * currSpeed;
			rigidbody.AddForce(forwardForce);
		}
		//Brake Mechanic
		else{				
			//brakeForce = -cubeForward * currSpeed;
			rigidbody.AddForce(rigidbody.velocity * -brakeSpeed);
		}
		
		/*if(Input.GetKey(KeyCode.S)){
			Vector3 tempPos = transform.position;
			Vector3 tempAngMove = transform.position;
			tempPos -= cubeForward * moveSpeed;
			if (angle < 46 && angle > 10 && crossProd.y < 0){
				degreePerSecond = 80;
				tempAngMove = Vector3.up * degreePerSecond * Time.deltaTime;
				transform.Rotate(tempAngMove);				
			}
			else if (angle < 46 && angle > 10 && crossProd.y > 0){
				degreePerSecond = -80;
				tempAngMove = Vector3.up * degreePerSecond * Time.deltaTime;
				transform.Rotate(tempAngMove);
			}
			tempPos = -cubeForward * moveSpeed * force;
			rigidbody.AddForce(tempPos);
		}*/
		
		//Steering Mechanics
		Vector3 angMove = transform.position;
		if (crossProd.y < 0){
			angMove = GetAngularDirection(absoluteAngle);
			angMove = -angMove;
			transform.Rotate(angMove);				
		}
		else if (crossProd.y > 0){
			angMove = GetAngularDirection(absoluteAngle);
			transform.Rotate(angMove);
		}
		
		//Camera Switch
		if(Input.GetKeyDown(KeyCode.V)){
			standardCam.SetActive(!standardCam.activeSelf);
			oculusCam.SetActive(!oculusCam.activeSelf);
		}
		
		//Jump
		if(grounded && Input.GetKeyDown(KeyCode.LeftShift)){
			rigidbody.AddForce(Vector3.up * jumpPower * 100);
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
	
	public override string GetControllerName() {
    	return "Ball";
   	}
	
	private Vector3 GetAngularDirection(float angle){
		Vector3 result = Vector3.zero;
		angle /= 90;									//scaled for optimal head movement
		result = Vector3.up * angle * turnSensitivity;
		return result;
	}
	
	public float GetAngle(){
		return contAngle;
	}
	
	private float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up){
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0.0f) {
            return 1.0f;

        } else if (dir < 0.0f) {
            return -1.0f;

        } else {
            return 0.0f;
        }

    }
}
