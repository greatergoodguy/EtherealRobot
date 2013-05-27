using UnityEngine;
using System.Collections;

public class MovementPC_Tom : PlayerController {
	
	public float moveSpeed = 6.0f;
	public float degreePerSecond = 4.0f;
	public float force = 8.0f;
	
	private Vector3 cube;
	private Vector3 sphere;
	private Vector3 cubeForward;
	private Vector3 sphereForward;
	private Vector3 crossProd;
	private Vector3 angDirection;
	private Transform head;
	
	private Vector3 forward;
	protected CameraController_BB 	CameraController 	= null;
	//protected OVRCameraController 	CameraController 	= null;
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
		
		CameraController_BB[] CameraControllers;
		CameraControllers = gameObject.GetComponentsInChildren<CameraController_BB>();
		
		if(CameraControllers.Length == 0)
			Debug.LogWarning("OVRPlayerController: No OVRCameraController attached.");
		else if (CameraControllers.Length > 1)
			Debug.LogWarning("OVRPlayerController: More then 1 OVRCameraController attached.");
		else
			CameraController = CameraControllers[0];
		/*OVRCameraController[] CameraControllers;
		CameraControllers = gameObject.GetComponentsInChildren<OVRCameraController>();
		
		if(CameraControllers.Length == 0)
			Debug.LogWarning("OVRPlayerController: No OVRCameraController attached.");
		else if (CameraControllers.Length > 1)
			Debug.LogWarning("OVRPlayerController: More then 1 OVRCameraController attached.");
		else
			CameraController = CameraControllers[0];*/
		
		cube = transform.position;
		sphereForward = transform.position;
		head = transform.FindChild("Head");
		SetCameras();
		
		AllowMouseRotation = false;
	}
	
	// Update is called once per frame
	void Update () {
		//head.transform.rotation = CameraController.transform.rotation;
		
		//Gets forward Vector
		cubeForward = transform.forward;
		sphereForward = head.forward;
		
		float angle = Vector3.Angle (cubeForward,sphereForward);
		crossProd = Vector3.Cross(cubeForward, sphereForward);
		
		//Force Vectors
		Vector3 forwardForce = new Vector3();
		
		//Basic Movement
		//use vector.up for rotating and float degreePerSecond and Time.deltaTime
		if(Input.GetKey(KeyCode.W)){
			//Vector3 tempPos = transform.position;
			Vector3 tempAngMove = transform.position;
			//tempPos += cubeForward * moveSpeed;
			/*if (angle < 46 && angle > 10 && crossProd.y < 0){
				degreePerSecond = -80;
				tempAngMove = Vector3.up * degreePerSecond * Time.deltaTime;
				transform.Rotate(tempAngMove);				
			}
			else if (angle < 46 && angle > 10 && crossProd.y > 0){
				degreePerSecond = 80;
				tempAngMove = Vector3.up * degreePerSecond * Time.deltaTime;
				transform.Rotate(tempAngMove);
			}*/
			forwardForce = cubeForward * moveSpeed * force;
			if (crossProd.y < 0){
				tempAngMove = GetAngularDirection(angle);
				tempAngMove = -tempAngMove;
				transform.Rotate(tempAngMove);				
			}
			else if (crossProd.y > 0){
				tempAngMove = GetAngularDirection(angle);
				transform.Rotate(tempAngMove);
			}
		}
		//if(Input.GetKey(KeyCode.S)){
			//Vector3 tempPos = transform.position;
			//Vector3 tempAngMove = transform.position;
			//tempPos -= cubeForward * moveSpeed;
			//if (angle < 46 && angle > 10 && crossProd.y < 0){
				//degreePerSecond = 80;
				//tempAngMove = Vector3.up * degreePerSecond * Time.deltaTime;
				//transform.Rotate(tempAngMove);				
			//}
			//else if (angle < 46 && angle > 10 && crossProd.y > 0){
				//degreePerSecond = -80;
				//tempAngMove = Vector3.up * degreePerSecond * Time.deltaTime;
				//transform.Rotate(tempAngMove);
			//}
			//tempPos = -cubeForward * moveSpeed * force;
			//rigidbody.AddForce(tempPos);
		//}
		angDirection = GetAngularDirection(angle);
		rigidbody.AddForce(forwardForce);
		//print(degreePerSecond);
		//print (angle);
		
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
	
	private Vector3 GetAngularDirection(float angle){
		Vector3 result = Vector3.zero;
		angle /= 90;
		result = Vector3.up * angle * degreePerSecond;
		return result;
	}
}
