using UnityEngine;
using System.Collections;

public class EtherealPC : PlayerController {
	
	//private float force = 6.0f;
	//private float moveSpeed = 5.0f;
	
	// Static Class Vars for Tuning
	public static float MIN_TURN_SENS = 0.0f;
	public static float MAX_TURN_SENS = 1.0f;
	public static float MIN_ACCEL = 0.1f;
	public static float MAX_ACCEL = 10.0f;
	public static float MIN_BRAKE_SPEED= 1.0f;
	public static float MAX_BRAKE_SPEED = 30.0f;
	public static float MIN_TOP_SPEED = 10.0f;
	public static float MAX_TOP_SPEED = 200.0f;
	public static float MIN_JUMP_POW = 1.0f;
	public static float MAX_JUMP_POW = 10.0f;
	
	// Public Tunable Movement Vars
	public float turnSensitivity = 0.1f;
	public float acceleration = 1.0f;
	public float brakeSpeed = 1.0f;			//dont make larger than max speed
	public float maxSpeed = 111.0f;
	public float jumpPower = 5.0f;
	
	// Private Parts
	private bool canJump = false;
	private float currForce = 0.0f;
	private float distToGround;
	private float velo;
	private float increaseRate = 1;
	//private Vector3 cube;
	//private Vector3 sphere;
	private Vector3 cubeForward;
	private Vector3 sphereForward;
	private Vector3 forwardForce;
	private Vector3 upwardForce;
	private Vector3 sphereAng;
	private Vector3 crossProd;
	private Transform head;
	private GameObject standardCameraGO;
	private GameObject oculusCameraGO;
	
	// Camera Stuff
	//protected CameraController_BB 	CameraController 	= null;
	//public float RotationAmount  = 1.5f;
	//private Quaternion OrientationOffset = Quaternion.identity;		// Initial direction of controller (passed down into CameraController)
	//private float YRotation = 0.0f;									// Rotation amount from inputs (passed down into CameraController)
	//private float RotationScaleMultiplier = 1.0f;
	//static float sDeltaRotationOld = 0.0f;
	protected OVRCameraController CameraController = null;
	
	// Transfom used to point player in a given direction; 
	// We should attach objects to this if we want them to rotate 
	// separately from the head (i.e. the body)
	//protected Transform DirXform = null;
	//public static bool AllowMouseRotation = true;
	
	public MouseLook_Ethereal mouseLook;
	//private DebugData debugData;
	
	// Awake always runs
	void Awake () {
		mouseLook = GetComponentInChildren<MouseLook_Ethereal>();
		DebugUtils.Assert(mouseLook != null);
	}
	
	// Start runs when gameObject is enabled, use for initialization
	void Start () {
		
		/* Friction fixes */
		rigidbody.freezeRotation = true;
		collider.material.dynamicFriction = 1.0f;
		collider.material.dynamicFriction2 = 1.0f;
		collider.material.staticFriction = 1.0f;
		collider.material.staticFriction2 = 1.0f;
		collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
		
		/* Initiate Rift control crap */
		OVRCameraController[] CameraControllers;
		CameraControllers = gameObject.GetComponentsInChildren<OVRCameraController>();
		if (CameraControllers.Length == 0)
			Debug.LogWarning("OVRPlayerController: No OVRCameraController attached.");
		else if (CameraControllers.Length > 1)
			Debug.LogWarning("OVRPlayerController: More then 1 OVRCameraController attached.");
		else
			CameraController = CameraControllers[0];
		
		/* Initiate other variables.. */
		distToGround = collider.bounds.extents.y;
		//cube = transform.position;
		//sphereForward = Vector3.zero;
		
		/* Initiate gameObjects */
		head = transform.FindChild("Head");
		transform.forward = head.forward;
		standardCameraGO = head.FindChild("Camera").gameObject;
		oculusCameraGO = head.FindChild("OVRCameraController").gameObject;
		/* Set default camera mode */
		standardCameraGO.SetActive(true);
		oculusCameraGO.SetActive(false);
		
		//SetCameras();					// TODO: What's this for?
		//AllowMouseRotation = false;	// TODO: Deprecated?
		
		/* Initate fine tuning GUI */
		debugData = new DebugData(this);
		debugData.AddData("Acceleration: ", 
			new DebugData.GetValueDelagate(GetAcceleration), 
			new DebugData.IncreaseDelagate(IncreaseAccel), 
			new DebugData.DecreaseDelagate(DecreaseAccel));
		debugData.AddData("Max Speed: ", 
			new DebugData.GetValueDelagate(GetMaxSpeed),
			new DebugData.IncreaseDelagate(IncreaseMaxSpeed), 
			new DebugData.DecreaseDelagate(DecreaseMaxSpeed));
		debugData.AddData("Sensitivy: ", 
			new DebugData.GetValueDelagate(GetSensitivity),
			new DebugData.IncreaseDelagate(IncreaseSensitivity), 
			new DebugData.DecreaseDelagate(DecreaseSensitivity));
		debugData.AddData("Brake Speed: ", 
			new DebugData.GetValueDelagate(GetBrakeSpeed),
			new DebugData.IncreaseDelagate(IncreaseBrakeSpeed), 
			new DebugData.DecreaseDelagate(DecreaseBrakeSpeed));
		debugData.AddData("Jump Power: ", 
			new DebugData.GetValueDelagate(GetJumpPower),
			new DebugData.IncreaseDelagate(IncreaseJumpPow), 
			new DebugData.DecreaseDelagate(DecreaseJumpPow));
	}
	
	// FixedUpdate is called once per frame
	void FixedUpdate () {		
		
		// Gets forward Vector
		cubeForward = transform.forward;
		sphereForward = head.forward;
		
		Vector3 sphereAng = new Vector3 (sphereForward.x, cubeForward.y, sphereForward.z);
		float absoluteAngle = Vector3.Angle (cubeForward,sphereAng);
		
		/* TODO: Extract contAngle out of parent
		 * is currently static class float var inherited from parent */
		contAngle = absoluteAngle * AngleDir (cubeForward, sphereAng, transform.up);
		//crossProd = Vector3.Cross(cubeForward, sphereAng);	// TODO: UNUSED
		
		// Steering Mechanics: rotates player head and body
		float currAng = Mathf.SmoothDamp (0f, contAngle, ref velo, turnSensitivity);
		if (!InputManager.activeInput.GetButton_Look()) {
			transform.Rotate (0f, currAng, 0f);
		}
			
		// Velocity Vector
		Vector3 currVeloVector = rigidbody.velocity;
		float currVelo = currVeloVector.magnitude;
		
		// Basic Movement Acceleration
		if (InputManager.activeInput.GetButton_Accel() ||
			InputManager.activeInput.GetButton_Forward()) {
			// Below max speed, add forward force
			if (currVelo < maxSpeed) {
				currForce += acceleration;
				forwardForce = cubeForward * currForce;
				rigidbody.AddForce(forwardForce);
				upwardForce = sphereForward * currForce * 2f;
				rigidbody.AddForce(upwardForce);
			// At max speed, maintain velocity
			} else {
				rigidbody.AddForce(-forwardForce);
				rigidbody.AddForce(-upwardForce);
				forwardForce = cubeForward * currForce;
				upwardForce = sphereForward * currForce;
				rigidbody.AddForce(forwardForce);
				rigidbody.AddForce(upwardForce);
			}
			//Debug.Log ("Current velocity = "+currVelo);
		// Brake Mechanic
		} else {				
			rigidbody.AddForce(rigidbody.velocity * -brakeSpeed);
			currForce = 0f;
		}
		
		// Jumping Mechanic
		if (canJump && IsGrounded()) {
			if (InputManager.activeInput.GetButtonDown_Jump()) {
				rigidbody.AddForce(Vector3.up * jumpPower * 100);
				canJump = false;
			}
		}
		// Camera Toggle
		if (InputManager.activeInput.GetButtonDown_SwitchCameraMode()) {
			SwitchCameraController();
		}
		
		/* TODO: UNNEEDED?
		// Controls the Camera rotation
		float rotateInfluence = DeltaTime * RotationAmount * RotationScaleMultiplier;
		// Rotate
		float deltaRotation = 0.0f;
		if(AllowMouseRotation == false){
			//deltaRotation = Input.GetAxis("Mouse X") * rotateInfluence * 3.25f;
		}
		float filteredDeltaRotation = (sDeltaRotationOld * 0.0f) + (deltaRotation * 1.0f);
		YRotation += filteredDeltaRotation;
		//sDeltaRotationOld = filteredDeltaRotation;
		// Rotate
		YRotation += OVRGamepadController.GetAxisRightX() * rotateInfluence;
		
		SetCameras();		// TODO: WHY?
		*/
	}
	
	/* TODO: DO WE NEED THESE?
	// InitializeInputs
	public void InitializeInputs() {
		// Get our start direction
		OrientationOffset = transform.rotation;
		// Make sure to set y rotation to 0 degrees
		YRotation = 0.0f;
	}
	public void SetCameras() {
		if (CameraController != null) {
			// Make sure to set the initial direction of the camera 
			// to match the game player direction
			CameraController.SetOrientationOffset(OrientationOffset);
			CameraController.SetYRotation(YRotation);
		}
	}
	*/
	
	/* TODO: Might need OnCollision stuff from Mech scripts */
	void OnCollisionEnter(Collision collision) {
		if (!canJump) canJump = true;
    }
    /* Checks to see if player is grounded */
	public bool IsGrounded() { 
		return Physics.Raycast (transform.position, -Vector3.up, distToGround + 0.2f); 
	}
    
	/* TODO: NOT USED, STILL NEEDED? */
	private Vector3 GetAngularDirection(float angle) {
		angle /= 90;									//scaled for optimal head movement
		return Vector3.up * angle * turnSensitivity;
	}
	/* Find angle direction: Left or Right 
	 * TODO: Conditionals can be used to implement control "dead zones" */
	private float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up) {
        Vector3 perp = Vector3.Cross (fwd, targetDir);
        float dir = Vector3.Dot (perp, up);
        if (dir > 0.0f)  		return 1.0f;
        else if (dir < 0.0f)  	return -1.0f;
        else 					return 0.0f; 
    }
	/* Switch between standard and Oculus camera modes */
	public override void SwitchCameraController(){
		standardCameraGO.SetActive(!standardCameraGO.activeSelf);
		oculusCameraGO.SetActive(!oculusCameraGO.activeSelf);	
		mouseLook.SwitchHeadMotion();
	}
	
	/* Getters */
	public override string GetControllerName() {return "Ethereal"; }
	public float GetAngle() { 					return contAngle; }
	public override DebugData GetDebugData() {	return debugData; }
	public float GetAcceleration() {			return acceleration; }
	public float GetMaxSpeed() {				return maxSpeed; }
	public float GetSensitivity() {				return turnSensitivity; }
	public float GetBrakeSpeed() {				return brakeSpeed; }
	public float GetJumpPower() {				return jumpPower; }
	
	/* MOVEMENT TUNING HELPER METHODS */
	public void IncreaseAccel() {
		acceleration += Time.deltaTime * increaseRate;
		if (acceleration > MAX_ACCEL) acceleration = MAX_ACCEL;	
	}
	public void DecreaseAccel() {
		acceleration -= Time.deltaTime * increaseRate;
		if (acceleration < MIN_ACCEL) acceleration = MIN_ACCEL;	
	}
	public void IncreaseMaxSpeed() {
		maxSpeed += Time.deltaTime * increaseRate;
		if (maxSpeed > MAX_TOP_SPEED) maxSpeed = MAX_TOP_SPEED;	
	}
	public void DecreaseMaxSpeed() {
		maxSpeed -= Time.deltaTime * increaseRate;
		if (maxSpeed < MIN_TOP_SPEED) maxSpeed = MIN_TOP_SPEED;	
	}
	public void IncreaseSensitivity() {
		turnSensitivity += Time.deltaTime * increaseRate;
		if (turnSensitivity > MAX_TURN_SENS)	turnSensitivity = MAX_TURN_SENS;	
	}
	public void DecreaseSensitivity() {
		turnSensitivity -= Time.deltaTime * increaseRate;
		if (turnSensitivity < MIN_TURN_SENS) turnSensitivity = MIN_TURN_SENS;	
	}
	public void IncreaseBrakeSpeed() {
		brakeSpeed += Time.deltaTime * increaseRate;
		if (brakeSpeed > MAX_BRAKE_SPEED) brakeSpeed = MAX_BRAKE_SPEED;	
	}
	public void DecreaseBrakeSpeed() {
		brakeSpeed -= Time.deltaTime * increaseRate;
		if (brakeSpeed < MIN_BRAKE_SPEED) brakeSpeed = MIN_BRAKE_SPEED;	
	}
	public void IncreaseJumpPow() {
		jumpPower += Time.deltaTime * increaseRate;
		if (jumpPower > MAX_JUMP_POW) jumpPower = MAX_JUMP_POW;	
	}
	public void DecreaseJumpPow() {
		jumpPower -= Time.deltaTime * increaseRate;
		if (jumpPower < MIN_JUMP_POW) jumpPower = MIN_JUMP_POW;	
	}
}
