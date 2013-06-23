using UnityEngine;
using System.Collections;

public class EtherealMove : MonoBehaviour {

	// Public Tunable Movement Vars
	public float turnSensitivity = 5f;
	public float acceleration = 2.5f;
	public float brakeSpeed = 1.0f;				// Don´t make larger than max speed
	public float maxSpeed = 88.0f;
	public float jumpPower = 5.0f;
	public float hoverHeight = 29.0f;
	public float grav = 30.0f;
	public float warpRatio = 0.5f;				// 0..1: percent of FOV increase at max speed
	public float inputDeadZone = 13.0f;			// Input deadzone in degrees from origin
	public float corneringThrustRatio = 0.5f;
	// Physics Vars
	private bool theFinalFrontier = false;
	private bool canJump, throttleOn = false;
	private float veloY, distToGround;
	private float angleDiff, angleVelo, turnSig;
	private Vector3 jumpForce;
	private Vector3 modifiedVeloAngle;			// TODO: questionable use 
	private Quaternion noBacksies;
	private Transform head;
	// Rotation Vars	
	public float lookAngleX, lookAngleY = 0f;	// TODO: questionable use
	private float deltaGs, yInt;
	public float maxVeloCornerAngle = 35.0f;	// max cornering angle at max speed (200mph)
	// Other Private Parts
	private float camFOV;
	private Camera[] activeCams;
	
	/* TODO: Rid lookAngle from parent, is static class float var */
	
	void Awake () {
		float maxAngle = 80f; 
		deltaGs = (maxVeloCornerAngle - maxAngle) / 293f;	// Interpolate angle delta from 1-292 ft/s
		yInt = 80f - deltaGs;	
	}
	
	// Use this for initialization
	void Start () {
	
		/* Rigidbody Friction Settings */
		//rigidbody.constraints = 
			//RigidbodyConstraints.FreezeRotationX | 
			//RigidbodyConstraints.FreezeRotationY |
			//RigidbodyConstraints.FreezeRotationZ;
		rigidbody.drag = 0f;
		collider.material.dynamicFriction = 1.0f;
		collider.material.dynamicFriction2 = 1.0f;
		collider.material.staticFriction = 1.0f;
		collider.material.staticFriction2 = 1.0f;
		collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
		/* Enable Rigidbody Interpolation: smooths fixed frame rate physics */
		rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
		/* Initialize other variables */
		head = transform.Find("Head");
		transform.forward = head.forward;
		distToGround = collider.bounds.extents.y;
		camFOV = Camera.main.fieldOfView;
	}
	
	// FixedUpdate is called every fixed physics frame
	void FixedUpdate () {		
		
		// Set Gravity
		rigidbody.useGravity = theFinalFrontier; 
		// Set Us up the Bomb	
		float altitude;
		float potentAngle = rigidbody.velocity.magnitude * deltaGs + yInt;
		float thrust = maxSpeed/acceleration;
		bool understeer = angleDiff > angleVelo && potentAngle > angleVelo;
		float thrustMod = (understeer == true) ? (potentAngle-angleVelo)/potentAngle : 0f;
		Vector3 forwardThrust = head.forward * thrust;
		Vector3 corneringThrust = (-head.right * turnSig) * thrust * thrustMod;
		Vector3 modifiedThrust = (forwardThrust + corneringThrust).normalized * thrust;
		/* NOTE: Force = units/s/n, at n seconds will be at units/s velocity */
		
		// "Gravity"/Hovering and "Physics"
		if (!theFinalFrontier) {
			// TODO: Considers anything with a collider as "ground", needs work
			RaycastHit ground;
			if (Physics.Raycast (transform.position, -Vector3.up, out ground)) {
				altitude = ground.distance;
				if (altitude > hoverHeight)	{			// Exiting Hover Zone: Thrust Disabled
					rigidbody.useGravity = true;
				} else {								// Entering Hover Zone: Thrust Enabled
					rigidbody.useGravity = false;
		
					// Accel/Decel and Cornering "Physics"
					// TODO: Consider ForceMode.Velocity with analog controls using deltaTime
					if (throttleOn) { 
						Debug.Log ("GO TIME");
						rigidbody.AddForce (modifiedThrust, ForceMode.Acceleration); }
					else { 
						Debug.Log ("NO GO TIME");
						rigidbody.AddForce (rigidbody.velocity * -0.69f, ForceMode.Acceleration); }
					
					// Velocity Limiter
					if (rigidbody.velocity.magnitude > maxSpeed) {
						float v = rigidbody.velocity.magnitude;
						float oppositeF = maxSpeed - v;
						rigidbody.AddForce (rigidbody.velocity.normalized * oppositeF, ForceMode.VelocityChange);
					} 
					
					// Jumping "Mechanic"
					if (canJump) rigidbody.AddForce (Vector3.up * jumpPower * 100f, ForceMode.VelocityChange);
					
					// Dampen Rough Terrain: Counters negative velocity.y forces when at certain height
					// TODO: Consider using constant minimal hover height
					if (altitude < (0.19f * hoverHeight) && rigidbody.velocity.y < 0f) {
						float dampY = Mathf.SmoothDamp (rigidbody.velocity.y, 0f, ref veloY, 0.1f);
						rigidbody.velocity = new Vector3 (rigidbody.velocity.x, dampY, rigidbody.velocity.z);	
						//Debug.Log ("Current Y-Axis Velocity = "+dampY);
					}
				}
			}
		}
		
		//Debug.Log ("Distance to Ground = "+altitude);
		//Debug.Log ("Current velocity = "+rigidbody.velocity.magnitude);
	}
	
	// Update is called once per render frame
	void Update () {
	
		// Prayer Toggrahs
		if (Input.GetKeyDown (KeyCode.G)) {
			theFinalFrontier = !theFinalFrontier;
			Debug.Log (!theFinalFrontier ? "GRAVITY ON" : "GRAVITY OFF");
		}
		if (InputManager.activeInput.GetButtonDown_SwitchCameraMode()) {	// Camera Toggle
			camFOV = Camera.main.fieldOfView;
		}
		
		// Initialize Shit
		Vector3 normalizedVelo = rigidbody.velocity.normalized;
		float currVelo = rigidbody.velocity.magnitude;
		float warpCam = Mathf.Clamp (currVelo/maxSpeed, 0f , 1f);
		activeCams = Camera.allCameras;
		angleDiff = Vector3.Angle (transform.forward, head.forward);
		angleVelo = Vector3.Angle (transform.forward, rigidbody.velocity.normalized);
		/* NOTE: 1 = LEFT, -1 = RIGHT 		Debug.Log ("Turn Signal = "+turnSig); */
		// Debug.Log ("RIGHT IS -1, LEFT IS 1; TurnSig = "+turnSig);
		turnSig = AngleDir (transform.forward, rigidbody.velocity.normalized, transform.up);
		
		// Camera Twix
		// TODO: chop off bottom of view
		if (activeCams.Length == 1) {
			activeCams[0].fieldOfView = camFOV + (camFOV*warpRatio * warpCam);
		} else if (activeCams.Length == 2) {
			activeCams[0].fieldOfView = camFOV + (camFOV*warpRatio * warpCam);
			activeCams[1].fieldOfView = camFOV + (camFOV*warpRatio * warpCam);
		} else { Debug.LogError("ERROR: Expected 1 or 2 active cameras, result = "+activeCams.Length); }
		
		// Set Us Up the Bomb for Fixed Update Physics
		// TODO: IMPLEMENT STABILIZATION TOGGLE
		throttleOn = InputManager.activeInput.GetButton_Accel() || InputManager.activeInput.GetButton_Forward();
		canJump = (IsGrounded() && InputManager.activeInput.GetButtonDown_Jump());		// Jump
		
		// Praya Moovmen n Lotashun 
		if (!InputManager.activeInput.GetButton_Look()) {
			// Deadzone and Anti-flippyfloppy Stuff
			float noBackflips = Vector3.Angle (Vector3.up, head.forward);
			float noFrontflips = Vector3.Angle (-Vector3.up, head.forward);
			if (noBackflips < 13f || noFrontflips < 13f || angleDiff < 15f ) {
				if (noBacksies == Quaternion.identity) noBacksies = head.rotation;
				transform.rotation = Quaternion.RotateTowards (transform.rotation, head.rotation, 0.1f);
				Debug.Log ("NO FLIPPYFLOPPIES ALLOWED");
			} else {
				if (noBacksies != Quaternion.identity) noBacksies = Quaternion.identity;
				transform.rotation = Quaternion.RotateTowards (transform.rotation, head.rotation, Time.deltaTime * 50f);
			}
		}
		/* TODO: UNNEEDED?
		float rotateInfluence = DeltaTime * RotationAmount * RotationScaleMultiplier;	// Camera rotation
		float deltaRotation = 0.0f;														// Rotate
		float filteredDeltaRotation = (sDeltaRotationOld * 0.0f) + (deltaRotation * 1.0f);
		YRotation += filteredDeltaRotation;
		//sDeltaRotationOld = filteredDeltaRotation;
		YRotation += OVRGamepadController.GetAxisRightX() * rotateInfluence;			// Gamepad Rotation
		//SetCameras();																	// TODO: WHY?
		*/
		
		//Debug.Log ("Current velocity = "+currVelo);
	}
	
	/* TODO: Might need OnCollision stuff from Mech scripts */
	
	/* TODO: UNCOMMENT LATER, WILL NEED FOR POWERUP IMPLEMENTATION
	void OnCollisionEnter(Collision collision) {
		if (!canJump) canJump = true;
    }
    */
    
    /* Checks to see if player is grounded */
	public bool IsGrounded() { 
		return Physics.Raycast (transform.position, -Vector3.up, distToGround + 0.2f); 
	}
    
	/* Find angle direction: Left or Right 
	 * TODO: Conditionals can be used to implement control "dead zones" */
	private float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up) {
        Vector3 perp = Vector3.Cross (fwd, targetDir);
        float dir = Vector3.Dot (perp, up);		// dot(v1, v2) = cos(theta)
        if (dir > 0.0f)  		return 1.0f;
        else if (dir < 0.0f)  	return -1.0f;
        else 					return 0.0f;
  	}
  	
	/* TODO: NOT USED, STILL NEEDED?
	private Vector3 GetAngularDirection(float angle) {
		angle /= 90;									//scaled for optimal head movement
		return Vector3.up * angle * turnSensitivity;
	}
	*/
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
}
