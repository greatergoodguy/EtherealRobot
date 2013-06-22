using UnityEngine;
using System.Collections;

public class EtherealMove : MonoBehaviour {

	// Public Tunable Movement Vars
	public float turnSpeed = 1.0f;
	public float turnSensitivity = 5f;
	public float acceleration = 2.5f;
	public float brakeSpeed = 1.0f;				// Don´t make larger than max speed
	public float maxSpeed = 88.0f;
	public float jumpPower = 5.0f;
	public float hoverHeight = 20.0f;
	public float grav = 30.0f;
	public float camWarpRatio = 0.5f;			// 0..1: percent of FOV increase at max speed
	public float inputDeadZone = 3.0f;			// Input deadzone in degrees from origin
	// Physics Vars
	private float increaseRate = 1f;
	private bool testBool = false;
	private bool inHoverZone = false;
	private bool killGrav = true;
	private bool canJump = false;
	private bool inputThrottleOn = false;
	private float thrust;
	private float currVelo;
	private float distToGround;
	private float camFOV;
	private Transform head;
	private Vector3 forwardForce;
	private Vector3 forwardThrust;
	private Vector3 normalizedVelo;
	private Vector3 jumpForce;
	private Vector3 modifiedVeloAngle;
	private float veloY;	
	// Rotation Vars	
	public float lookAngleX, lookAngleY;
	private float deltaGs;
	private float yInt;
	private Vector3 cubeForward;
	private Vector3 sphereForward;
	private Vector3 lastSphereFwd;	
	private Vector3 sphereAng;
	private Vector3 crossProd;
	private Vector3 vecVelo;
	private float currAngX, veloAngX = 0f;
	private float currAngY, veloAngY = 0f;
	public float maxVeloCornerAngle = 35.0f;	// max cornering angle at max speed (200mph)
	private float temp = 0f;
	private float veloTest;
	// Other Private Parts
	private Camera[] activeCams;
	
	
	void Awake () {
		float maxAngle = 80f; 
		deltaGs = (maxVeloCornerAngle - maxAngle) / 293f;	// Interpolate angle delta from 1-292 ft/s
		yInt = 80f - deltaGs;	
	}
	
	// Use this for initialization
	void Start () {
	
		/* Rigidbody Friction Settings */
		rigidbody.freezeRotation = true;
		collider.material.dynamicFriction = 1.0f;
		collider.material.dynamicFriction2 = 1.0f;
		collider.material.staticFriction = 1.0f;
		collider.material.staticFriction2 = 1.0f;
		collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
		/* Enable Rigidbody Interpolation: smooths fixed frame rate physics */
		rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
		/* Initialize other variables */
		forwardThrust = Vector3.zero;
		head = transform.Find("Head");
		transform.forward = head.forward;
		distToGround = collider.bounds.extents.y;
		camFOV = Camera.main.fieldOfView;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown (KeyCode.G)) {
			killGrav = !killGrav;
			Debug.Log ("Toggling Gravity");
		}
		if (InputManager.activeInput.GetButtonDown_SwitchCameraMode()) {	// Camera Toggle
			camFOV = Camera.main.fieldOfView;
		}
		
		thrust = maxSpeed/acceleration;
		activeCams = Camera.allCameras;
		float warpCam = (currVelo/maxSpeed > 1f) ? 1f : currVelo/maxSpeed;
		if (activeCams.Length == 1) {
			activeCams[0].fieldOfView = camFOV + (camFOV*0.5f * warpCam);
		} else if (activeCams.Length == 2) {
			activeCams[0].fieldOfView = camFOV + (camFOV*0.5f * warpCam);
			activeCams[1].fieldOfView = camFOV + (camFOV*0.5f * warpCam);
		} else { Debug.LogError("ERROR: Expected 1 or 2 active cameras, result = "+activeCams.Length); }
		//Camera.main.fieldOfView = camFOV + (camFOV*0.5f * warpCam);
		
		/* NOTE: Force = units/s/n, at n seconds will be at units/s velocity */
		if (InputManager.activeInput.GetButton_Accel() ||		
			InputManager.activeInput.GetButton_Forward()) {		// Forward Acceleration Mechanic
			/* TODO: calculate values based on deltaTime */
			// TODO: switch to ForceMode.Velocity with analog controls
			forwardThrust = head.forward * thrust;
		} else {												// Brake Mechanic 
			forwardThrust = rigidbody.velocity * -1.0f;			// ver.0: rigidbody.velocity * -brakeSpeed
		}
		//Debug.Log ("Current velocity = "+currVelo);
		
		if (canJump && IsGrounded()) {							// Jump Mechanic
			if (InputManager.activeInput.GetButtonDown_Jump()) {
			 	jumpForce = Vector3.up * jumpPower * 100;
				canJump = false;
			}
		} else { jumpForce = Vector3.zero; }
		
		if ((lookAngleY > inputDeadZone || lookAngleX > inputDeadZone) && 			// Cornering Mechanics
			!InputManager.activeInput.GetButton_Look()) {
			modifiedVeloAngle = (normalizedVelo + head.forward.normalized) * currVelo;
		} else { modifiedVeloAngle = rigidbody.velocity; }
		
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
	
	// FixedUpdate is called every fixed physics framerate frame
	void FixedUpdate () {		
		
		cubeForward = transform.forward;
		sphereForward = head.forward;
		normalizedVelo = rigidbody.velocity.normalized;
		currVelo = rigidbody.velocity.magnitude;
		float absoluteAngle, angleDirY, angleDirX = 0f;
		if (currVelo < 99f) {									// Body Fwd to Head Fwd Angle
			Vector3 sphereAngY = new Vector3 (sphereForward.x, cubeForward.y, sphereForward.z);
			Vector3 sphereAngX = new Vector3 (cubeForward.x, sphereForward.y, sphereForward.z);
			absoluteAngle = Vector3.Angle (cubeForward, sphereAngY);
			//Debug.Log ("Angle = "+absoluteAngle);
			//lookAngleY = absoluteAngle * AngleDir (cubeForward, sphereAngY, transform.up);
			lookAngleY += absoluteAngle * AngleDir (cubeForward, sphereAngY, transform.up);
			absoluteAngle = Vector3.Angle (cubeForward, sphereAngX);
			//lookAngleX = absoluteAngle * AngleDir (cubeForward, sphereAngX, transform.right);
			lookAngleX += absoluteAngle * AngleDir (cubeForward, sphereAngX, transform.right);
		} else {												// Body Fwd to Velo Fwd Angle, keeps velo vector in frame
			float maxLook = currVelo * deltaGs + yInt;
			//Debug.Log ("maxLook = "+maxLook);
			Vector3 sphereAngY = new Vector3 (sphereForward.x, normalizedVelo.y, sphereForward.z);	
			Vector3	sphereAngX = new Vector3 (normalizedVelo.x, sphereForward.y, sphereForward.z);	
			absoluteAngle = Vector3.Angle (normalizedVelo, sphereAngY);
			//Debug.Log ("absoluteAngleY = "+absoluteAngle);
			angleDirY = AngleDir (normalizedVelo, sphereAngY, transform.up);
			lookAngleY = (absoluteAngle > maxLook) ? maxLook*angleDirY : absoluteAngle*angleDirY;
			absoluteAngle = Vector3.Angle (normalizedVelo, sphereAngX);
			//Debug.Log ("absoluteAngleX = "+absoluteAngle);
			angleDirX = AngleDir (normalizedVelo, sphereAngX, transform.right);
			lookAngleX = (absoluteAngle > maxLook) ? maxLook*angleDirX : absoluteAngle*angleDirX;
		}
		/* TODO: Extract lookAngle out of parent
		 * is currently static class float var inherited from parent */
		 
		// Steering Mechanics: rotates player head and body
		lookAngleY = Mathf.Clamp (lookAngleY, -80f, 80f);
		lookAngleX = Mathf.Clamp (lookAngleX, -80f, 80f);
		currAngY = Mathf.SmoothDamp (currAngY, lookAngleY, ref veloAngY, turnSensitivity);
		currAngX = Mathf.SmoothDamp (currAngX, lookAngleX, ref veloAngX, turnSensitivity);
		//currAngY += Mathf.SmoothDamp (0f, lookAngleY, ref veloAngY, Time.deltaTime * turnSensitivity);
		//currAngX += Mathf.SmoothDamp (0f, lookAngleX, ref veloAngX, Time.deltaTime * turnSensitivity);
		//currAngX = Mathf.Clamp (currAngX, -80f, 80f);
		if (!InputManager.activeInput.GetButton_Look()) {
			//transform.Rotate (0f, currAngY, 0f);
			//transform.Rotate (currAngX, 0f, 0f);
			//transform.rotation = Quaternion.Euler (currAngX, 0f, 0f);
			//transform.rotation = Quaternion.Euler (0f, currAngY, 0f);
			sphereForward.x = 0f;
			//Debug.Log ("Look angle = "+Vector3.Angle (Vector3.up, sphereForward));
			float yClamp = Vector3.Angle (Vector3.up, sphereForward);
			if (yClamp <= 10f) {
				if (temp == 0f) temp = currAngX;
				transform.localRotation = Quaternion.Euler (-currAngX, transform.localRotation.y, 0f);
				transform.rotation = Quaternion.Euler (temp, currAngY, 0f);
			} else {
				temp = 0f;
				transform.rotation = Quaternion.Euler (currAngX, currAngY, 0f);
			}
			
			//transform.eulerAngles = new Vector3 (currAngX, currAngY, 0f);
		}
		//Debug.Log ("lookAngleX = "+lookAngleX); 
		//Debug.Log ("lookAngleY = "+lookAngleY);
		//Debug.Log ("Current AngleX = "+currAngX); Debug.Log ("Current AngleY = "+currAngY);	
		
		/* Clamp Rotation to Head
		transform.rotation = Quaternion.Euler (
			head.GetComponent<MouseLook_Ethereal>().xRotateCurrent,
			head.GetComponent<MouseLook_Ethereal>().yRotateCurrent,
			0.0f);
		*/
		
		// Set Gravity
		rigidbody.useGravity = !killGrav; 
		// Accel/Decel
		rigidbody.AddForce(forwardThrust, ForceMode.Acceleration);
		// Velocity Limiter
		if (currVelo > maxSpeed) {
			rigidbody.velocity = normalizedVelo * maxSpeed;
		} 
		// Cornering
		/* TODO: 
		 * if (isCornering) then modify the velocity vectoŕ angle
		 */
		 
		/*
		rigidbody.velocity = modifiedVeloAngle;		// EDIT: SHOULD CHANGE PER PHYSICS STEP..
		// Jumping Mechanic
		rigidbody.AddForce(jumpForce);
		// Realistic Gravity Compensation & 'Hovering' Mechanic
		if (!killGrav) {
			if (!IsGrounded () && rigidbody.useGravity) {
				rigidbody.AddForce(Vector3.up * -grav);
			}
			RaycastHit ground;
			if (Physics.Raycast (transform.position, -Vector3.up, out ground)) {
				float altitude = ground.distance;
				if (altitude > hoverHeight)	{ 
					rigidbody.useGravity = true;
				} else { 
					rigidbody.useGravity = false;
					// Dampen rough terrain, add downward negative force based on velocity.y
					if (altitude < 0.20f*hoverHeight && rigidbody.velocity.y < 0f) {
						float dampY = Mathf.SmoothDamp (rigidbody.velocity.y, 0f, ref veloY, 0.1f);
						rigidbody.velocity = new Vector3 (rigidbody.velocity.x, dampY, rigidbody.velocity.z);	
						//Debug.Log ("Current Y Velocity = "+dampY);
						//Debug.Log ("WHAT IN THE GODDAMN FUCK");
					}
				}
				//Debug.Log ("Distance to Ground = "+altitude);
			}
		}
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
    
	/* TODO: NOT USED, STILL NEEDED?
	private Vector3 GetAngularDirection(float angle) {
		angle /= 90;									//scaled for optimal head movement
		return Vector3.up * angle * turnSensitivity;
	}
	*/
	
	/* Find angle direction: Left or Right 
	 * TODO: Conditionals can be used to implement control "dead zones" */
	private float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up) {
        Vector3 perp = Vector3.Cross (fwd, targetDir);
        float dir = Vector3.Dot (perp, up);		// dot(v1, v2) = cos(theta)
        if (dir > 0.0f)  		return 1.0f;
        else if (dir < 0.0f)  	return -1.0f;
        else 					return 0.0f; 
  }
}
