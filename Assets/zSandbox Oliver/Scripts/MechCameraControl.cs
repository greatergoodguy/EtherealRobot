using UnityEngine;
using System.Collections;

public class MechCameraControl : MonoBehaviour {

	// class variables for looking
	public bool invertedLook = false;
	public float lookSensitivity = 2.0f;
	public float maxAngleY = 50f;       // only applies when mouse steering is disabled
	private bool oculusMode;
	private float lookDamp = 0.1f;
	private float xRotateTarget;
	private float yRotateTarget;
	private float zRotateTarget;
	private float xRotateCurrent;
	public float yRotateCurrent;        // is accessed by player control script
	private float zRotateCurrent = 0.0f;
	private float xRotateSpeed;
	private float yRotateSpeed;
	private float zRotateSpeed;
	
	// class variables for look tilting
	public float tiltAngle = 8f;
	public float tiltSensitivity = 0.5f;
	public float tiltSpeed = 100f;
	private float tiltDamp = 0.1f;
	private float lookDirection;
	
	// class variables for head bob control
	public float bobFrequency = 0.4f;	// 0..1 - Adjusts stride
	public float horizontalBob = 0.45f;	// 0..1 - Adjusts amount of horizontal bob
	public float verticalBob = 0.35f;	// 0..1 - Adjusts amount of vertical bob
	
	/* NOTE: Tilt angle settings are effected by horizontal and vertical bob settings. 
	 * Actual tilt angles are approximatly tiltAngleX * verticalBob, etc. */
	 
	public float tiltAngleX = 8f;       // Adjusts up/down tilt during bob
	public float tiltAngleZ = 9f;       // Adjusts left/right tilt during bob
	public float jerk = 0.25f;	        // 0..1 - Adjust jerky-ness of movement, 1 is smoothest
	public float resetDamp = 0.15f;	    // Amount of time it takes to reset position
	// public float climbBobFreq = 0.1f;  // Adjusts stride during a slow climb
	public float resetBobFreq = 0.05f;  // Adjusts speed of position reset after walking
	private float counter = 0.0f;
	private float xMovement;
	private float yMovement;
	private float distance;
	
	public GameObject mech;
	
	
	// Awake is always called 
	//void Awake () {
	//}
			
	// Initialization
	//void Start () {
	//	zRotateCurrent = transform.localRotation.z;
	//}
	
	// Update is called once per frame
	void Update () {
	
		// TODO: maybe consider using a message rather than pinging parent each frame
		oculusMode = mech.GetComponent<MechPlayerControl>().oculusMode;
		
		mouseLook();
		headBob();
					
		// Apply headbob translations camera's position relative to the parent
		transform.localPosition = new Vector3 (xMovement, yMovement, transform.localPosition.z);
		// TODO: ?more efficient to translate the existing transform matrix than newing up one and assigning?
		// transform.Translate(current sine pos - previous, current sine pos - previous, 0f);	
		
		// Apply headbob and mouselook rotations to camera
		if (!oculusMode) {
			transform.rotation = Quaternion.Euler (
				xRotateCurrent + (yMovement * tiltAngleX), 
				yRotateCurrent, 
				zRotateCurrent + (xMovement * tiltAngleZ));
		} else {
			transform.localRotation = Quaternion.Euler (
				xRotateCurrent + (yMovement * tiltAngleX), 
				yRotateCurrent, 
				zRotateCurrent + (xMovement * tiltAngleZ));		
		}
	}
	

	// Mouselook code that does stuff for looking with mouse
	void mouseLook () {
		
		// Get the target rotation based on mouse input
		float mouseXLoc = Input.GetAxis ("Mouse X");
		yRotateTarget += mouseXLoc * lookSensitivity;
		
		if (invertedLook)
			xRotateTarget += Input.GetAxis ("Mouse Y") * lookSensitivity;
		else
			xRotateTarget -= Input.GetAxis ("Mouse Y") * lookSensitivity;
		// Clamps xRotation to prevent head flipping
		xRotateTarget = Mathf.Clamp (xRotateTarget, -90f, 90f);
	
		// Find if looking left or right and magnitude of direction change
		lookDirection = Mathf.Lerp (lookDirection, mouseXLoc, tiltSpeed * Time.deltaTime); 

		// CLASSIC MODE: Mouse Steering enabled
		if (!oculusMode) {
			if (lookDirection > tiltSensitivity) {  // looking right
				zRotateTarget = -tiltAngle;
			} else if (lookDirection < -tiltSensitivity) {  // looking left
				zRotateTarget = tiltAngle;
			} else { 
				zRotateTarget = 0.0f;
			}
		// OCULUS MODE: Mouse Steering disabled
		} else {
			yRotateTarget = Mathf.Clamp (yRotateTarget, -maxAngleY, maxAngleY);
		}

		// Smooth out movement
		// TODO: ?faster using a Vector3?
		yRotateCurrent = Mathf.SmoothDamp (yRotateCurrent, yRotateTarget, ref yRotateSpeed, lookDamp);
		xRotateCurrent = Mathf.SmoothDamp (xRotateCurrent, xRotateTarget, ref xRotateSpeed, lookDamp);
		zRotateCurrent = Mathf.SmoothDamp (zRotateCurrent, zRotateTarget, ref zRotateSpeed, tiltDamp);
		// Debug.Log ("xRotateSpeed = "+xRotateSpeed+"\nyRotateSpeed = "+yRotateSpeed+"\nzRotateSpeed = "+zRotateSpeed);
		// Debug.Log ("xRotateTarget = "+xRotateTarget+"\nyRotateTarget = "+yRotateTarget+"\nzRotateTarget = "+zRotateTarget);
		// Debug.Log ("xRotateCurrent = "+xRotateCurrent+"\nyRotateCurrent = "+yRotateCurrent+"\nzRotateCurrent = "+zRotateCurrent);
	}
	
	// Headbob method bobs head as long as mech is moving
	void headBob () {
	
		// If player is grounded, then WHIP YO HEAD BACK N FORTH WHIP YO HEAD BACK N FORTH
		if (mech.GetComponent<MechPlayerControl>().grounded) {
			// Keep counter between 0 and 2PI
			// (counter will most likely never overflow, but just incase..)
			if (Mathf.Approximately (counter, 2*Mathf.PI) || counter > 2*Mathf.PI) 
				counter = 0.0f;
				
			// mech is trekking about
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || 
				Input.GetKey(KeyCode.A) || Input.GetKey (KeyCode.D)) {
				/*
				 * TODO: adapt movement for difficult terrain, use force and velocity
				// mech traversing difficult terrain
				if (exerting abnormal force relative to speed) {
					counter += climbBobFreq;
					Debug.Log ("Mech Climbing");
				}
				*/
				float currVelo = mech.GetComponent<MechPlayerControl>().currentHorizontalVelo.magnitude;
				counter += Mathf.PI * currVelo/80.0f * bobFrequency;
				// Debug.Log ("Trek Velocity = "+currVelo);
			}
			// mech not moving or moving very slowly
			else if (xMovement > 0.01f || xMovement < -0.01f) {
				// mech not moving, finish step animation
				counter += resetBobFreq;
				// Debug.Log ("Mech Stationary and Finishing Step");
			}
			Mathf.Clamp (counter, 0f, 2*Mathf.PI);
			// Using sine function for head bob
			// NOTE: sin (x*2) ** 0.5 might also work for vertical head movement
			xMovement = Mathf.Sin (counter) * horizontalBob;
			yMovement = Mathf.Sin (counter * 2) * verticalBob;
			// Flattens out and jerks downward head movement
			float yThreshold = -verticalBob * jerk;
			if (yMovement < yThreshold) {
				yMovement = yThreshold * jerk/4;
			}
		}
	}
	
	/* CAN MAYBE PERHAPS PROBABLY MOST LIKELY IN THE FUTURE NOT NEED THIS ANYMORE
	// Detects movement/velocity of mech
	bool isStationary () {
	
		return (mech.rigidbody.velocity.x < 0.5f && 
				mech.rigidbody.velocity.y < 0.5f &&
				mech.rigidbody.velocity.z < 0.5f);
	}
	*/
	
	/* DEPREMECATED MAYBE
	// Resets head position when stationary
	void resetPosition () {
	
		distance = 0.0f;
		//Debug.Log ("BEFORE DAMP: xMovement = "+xMovement+", yMovement = "+yMovement);
		xMovement = Mathf.SmoothDamp (xMovement, 0.0f, ref xHeadVelo, resetDamp);
		yMovement = Mathf.SmoothDamp (yMovement, 0.0f, ref yHeadVelo, resetDamp);
		//Debug.Log ("AFTER DAMP: xMovement = "+xMovement+", yMovement = "+yMovement);
		//Debug.Log ("xHeadVelo = "+xHeadVelo+", yHeadVelo = "+yHeadVelo);
	}
	*/
}
