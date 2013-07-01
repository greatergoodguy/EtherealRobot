using UnityEngine;
using System.Collections;

public class EtherealMove : MonoBehaviour {

	// Public Tunable Movement Vars
	public float acceleration = 30.0f;
	public float lateralGs = 90.0f;
	// public float turnSensitivity = 5f;
	public float brakeSpeed = 1.0f;				// Don´t make larger than max speed
	public float maxSpeed = 90.0f;
	public float jumpPower = 5.0f;
	public float hoverHeight = 99.0f;
	public float grav = 30.0f;
	public float warpRatio = 0.5f;				// 0..1: percent of FOV increase at max speed
	public float inputDeadZone = 13.0f;			// Input deadzone in degrees from origin
	public float corneringThrustRatio = 0.5f;
	// Physics Vars
	private bool theFinalFrontier = false;
	private bool canJump, throttleOn, brakeOn = false;
	private float deltaVelo = 0f;
	private float veloY, distToGround;
	private float angleLook, angleVelo, potentAngle, turnSig;
	private Vector3 jumpForce, fullGripAngleVec;
	private Vector3 modifiedVeloAngle;			// TODO: questionable use 
	private Quaternion noBacksies;
	private Transform head;
	// Rotation Vars	
	private float deltaGs, yInt;
	public float maxVeloCornerAngle = 35.0f;	// max cornering angle at max speed (200mph)
	// Other Private Parts
	private float camFOV;
	private Camera[] activeCams;
	
	void Awake () {
		float maxAngle = 80f; 
		deltaGs = (maxVeloCornerAngle - maxAngle) / 293f;	// Interpolate angle delta from 1-292 ft/s
		yInt = 80f - deltaGs;	
	}
	
	// Use this for initialization
	void Start () {
	
		/* Rigidbody Friction Settings */
		rigidbody.constraints = 
			RigidbodyConstraints.FreezeRotationX | 
			RigidbodyConstraints.FreezeRotationY |
			RigidbodyConstraints.FreezeRotationZ;
		rigidbody.drag = 0f;
		collider.material.dynamicFriction = 0.8f;
		collider.material.dynamicFriction2 = 0.8f;
		collider.material.staticFriction = 0.8f;
		collider.material.staticFriction2 = 0.8f;
		collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
		/* Enable Rigidbody Interpolation: smooths fixed frame rate physics */
		//rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
		/* Initialize other variables */
		head = transform.Find("Head");
		transform.forward = head.forward;
		distToGround = collider.bounds.extents.y;
		camFOV = Camera.main.fieldOfView;
	}
	
	// FixedUpdate is called every fixed physics frame
	void FixedUpdate () {		
		
		// Set Gravity
		//rigidbody.useGravity = !theFinalFrontier; 
		// Set Us up the Bomb	
		float altitude = 0f;
		float currVelo = rigidbody.velocity.magnitude;
		Vector3 thrust = Vector3.zero;
		Vector3 turnVector = Vector3.zero;
		Vector3 veloVector = Vector3.zero;
		/*
		potentAngle = rigidbody.velocity.magnitude * deltaGs + yInt;
		float thrust = maxSpeed/acceleration;
		//bool understeer = angleLook > angleVelo && potentAngle > angleVelo;
		//float thrustMod = (understeer == true) ? (potentAngle-angleVelo)/potentAngle : 0f;
		Vector3 forwardThrust = head.forward * thrust;
		//Vector3 corneringThrust = (-head.right * turnSig) * thrust * thrustMod;
		Vector3 modifiedThrust = head.forward;
		*/
		//if (rigidbody.velocity.magnitude >= 0.5f)
			//modifiedThrust = rigidbody.velocity.normalized;
		//Vector3 modifiedThrust = (forwardThrust + corneringThrust).normalized; // * thrust;
		/* NOTE: Force = units/s/n, at n seconds will be at units/s velocity */
		
		// "Gravity"/Hovering and "Physics"
		if (!theFinalFrontier) {
			// TODO: Considers anything with a collider as "ground", needs work
			RaycastHit ground;
			if (Physics.Raycast (transform.position, -Vector3.up, out ground)) {
				altitude = ground.distance;
				if (altitude > hoverHeight)	{			// Exiting Hover Zone: Thrust Disabled
					// TODO: Implement steering
					rigidbody.useGravity = true;
					Vector3 pullDown = new Vector3 (0f, -rigidbody.velocity.y, 0f);
					rigidbody.AddForce (pullDown * 0.7f, ForceMode.Acceleration);
					rigidbody.AddForce (Vector3.up * -grav * Time.deltaTime, ForceMode.Acceleration);
				} else {								// Entering Hover Zone: Thrust Enabled
					rigidbody.useGravity = false;
		
					// Accel/Decel and Cornering "Physics"
					// TODO: Consider ForceMode.Velocity with analog controls using deltaTime
					// Velocity Limiter
					if (rigidbody.velocity.magnitude > maxSpeed) {
						Debug.Log ("LIMITING SPEED");
						float oppositeF = maxSpeed - rigidbody.velocity.magnitude;
						rigidbody.AddForce (rigidbody.velocity.normalized * oppositeF, ForceMode.VelocityChange);
						//rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
						//rigidbody.AddForce (veloVector, ForceMode.Acceleration);
						//modifiedThrust = (modifiedThrust + rigidbody.velocity.normalized).normalized;
						//rigidbody.AddForce (rigidbody.velocity * -1f, ForceMode.VelocityChange);
						//rigidbody.AddForce (modifiedThrust * maxSpeed, ForceMode.VelocityChange); 
					} else if (Vector3.Angle (transform.forward, rigidbody.velocity.normalized) > 90f) {
						Debug.Log ("GOING BACKWARDS");
						//rigidbody.AddForce (-rigidbody.velocity * 0.7f, ForceMode.VelocityChange);
						rigidbody.AddForce (rigidbody.velocity * -0.8f, ForceMode.VelocityChange);
					}
					// Update Velocity
					if (throttleOn) {
						Debug.Log ("GO TIME");
						//rigidbody.AddForce (modifiedThrust * thrust, ForceMode.Acceleration); 
						//deltaVelo = acceleration * Time.deltaTime;
						if (rigidbody.velocity.magnitude == 0f) {
							Debug.Log ("Starting forward momentum");
							thrust = head.forward * acceleration;
							//currVelo = thrust.magnitude;
							//turnVector = Vector3.Cross (thrust.normalized, head.forward); 
							//turnVector = Vector3.Cross (turnVector, thrust.normalized) * lateralGs;// * Time.deltaTime;
							//if (rigidbody.velocity.magnitude > 0f) currVelo = rigidbody.velocity.magnitude;
							//veloVector = (thrust + turnVector).normalized * acceleration;
							veloVector = thrust;
							//initialForce = head.forward * deltaVelo;
							//rigidbody.AddForce (head.forward * deltaVelo, ForceMode.Acceleration);
						} else {
							Debug.Log ("Adding forward force");
							//thrust = rigidbody.velocity.normalized * acceleration;
							//currVelo = rigidbody.velocity.magnitude;
							//rigidbody.AddForce (rigidbody.velocity.normalized * acceleration);
							rigidbody.AddForce (head.forward * acceleration);
							turnVector = Vector3.Cross (rigidbody.velocity.normalized, head.forward); 
							turnVector = Vector3.Cross (turnVector, head.forward) * lateralGs;// * Time.deltaTime;
							//turnVector = Vector3.Cross (turnVector, rigidbody.velocity.normalized) * lateralGs;// * Time.deltaTime;
							//if (rigidbody.velocity.magnitude > 0f) currVelo = rigidbody.velocity.magnitude;
							//veloVector = (thrust + turnVector).normalized * acceleration;
							veloVector = turnVector;
							//initialForce = rigidbody.velocity * deltaVelo;
							//rigidbody.AddForce (rigidbody.velocity.normalized * deltaVelo, ForceMode.Acceleration);
						}
						rigidbody.AddForce (veloVector, ForceMode.Acceleration);
						rigidbody.velocity = rigidbody.velocity.normalized * rigidbody.velocity.magnitude;
					} else if (brakeOn) {
						Debug.Log ("NO GO TIME");
						//deltaVelo += -acceleration * Time.deltaTime * 2.5f;
					} else { 
						Debug.Log ("SLOW GO TIME 1");
						//float v = rigidbody.velocity.magnitude;
						//thrust = rigidbody.velocity.normalized * v * 0.2f;
						//currVelo = thrust.magnitude;
						turnVector = Vector3.Cross (rigidbody.velocity.normalized, head.forward); 
						turnVector = Vector3.Cross (turnVector, head.forward) * lateralGs;// * Time.deltaTime;
						//turnVector = Vector3.Cross (turnVector, rigidbody.velocity.normalized) * lateralGs;// * Time.deltaTime;
						//if (rigidbody.velocity.magnitude > 0f) currVelo = rigidbody.velocity.magnitude;
						//veloVector = (thrust + turnVector).normalized * currVelo;
						veloVector = turnVector;
						//rigidbody.AddForce (rigidbody.velocity * -0.5f, ForceMode.VelocityChange);
						//rigidbody.AddForce (modifiedThrust * v * 0.5f, ForceMode.Acceleration); 
						//deltaVelo += -acceleration * Time.deltaTime * 0.7f;
						//deltaVelo = -deltaVelo;
						rigidbody.AddForce (veloVector, ForceMode.Acceleration);
						rigidbody.velocity = rigidbody.velocity.normalized * (currVelo * 0.95f);
						//Debug.Log ("Velocity = "+rigidbody.velocity.magnitude);
						if (rigidbody.velocity.magnitude < 0.5f) {
							rigidbody.AddForce (rigidbody.velocity * -1f, ForceMode.VelocityChange);
						}
					}
					//Debug.Log ("Current Velocity: "+rigidbody.velocity.magnitude);
					//Debug.Log ("Time.deltaTime: "+Time.deltaTime);
					//Debug.Log ("Rigidbody Velocity Vec = "+rigidbody.velocity.x+", "+rigidbody.velocity.y+", "+rigidbody.velocity.z);
					//rigidbody.AddForce (transform.TransformDirection(turnVector), ForceMode.Acceleration);
					//rigidbody.AddForce (turnVector, ForceMode.VelocityChange);
					//rigidbody.velocity = rigidbody.velocity.normalized;
					//rigidbody.velocity = rigidbody.velocity * currVelocity;
					//Debug.Log ("Rigidbody Velocity Vec = "+rigidbody.velocity.x+", "+rigidbody.velocity.y+", "+rigidbody.velocity.z);
					//veloVector = veloVector - transform.worldToLocalMatrix;		// converts the WC vector to LC vector
					//rigidbody.AddForce (-rigidbody.velocity, ForceMode.Acceleration);
					//rigidbody.AddForce (veloVector, ForceMode.Acceleration);
					
					// Jumping "Mechanic"
					if (canJump) rigidbody.AddForce (Vector3.up * jumpPower * 100f, ForceMode.VelocityChange);
					
					// Dampen Rough Terrain: Counters negative velocity.y forces when at certain height
					// TODO: Consider using constant minimal hover height
					if (altitude < (0.01f * hoverHeight) && rigidbody.velocity.y < 0f) {
						float dampY = Mathf.SmoothDamp (rigidbody.velocity.y, 0f, ref veloY, 0.01f);
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
			rigidbody.useGravity = theFinalFrontier ? false : true;
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
		/* NOTE: 1 = CW, -1 = CCW 		Debug.Log ("Turn Signal = "+turnSig); */
		// Debug.Log ("CCW IS -1, CW IS 1; TurnSig = "+turnSig);
		turnSig = AngleDir (transform.forward, rigidbody.velocity.normalized, transform.up);
		angleLook = Vector3.Angle (transform.forward, head.forward);
		angleVelo = Vector3.Angle (transform.forward, rigidbody.velocity.normalized);
		potentAngle = rigidbody.velocity.magnitude * deltaGs + yInt;
		
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
		brakeOn = false;
		
		// Moovmen n Lotashun 
		if (!InputManager.activeInput.GetButton_Look()) {
			// Deadzone and Anti-flippyfloppy Stuff
			float noBackflips = Vector3.Angle (Vector3.up, head.forward);
			float noFrontflips = Vector3.Angle (-Vector3.up, head.forward);
			//float clampOnVelo = Vector3.Angle (transform.forward, rigidbody.velocity);
			if (noBackflips < 15f || noFrontflips < 15f) {
				if (noBacksies == Quaternion.identity) noBacksies = head.rotation;
				transform.rotation = Quaternion.RotateTowards (transform.rotation, noBacksies, 0.8f);
				//transform.rotation = Quaternion.RotateTowards (transform.rotation, head.rotation, 0.1f);
				//Debug.Log ("Backflip Angle: "+noBackflips); Debug.Log ("Frontflip Angle: "+noFrontflips);
				//Debug.Log ("NO FLIPPYFLOPPIES ALLOWED");
			} else {
				if (noBacksies != Quaternion.identity) noBacksies = Quaternion.identity;
				Quaternion flattenRotation = Quaternion.Euler (transform.eulerAngles.x, transform.eulerAngles.y, 0f);
				if (angleLook < 10f) {
					transform.rotation = Quaternion.RotateTowards (transform.rotation, flattenRotation, 0.1f);
				} else if (!throttleOn && rigidbody.velocity.magnitude < 0.5f) {
					transform.rotation = Quaternion.RotateTowards (flattenRotation, head.rotation, 0.8f);
				} else {
					transform.rotation = Quaternion.RotateTowards (flattenRotation, head.rotation, 0.9f);
					//Quaternion veloQuatty = Quaternion.FromToRotation (transform.forward, rigidbody.velocity.normalized);
					//transform.rotation = Quaternion.RotateTowards (flattenRotation, veloQuatty, 0.8f);
					//Vector3 clampVeloVector = Vector3.RotateTowards (transform.forward, rigidbody.velocity.normalized, 0.8f, 0f);
					//transform.rotation = Quaternion.LookRotation(clampVeloVector);
					/*
					Vector3 bullshit = transform.forward;
					bullshit.z = 0f;
					transform.rotation = Quaternion.FromToRotation (bullshit, rigidbody.velocity.normalized);
					*/
					//transform.rotation = Quaternion.RotateTowards (flattenRotation, Quaternion.FromToRotation (bullshit, rigidbody.velocity), 0.8f);
					//Debug.Log ("Clamping to velocity vector");
				}
				//transform.rotation = Quaternion.RotateTowards (flattenRotation, head.rotation, Time.deltaTime * 50f);
				//Debug.Log ("Normal rotation");
			}
		}
		//Debug.Log ("Current velocity = "+currVelo);
			
		/* TODO: UNNEEDED?
		float rotateInfluence = DeltaTime * RotationAmount * RotationScaleMultiplier;	// Camera rotation
		float deltaRotation = 0.0f;														// Rotate
		float filteredDeltaRotation = (sDeltaRotationOld * 0.0f) + (deltaRotation * 1.0f);
		YRotation += filteredDeltaRotation;
		//sDeltaRotationOld = filteredDeltaRotation;
		YRotation += OVRGamepadController.GetAxisRightX() * rotateInfluence;			// Gamepad Rotation
		//SetCameras();																	// TODO: WHY?
		*/
	}
	
	/* TODO: Might need OnCollision stuff from Mech scripts */
	
	/* TODO: UNCOMMENT LATER, WILL NEED FOR POWERUP IMPLEMENTATION
	void OnCollisionEnter(Collision collision) {
		if (!canJump) canJump = true;
    }
    */
    
    /* Checks to see if player is grounded */
	public bool IsGrounded() { 
		return Physics.Raycast (transform.position, -Vector3.up, distToGround + 0.29f); 
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
