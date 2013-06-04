using UnityEngine;
using System.Collections;

public class MechPlayerControl : MonoBehaviour {

	public float walkAccel = 20.0f;
	public float maxWalkSpeed = 4.5f;
	public float decel = 15.0f;
	public float maxJumpAngle = 60.0f;     // hill/terrain slope mech cannot jump from
	public float rotationRateQE = 1.0f;    // y rotation rate with Q and E, degrees per frame
	public float tiltAngle = 8.0f;         // angle of tilt during y rotation
	private float tiltDamp = 0.1f;
	private float tiltSpeed;
	//public float maxYAngle = 50f;
	public bool enableQEKeys = false;
	public bool enableMouseSteer = true;
	public bool canJump = false;
	public bool grounded = false;
	private float rotationAngle;
	private float decelVeloX;
	private float decelVeloZ;
	private float zRotateCurrent;
	private float zRotateTarget;
	public Vector2 currentHorizontalVelo;  // accessed by cameral control script
	public GameObject stdCamObj;
	

	// Initialization
	void Start () {
		currentHorizontalVelo = Vector2.zero;
	}
	
	// Update is called once per frame
	void Update () {
	
		// check if speed has reached max
		currentHorizontalVelo.x = rigidbody.velocity.x;
		currentHorizontalVelo.y = rigidbody.velocity.z;
		// if greater than max speed, set to max walk speed
		if (currentHorizontalVelo.magnitude > maxWalkSpeed) {
			currentHorizontalVelo = currentHorizontalVelo.normalized; 
			currentHorizontalVelo *= maxWalkSpeed;
			rigidbody.velocity = new Vector3 (currentHorizontalVelo.x, rigidbody.velocity.y, currentHorizontalVelo.y);
		}
		
		// move/accelerate the player
		if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
			rigidbody.AddRelativeForce (Input.GetAxis("Horizontal") * walkAccel, 0f, Input.GetAxis("Vertical") * walkAccel);
		// stop/decelerate the player
		} else {
			rigidbody.velocity = new Vector3 (
				Mathf.SmoothDamp (currentHorizontalVelo.x, 0f, ref decelVeloX, decel),
				rigidbody.velocity.y, 
				Mathf.SmoothDamp (currentHorizontalVelo.y, 0f, ref decelVeloZ, decel));
		}
		// TODO: implement lumbering movement with QE steering when at a standstill
		// rotates player with Q and E
		if (enableQEKeys) {
			enableMouseSteer = false;
			if (Input.GetKey(KeyCode.Q)) {
				rotationAngle -= rotationRateQE;
				zRotateTarget = tiltAngle;
			} else if (Input.GetKey(KeyCode.E)) {
				rotationAngle += rotationRateQE;
				zRotateTarget = -tiltAngle;
			} else {
				zRotateTarget = 0;
			}
			// TODO: might need to handle overflow for zoolander edge case
			//rotationAngle = Mathf.Clamp(rotationAngle, -maxYAngle, maxYAngle);
		}
		
		// TODO: Find out if designers want option for QE keys in mouse steering mode.
		// They better not because that's fucking stupid. Whoever says 'yes' better have a damn good reason. 
		// Control Oculus switch with one boolean if designers are smart.
		
		// Mouse Mode: rotates the player model with mouse/camera movement
		if (enableMouseSteer) {
			enableQEKeys = false;
			transform.rotation = Quaternion.Euler (
				0.0f, 
				stdCamObj.GetComponent<MechCameraControl>().yRotateCurrent + rotationAngle,
				0.0f);
		// OCULUS MODE, MOTHERFUCKERS!
		} else {
			// TODO: finish lumbering rotation around the z-axis in Oculus Mode
			zRotateCurrent = Mathf.SmoothDamp (zRotateCurrent, zRotateTarget, ref tiltSpeed, tiltDamp);
			transform.rotation = Quaternion.Euler (0.0f, rotationAngle,	zRotateCurrent);		
		}
	}

	// detects if player is in contact with the ground
	void OnCollisionEnter (Collision collisionInfo) {
	
		// TODO: consider adding a non-rigidbody obj to bottom of mech for grounded buffer
		
		string objName = collisionInfo.gameObject.name;
		if (objName.Equals("Plane") || objName.Equals("HillyTerrain")) grounded = true;
		Debug.Log ("OnCollisionEnter object collided with is: " + objName);
	}
	
	// detects if ground player is sloped low enough for jumping
	void OnCollisionStay (Collision collisionInfo) {
	
		foreach (ContactPoint contact in collisionInfo.contacts) {
			if (Vector3.Angle(contact.normal, Vector3.up) < maxJumpAngle) canJump = true;
		}
		//Debug.Log ("OnCollisionStay object collided with is: " + collisionInfo.gameObject.name);
	}
	
	// detects if player is in the air
	void OnCollisionExit () {
	
		grounded = canJump = false;
		Debug.Log ("I BELIEVE I CAN FLY!");
	}
}
