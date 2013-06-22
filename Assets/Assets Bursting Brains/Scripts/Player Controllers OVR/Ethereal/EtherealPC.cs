using UnityEngine;
using System.Collections;

public class EtherealPC : PlayerController {
	
	//private float force = 6.0f;
	//private float moveSpeed = 5.0f;
	
	// Static Class Vars for Tuning
	public static float MIN_TURN_SENS = 0.0f;
	public static float MAX_TURN_SENS = 1.0f;
	public static float MIN_ACCEL = 0.1f;
	public static float MAX_ACCEL = 10.0f;			// Amount of seconds to reach top speed
	public static float MIN_BRAKE_SPEED= 1.0f;
	public static float MAX_BRAKE_SPEED = 30.0f;
	public static float MIN_TOP_SPEED = 10.0f;
	public static float MAX_TOP_SPEED = 293.333f;	// Unit is feet/sec == 200 mph
	public static float MIN_JUMP_POW = 1.0f;
	public static float MAX_JUMP_POW = 10.0f;
	public static float MIN_GRAV = 0.0f;
	public static float MAX_GRAV = 120.0f;			// Unit is feet/sec², Earth grav = 30 ft/s²
	public static float MIN_HOVR = 0.0f;
	public static float MAX_HOVR = 100.0f;			// Unit is feet above ground
	// Public Tunable Movement Vars
	public float turnSensitivity = 0.5f;
	public float acceleration = 2.5f;
	public float brakeSpeed = 1.0f;					// Don´t make larger than max speed
	public float maxSpeed = 88.0f;
	public float jumpPower = 5.0f;
	public float hoverHeight = 40.0f;
	public float grav = 30.0f;
	public float lookAngleX, lookAngleY;			// TODO: Get rid of this
	// Private Parts
	private float increaseRate = 1f;
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
	public EtherealMove etherealMove;
	private DebugData debugData;
	
	// Awake always runs once regardless of object state
	void Awake () {
	
		etherealMove = GetComponent<EtherealMove>();
		mouseLook = GetComponentInChildren<MouseLook_Ethereal>();
		DebugUtils.Assert(mouseLook != null);
	}
	
	// Start runs when gameObject is enabled, use for initialization
	void Start () {
		
		/* Initialize Rift control crap */
		OVRCameraController[] CameraControllers;
		CameraControllers = gameObject.GetComponentsInChildren<OVRCameraController>();
		if (CameraControllers.Length == 0)
			Debug.LogWarning("OVRPlayerController: No OVRCameraController attached.");
		else if (CameraControllers.Length > 1)
			Debug.LogWarning("OVRPlayerController: More then 1 OVRCameraController attached.");
		else
			CameraController = CameraControllers[0];
		/* Initialize gameObjects */
		standardCameraGO = transform.Find("Head").Find("Camera").gameObject;
		oculusCameraGO = transform.Find("Head").Find("OVRCameraController").gameObject;
		//oculusCameraGO = transform.Find("Head").Find("OVRCameraController").gameObject;
		/* Set default camera mode */
		standardCameraGO.SetActive(true);
		oculusCameraGO.SetActive(false);
		/* Initialize Debug GUI */
		initializeDebug();
		
		/* Deprecated?
		SetCameras();											// TODO: What's this for?
		AllowMouseRotation = false;								// TODO: What's this for?
		*/
	}
	
	// Update is called every render frame
	void Update () {
	
		if (InputManager.activeInput.GetButtonDown_SwitchCameraMode()) {	// Camera Toggle
			SwitchCameraController();
		}
	}
	
	/* Switch between standard and Oculus camera modes */
	public override void SwitchCameraController(){
		standardCameraGO.SetActive(!standardCameraGO.activeSelf);
		oculusCameraGO.SetActive(!oculusCameraGO.activeSelf);	
		mouseLook.SwitchHeadMotion();
	}

	private void initializeDebug () {
		/* Initate fine tuning GUI */
		debugData = new DebugData(this);
		debugData.AddData("Current Velocity: ", 
			new DebugData.GetValueDelegate(GetCurrVelocity),
			new DebugData.IncreaseDelegate(DummyFunction), 
			new DebugData.DecreaseDelegate(DummyFunction));
		debugData.AddData("Acceleration: ", 
			new DebugData.GetValueDelegate(GetAcceleration), 
			new DebugData.IncreaseDelegate(IncreaseAccel), 
			new DebugData.DecreaseDelegate(DecreaseAccel));
		debugData.AddData("Max Speed: ", 
			new DebugData.GetValueDelegate(GetMaxSpeed),
			new DebugData.IncreaseDelegate(IncreaseMaxSpeed), 
			new DebugData.DecreaseDelegate(DecreaseMaxSpeed));
		debugData.AddData("Sensitivity: ", 
			new DebugData.GetValueDelegate(GetSensitivity),
			new DebugData.IncreaseDelegate(IncreaseSensitivity), 
			new DebugData.DecreaseDelegate(DecreaseSensitivity));
		/*
		debugData.AddData("Brake Speed: ", 
			new DebugData.GetValueDelegate(GetBrakeSpeed),
			new DebugData.IncreaseDelegate(IncreaseBrakeSpeed), 
			new DebugData.DecreaseDelegate(DecreaseBrakeSpeed));
		*/
		debugData.AddData("Jump Power: ", 
			new DebugData.GetValueDelegate(GetJumpPower),
			new DebugData.IncreaseDelegate(IncreaseJumpPow), 
			new DebugData.DecreaseDelegate(DecreaseJumpPow));
		debugData.AddData("Falling Gravity: ", 
			new DebugData.GetValueDelegate(GetGrav),
			new DebugData.IncreaseDelegate(IncreaseGrav), 
			new DebugData.DecreaseDelegate(DecreaseGrav));
		debugData.AddData("Hover Height: ", 
			new DebugData.GetValueDelegate(GetHoverHeight),
			new DebugData.IncreaseDelegate(IncreaseHovr), 
			new DebugData.DecreaseDelegate(DecreaseHovr));
	}
	
	/* Getters */
	public override DebugData GetDebugData() {	return debugData; }
	public override string GetControllerName() {return "Ethereal"; }
	public float GetAngle() { 					return lookAngleY; }
	public float GetAcceleration() {			return acceleration; }
	public float GetMaxSpeed() {				return maxSpeed; }
	public float GetSensitivity() {				return turnSensitivity; }
	public float GetBrakeSpeed() {				return brakeSpeed; }
	public float GetJumpPower() {				return jumpPower; }
	public float GetGrav() {					return grav; }
	public float GetHoverHeight() {				return hoverHeight; }
	public float GetCurrVelocity() {			return rigidbody.GetPointVelocity(transform.position).magnitude; }
	
	/* MOVEMENT TUNING HELPER METHODS */
	public void IncreaseAccel() {
		  acceleration += Time.deltaTime * increaseRate;
		if (  acceleration > MAX_ACCEL)   acceleration = MAX_ACCEL;	
	}
	public void DecreaseAccel() {
		  acceleration -= Time.deltaTime * increaseRate;
		if (  acceleration < MIN_ACCEL)   acceleration = MIN_ACCEL;	
	}
	public void IncreaseMaxSpeed() {
		  maxSpeed += Time.deltaTime * increaseRate;
		if (  maxSpeed > MAX_TOP_SPEED)   maxSpeed = MAX_TOP_SPEED;	
	}
	public void DecreaseMaxSpeed() {
		  maxSpeed -= Time.deltaTime * increaseRate;
		if (  maxSpeed < MIN_TOP_SPEED)   maxSpeed = MIN_TOP_SPEED;	
	}
	public void IncreaseSensitivity() {
		  turnSensitivity += Time.deltaTime * increaseRate;
		if (  turnSensitivity > MAX_TURN_SENS)	  turnSensitivity = MAX_TURN_SENS;	
	}
	public void DecreaseSensitivity() {
		  turnSensitivity -= Time.deltaTime * increaseRate;
		if (  turnSensitivity < MIN_TURN_SENS)   turnSensitivity = MIN_TURN_SENS;	
	}
	public void IncreaseBrakeSpeed() {
		  brakeSpeed += Time.deltaTime * increaseRate;
		if (  brakeSpeed > MAX_BRAKE_SPEED)   brakeSpeed = MAX_BRAKE_SPEED;	
	}
	public void DecreaseBrakeSpeed() {
		  brakeSpeed -= Time.deltaTime * increaseRate;
		if (  brakeSpeed < MIN_BRAKE_SPEED)   brakeSpeed = MIN_BRAKE_SPEED;	
	}
	public void IncreaseJumpPow() {
		  jumpPower += Time.deltaTime * increaseRate;
		if (  jumpPower > MAX_JUMP_POW)   jumpPower = MAX_JUMP_POW;	
	}
	public void DecreaseJumpPow() {
		  jumpPower -= Time.deltaTime * increaseRate;
		if (  jumpPower < MIN_JUMP_POW)   jumpPower = MIN_JUMP_POW;	
	}
	public void IncreaseGrav() {
		  grav += Time.deltaTime * increaseRate;
		if (  grav > MAX_GRAV)   grav = MAX_GRAV;	
	}
	public void DecreaseGrav() {
		  grav -= Time.deltaTime * increaseRate;
		if (  grav < MIN_GRAV)   grav = MIN_GRAV;	
	}
	public void IncreaseHovr() {
		  hoverHeight += Time.deltaTime * increaseRate;
		if (  hoverHeight > MAX_HOVR)   hoverHeight = MAX_HOVR;	
	}
	public void DecreaseHovr() {
		  hoverHeight -= Time.deltaTime * increaseRate;
		if (  hoverHeight < MIN_HOVR)   hoverHeight = MIN_HOVR;	
	}
	public void DummyFunction() {
	}
}
