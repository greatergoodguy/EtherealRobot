using UnityEngine;
using System.Collections;

public class MechPlayerControl : MonoBehaviour {

	public float walkAccel = 20.0f;
	public float maxWalkSpeed = 4.5f;
	public float decel = 15.0f;
	public float maxJumpAngle = 60.0f;		// hill/terrain slope mech cannot jump from
	public float rotationRateQE = 1.0f;		// y rotation rate with Q and E, degrees per frame
	public float tiltAngle = 8.0f;			// angle of tilt during y rotation in Oculus mode
	public float tiltDamp = 0.5f;			// amount of time it takes to reach max tilt angle
	private float tiltSpeed;
	//public float maxYAngle = 50f;
	public bool oculusMode = false;
	public bool canJump = false;
	public bool grounded = false;
	private bool isMoving = false;
	private bool isTurning = false;
	private float rotationAngle;
	private float decelVeloX;
	private float decelVeloZ;
	private float zRotateCounter;
	private float zRotateCurrent;
	private float zRotateTarget;
	public Vector2 currentHorizontalVelo;	// accessed by cameral control script
	public GameObject stdCamObj;
	

	// Initialization
	void Start () {
		currentHorizontalVelo = Vector2.zero;
	}
	
	// Update is called once per frame
	void Update () {
	
		isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
		isTurning = Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E); 
		
		// check if speed has reached max
		currentHorizontalVelo.x = rigidbody.velocity.x;
		currentHorizontalVelo.y = rigidbody.velocity.z;
		float currVelo = currentHorizontalVelo.magnitude;
		// if greater than max speed, set to max walk speed
		if (currVelo > maxWalkSpeed) {
			currentHorizontalVelo = currentHorizontalVelo.normalized; 
			currentHorizontalVelo *= maxWalkSpeed;
			rigidbody.velocity = new Vector3 (currentHorizontalVelo.x, rigidbody.velocity.y, currentHorizontalVelo.y);
		}
		
		/* DEPRECATED MOVEMENT IMPLEMENTATION: stopped working after a merge, switched to key based detection
		if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
			rigidbody.AddRelativeForce (Input.GetAxis("Horizontal") * walkAccel, 0f, Input.GetAxis("Vertical") * walkAccel);  */
			
		// move/accelerate the player
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) { 
			if (Input.GetKey(KeyCode.W)) rigidbody.AddRelativeForce (0f, 0f, 1f * walkAccel);
			if (Input.GetKey(KeyCode.S)) rigidbody.AddRelativeForce (0f, 0f, -1f * walkAccel);
			if (Input.GetKey(KeyCode.A)) rigidbody.AddRelativeForce (-1f * walkAccel, 0f, 0f);
			if (Input.GetKey(KeyCode.D)) rigidbody.AddRelativeForce (1f * walkAccel, 0f, 0f); 
		// stop/decelerate the player
		} else {
			rigidbody.velocity = new Vector3 (
				Mathf.SmoothDamp (currentHorizontalVelo.x, 0f, ref decelVeloX, decel),
				rigidbody.velocity.y, 
				Mathf.SmoothDamp (currentHorizontalVelo.y, 0f, ref decelVeloZ, decel));
		}
		
		// OCULUS MODE: rotates player with Q and E
		if (oculusMode) {
			if (Input.GetKey(KeyCode.Q)) {
				rotationAngle -= rotationRateQE;
				if (isMoving) zRotateTarget = tiltAngle;
				else zRotateTarget = 0;
			} else if (Input.GetKey(KeyCode.E)) {
				rotationAngle += rotationRateQE;
				if (isMoving) zRotateTarget = -tiltAngle;
				else zRotateTarget = 0;
			} else {
				zRotateTarget = 0;
			}
			// TODO: might need to handle rotationAngle overflow for zoolander edge case
			
			// adds appropriate tilt angle about the z-axis if moving or turning
			if (isMoving || isTurning)
				zRotateCurrent = Mathf.SmoothDamp (zRotateCurrent, zRotateTarget, ref tiltSpeed, tiltDamp);
			// apply rotations to mech
			transform.rotation = Quaternion.Euler (0.0f, rotationAngle,	zRotateCurrent);		
			
		// MOUSE MODE: rotates the player model with mouse/camera movement
		} else {
			transform.rotation = Quaternion.Euler (
				0.0f, 
				stdCamObj.GetComponent<MechCameraControl>().yRotateCurrent + rotationAngle,
				0.0f);
		}
	}
	
	
	/* TODO: MAKE THESE LAST THREE METHODS STATIC CLASS METHODS */
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
	void OnCollisionExit (Collision collisionInfo) {
	
		string objName = collisionInfo.gameObject.name;
		if (objName.Equals("Plane") || objName.Equals("HillyTerrain")) {
			grounded = canJump = false;
			Debug.Log ("I BELIEVE I CAN FLY!");
		}
	}
}
