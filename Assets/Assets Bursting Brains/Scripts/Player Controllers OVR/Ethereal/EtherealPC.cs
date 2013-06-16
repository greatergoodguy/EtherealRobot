using UnityEngine;
using System.Collections;

public class EtherealPC : PlayerController {
	
	//private float force = 6.0f;
	//private float moveSpeed = 5.0f;

	public float normalGravity = -9.81f;
	public float airGravity = -60.0f;
	public float turnSensitivity = 0.4f;
	public float acceleration = 0.5f;
	public float brakeSpeed = 1.0f;			//dont make larger than max speed
	public float maxSpeed = 88.0f;
	public float jumpPower = 5.0f;
	private float currForce = 0.0f;
	private float distToGround;
	private float velo;
	
	private float increaseRate = 1;
	
	private bool stucktoGround = false;
	private bool hasStuckToGround = false;
		
	public void IncreaseAccel(){
		acceleration += Time.deltaTime * increaseRate;
		if(acceleration > MAX_ACCEL){
			acceleration = MAX_ACCEL;	
		}
	}
	public void DecreaseAccel(){
		acceleration -= Time.deltaTime * increaseRate;
		if(acceleration < MIN_ACCEL){
			acceleration = MIN_ACCEL;	
		}
	}
	public void IncreaseMaxSpeed(){
		maxSpeed += Time.deltaTime * increaseRate;
		if(maxSpeed > MAX_TOP_SPEED){
			maxSpeed = MAX_TOP_SPEED;	
		}
	}
	public void DecreaseMaxSpeed(){
		maxSpeed -= Time.deltaTime * increaseRate;
		if(maxSpeed < MIN_TOP_SPEED){
			maxSpeed = MIN_TOP_SPEED;	
		}
	}
	public void IncreaseSensitivity(){
		turnSensitivity += Time.deltaTime * increaseRate;
		if(turnSensitivity > MAX_TURN_SENS){
			turnSensitivity = MAX_TURN_SENS;	
		}
	}
	public void DecreaseSensitivity(){
		turnSensitivity -= Time.deltaTime * increaseRate;
		if(turnSensitivity < MIN_TURN_SENS){
			turnSensitivity = MIN_TURN_SENS;	
		}
	}
	public void IncreaseBrakeSpeed(){
		brakeSpeed += Time.deltaTime * increaseRate;
		if(brakeSpeed > MAX_BRAKE_SPEED){
			brakeSpeed = MAX_BRAKE_SPEED;	
		}
	}
	public void DecreaseBrakeSpeed(){
		brakeSpeed -= Time.deltaTime * increaseRate;
		if(brakeSpeed < MIN_BRAKE_SPEED){
			brakeSpeed = MIN_BRAKE_SPEED;	
		}
	}
	public void IncreaseJumpPow(){
		jumpPower += Time.deltaTime * increaseRate;
		if(jumpPower > MAX_JUMP_POW){
			jumpPower = MAX_JUMP_POW;	
		}
	}
	public void DecreaseJumpPow(){
		jumpPower -= Time.deltaTime * increaseRate;
		if(jumpPower < MIN_JUMP_POW){
			jumpPower = MIN_JUMP_POW;	
		}
	}
	
	
	
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
	
	//jump variables
	private int state = 0;
	private bool canJump = true;
	
	private Vector3 cube;
	private Vector3 sphere;
	private Vector3 cubeForward;
	private Vector3 sphereForward;
	private Vector3 forwardForce;
	private Vector3 sphereAng;
	private Vector3 crossProd;
	private Vector3 forward;
	private Vector3 angDirection;
	private Transform head;
	private GameObject standardCameraGO;
	private GameObject oculusCameraGO;
	
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
	
	public MouseLook_Ethereal mouseLook;
	
	private DebugData debugData;
	
	void Awake () {
		mouseLook = GetComponentInChildren<MouseLook_Ethereal>();
		DebugUtils.Assert(mouseLook != null);
	}
	
	// Use this for initialization
	void Start () {
		
		//Friction Fixes
		rigidbody.freezeRotation = true;
		
		collider.material.dynamicFriction = 1.0f;
		collider.material.dynamicFriction2 = 1.0f;
		collider.material.staticFriction = 1.0f;
		collider.material.staticFriction2 = 1.0f;
		collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
		
		OVRCameraController[] CameraControllers;
		CameraControllers = gameObject.GetComponentsInChildren<OVRCameraController>();
		
		if(CameraControllers.Length == 0)
			Debug.LogWarning("OVRPlayerController: No OVRCameraController attached.");
		else if (CameraControllers.Length > 1)
			Debug.LogWarning("OVRPlayerController: More then 1 OVRCameraController attached.");
		else
			CameraController = CameraControllers[0];
		
		distToGround = collider.bounds.extents.y;
		cube = transform.position;
		sphereForward = transform.position;
		head = transform.FindChild("Head");
		standardCameraGO = head.FindChild("Camera").gameObject;
		oculusCameraGO = head.FindChild("OVRCameraController").gameObject;
		
		standardCameraGO.SetActive(true);
		oculusCameraGO.SetActive(false);
		
		SetCameras();
		
		AllowMouseRotation = false;
		
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
	
	float contAngle = 0;
	// Update is called once per frame
	void FixedUpdate () {		
		//Gets forward Vector
		cubeForward = transform.forward;
		sphereForward = head.forward;
		Vector3 sphereAng = new Vector3 (sphereForward.x, cubeForward.y, sphereForward.z);
		
		float absoluteAngle = Vector3.Angle (cubeForward,sphereAng);
		
		contAngle = absoluteAngle * AngleDir(transform.forward, sphereAng, transform.up);
		crossProd = Vector3.Cross(cubeForward, sphereAng);
		
		//Steering Mechanics
		float currAng = Mathf.SmoothDamp(0f, contAngle, ref velo, turnSensitivity);
		/*
		Vector3 angMove = transform.position;
		if (crossProd.y < 0){
			angMove = GetAngularDirection(absoluteAngle);
			angMove = -angMove;
			transform.Rotate(angMove);				
		}
		else if (crossProd.y > 0) {
			angMove = GetAngularDirection(absoluteAngle);
			transform.Rotate(angMove);
		}
		*/
		transform.Rotate(0f, currAng, 0f);
			
		//Force Vectors
		//Vector3 forwardForce = new Vector3();
		//Vector3 brakeForce = new Vector3();
	
		// Increases Gravity while in the air for grater fall speed
		if(IsNearlyGrounded()){
			rigidbody.AddForce(Vector3.up * normalGravity);	 // Does not use actual gravity, just changes for Ethereal object
			//Physics.gravity = new Vector3(0, normalGravity, 0);
		}
		else {
			rigidbody.AddForce(Vector3.up * airGravity);			
			//Physics.gravity = new Vector3(0, airGravity, 0);
		}
			
		//Velocity Vector
		Vector3 currVeloVector = rigidbody.velocity;
		float currVelo = currVeloVector.magnitude;
		
		//Basic Movement Acceleration
		if(InputManager.activeInput.GetButton_Accel() ||
			InputManager.activeInput.GetButton_Forward()){
			
			//Debug.Log ("Current velocity = "+currVelo);
			if (currVelo < maxSpeed) {
				currForce += acceleration;
				//currForce = 2f;
				forwardForce = cubeForward * currForce;
				rigidbody.AddForce(forwardForce);
			} else {
				rigidbody.AddForce(-forwardForce);
				forwardForce = cubeForward * currForce;
				rigidbody.AddForce(forwardForce);
			}
		//Brake Mechanic
		} else {				
			rigidbody.AddForce(rigidbody.velocity * -brakeSpeed);
			currForce = 0f;
		}
			
		if(canJump){
			if(InputManager.activeInput.GetButtonDown_Jump()){
				rigidbody.AddForce(Vector3.up * jumpPower * 100);
				canJump = false;
			}
		}
		
		if(InputManager.activeInput.GetButtonDown_SwitchCameraMode()){
			SwitchCameraController();
		}
		
		// Controls the Camera rotation
		float rotateInfluence = DeltaTime * RotationAmount * RotationScaleMultiplier;
		
		// Rotate
		float deltaRotation = 0.0f;
		if(AllowMouseRotation == false){
			/*
			deltaRotation = Input.GetAxis("Mouse X") * rotateInfluence * 3.25f;
			*/
		}
			
		float filteredDeltaRotation = (sDeltaRotationOld * 0.0f) + (deltaRotation * 1.0f);
		YRotation += filteredDeltaRotation;
		//sDeltaRotationOld = filteredDeltaRotation;
			
		// Rotate
		YRotation += OVRGamepadController.GetAxisRightX() * rotateInfluence;


		// Sticks player to ground if they press 'B'
        if (Input.GetKeyDown(KeyCode.G)){	
			stucktoGround = !stucktoGround;
			hasStuckToGround = false;

		}	
		if(stucktoGround){
			if(!hasStuckToGround){
				if(IsNearlyGrounded())
					hasStuckToGround = true;
			}
			else {
				GroundStick();
			}
		}
		
		SetCameras();

	}
	
	// Sticks player to the ground
	public void GroundStick() {
		RaycastHit hit = new RaycastHit();
		if(!IsNearlyGrounded()){
			if(Physics.Raycast(transform.position, -Vector3.up, out hit)){	
				float groundHeight = transform.position.y - (hit.distance - (distToGround * 2));
				Vector3 grounded = new Vector3(transform.position.x, groundHeight, transform.position.z);
				transform.position = grounded;
			}
		}	
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
			//CameraController.SetOrientationOffset(OrientationOffset);
			CameraController.SetYRotation(YRotation);
		}
	}
	
	void OnCollisionEnter(Collision collision){
		if (!canJump){
			canJump = true;
		}
    }
	
	public override string GetControllerName() {
    	return "Ethereal";
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
	
	public float GetAcceleration(){			return acceleration;}
	public float GetMaxSpeed(){				return maxSpeed;}
	public float GetSensitivity(){			return turnSensitivity;}
	public float GetBrakeSpeed(){			return brakeSpeed;}
	public float GetJumpPower(){			return jumpPower;}
	
	public bool IsGrounded(){
  		return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.2f);
	}
	
	public bool IsNearlyGrounded(){
  		return Physics.Raycast(transform.position, -Vector3.up, distToGround + 1.0f);
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
	
	public override void SwitchCameraController(){
		standardCameraGO.SetActive(!standardCameraGO.activeSelf);
		oculusCameraGO.SetActive(!oculusCameraGO.activeSelf);	
		
		mouseLook.SwitchHeadMotion();
	}
	
	public override DebugData GetDebugData(){
		return debugData;
	}
}
