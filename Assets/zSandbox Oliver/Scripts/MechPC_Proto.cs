using UnityEngine;
using System.Collections;

public class MechPC_Proto : MonoBehaviour {

	public float walkAccel = 20.0f;
	public float maxWalkSpeed = 4.5f;
	public float decel = 15.0f;
	public float maxJumpAngle = 60.0f;		// Slope of hill/terrain where mech cannot jump
	public float rotationRateQE = 1.0f;		// Y rotation rate with Q and E, degrees per frame
	public float tiltAngle = 7.0f;			// Angle of tilt with Q and E while walking
	public float tiltDamp = 0.5f;			// Number of seconds to reach/return from max tilt
	private float tiltSpeed;
	public bool oculusMode = false;
	public bool canJump = false;
	public bool grounded = false;
	private bool isMoving = false;
	private bool isTurning = false;
	private float rotationAngle;
	private float decelVeloX;
	private float decelVeloZ;
	private float zRotateCurrent;
	private float zRotateTarget;
	private Vector3 stdCamRot;
	
	/* TODO: Implement abstract the control class and have subclass static variables for:
	 * 1. oculusMode  
	 * 2. isMoving
	 * 3. isTurning  
	 * 4. isGrounded  
	 * 5. canJump
	 * 5. currVelo & currHoriVelo 
	 */
	public Vector2 currHoriVelo;			// REFACTOR, currently accessed by cam control script
	public GameObject stdCamObj;			// Janky fix, refactor and remove
	private GameObject standardCameraGO;
	private GameObject oculusCameraGO;
	

	// Initialization
	void Start () {
		currHoriVelo = Vector2.zero;
		standardCameraGO = transform.FindChild("Head").FindChild("Camera").gameObject;
		oculusCameraGO = transform.FindChild("Head").FindChild("OVRCameraController").gameObject;
		
		standardCameraGO.SetActive(true);
		oculusCameraGO.SetActive(false);
	}
	
	// Last update
	void LateUpdate () {
		if (InputManager.activeInput.GetButtonDown_SwitchCameraMode())
			SwitchCameraController();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		grounded = isGrounded();
		//if (InputManager.activeInput.GetButtonDown_SwitchCameraMode())
		//	SwitchCameraController();
		
		/* TODO: JANKY, REFACTOR AND REMOVE THESE TWO VARS */
		isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
		isTurning = Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E); 
		
		// Check if speed has reached max
		/* TODO: JANKY, REFACTOR AND REMOVE THESE THREE VARS */
		currHoriVelo.x = rigidbody.velocity.x;
		currHoriVelo.y = rigidbody.velocity.z;
		float currVelo = currHoriVelo.magnitude;
		// If greater than max speed, set to max walk speed
		if (currVelo > maxWalkSpeed) {
			currHoriVelo = currHoriVelo.normalized; 
			currHoriVelo *= maxWalkSpeed;
			rigidbody.velocity = new Vector3 (currHoriVelo.x, rigidbody.velocity.y, currHoriVelo.y);
		}
		
		/* DEPRECATED MOVEMENT IMPLEMENTATION: stopped working after a merge, switched to key based detection
		if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
			rigidbody.AddRelativeForce (Input.GetAxis("Horizontal") * walkAccel, 0f, Input.GetAxis("Vertical") * walkAccel);  */
			
		// Move/accelerate the player
		if (isMoving) {
			if (Input.GetKey(KeyCode.W)) rigidbody.AddRelativeForce (0f, 0f, 1f * walkAccel);
			if (Input.GetKey(KeyCode.S)) rigidbody.AddRelativeForce (0f, 0f, -1f * walkAccel);
			if (Input.GetKey(KeyCode.A)) rigidbody.AddRelativeForce (-1f * walkAccel, 0f, 0f);
			if (Input.GetKey(KeyCode.D)) rigidbody.AddRelativeForce (1f * walkAccel, 0f, 0f); 
		// Stop/decelerate the player
		} else {
			rigidbody.velocity = new Vector3 (
				Mathf.SmoothDamp (currHoriVelo.x, 0f, ref decelVeloX, decel),
				rigidbody.velocity.y, 
				Mathf.SmoothDamp (currHoriVelo.y, 0f, ref decelVeloZ, decel));
		}
		
		// OCULUS MODE: Rotates player with Q and E
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
			// TODO: Might need to handle rotationAngle overflow for zoolander edge case
			
			// Adds appropriate tilt angle about the z-axis if moving or turning
			if (isMoving && isTurning)
				zRotateCurrent = Mathf.SmoothDamp (zRotateCurrent, zRotateTarget, ref tiltSpeed, tiltDamp);
			// Apply rotations to mech
			transform.rotation = Quaternion.Euler (0.0f, rotationAngle,	zRotateCurrent);		
			
		// MOUSE MODE: Rotates the player model with mouse/camera movement
		} else {
			transform.rotation = Quaternion.Euler (
				0.0f, 
				stdCamObj.GetComponent<MechCC_Proto>().yRotateCurrent,					// JANKY
				0.0f);
		}
	}
	
	void SwitchCameraController () {
		Vector3 updatePosition = transform.localPosition;
		float updateRift = stdCamObj.GetComponent<MechCC_Proto>().yRotateCurrent; 
		float updateStd = rotationAngle;
		
		oculusMode = !oculusCameraGO.activeInHierarchy;
		standardCameraGO.SetActive(!oculusMode);
		oculusCameraGO.SetActive(oculusMode);	
		
		transform.localPosition = updatePosition;
		
		if (oculusMode) {
			rotationAngle = updateRift;
			Debug.Log ("Updating Oculus: yRotate = "+updateRift);
		} else {
			stdCamObj.GetComponent<MechCC_Proto>().yRotateCurrent = updateStd;
			stdCamObj.GetComponent<MechCC_Proto>().yRotateTarget = updateStd;
			Debug.Log ("Updating standard cam: yRotate = "+updateStd);
		}
	}	
	
	bool isGrounded () {
		float distToGround = collider.bounds.extents.y;
  		return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.2f);
	}
	
	
	/* TODO: MAKE THESE LAST THREE METHODS STATIC CLASS METHODS */
	// Detects if player is in contact with the ground
	void OnCollisionEnter (Collision collisionInfo) {
	
		// TODO: consider adding a non-rigidbody obj to bottom of mech for grounded buffer
		string objName = collisionInfo.gameObject.name;
		// if (objName.Equals("Plane") || objName.Equals("HillyTerrain")) grounded = true;
		Debug.Log ("OnCollisionEnter object collided with is: " + objName);
	}
	// Detects if ground player is sloped low enough for jumping
	void OnCollisionStay (Collision collisionInfo) {
	
		foreach (ContactPoint contact in collisionInfo.contacts) {
			if (Vector3.Angle(contact.normal, Vector3.up) < maxJumpAngle) canJump = true;
		}
		//Debug.Log ("OnCollisionStay object collided with is: " + collisionInfo.gameObject.name);
	}
	// Detects if player is in the air
	void OnCollisionExit (Collision collisionInfo) {
	
		string objName = collisionInfo.gameObject.name;
		if (objName.Equals("Plane") || objName.Equals("HillyTerrain")) {
			grounded = canJump = false;
			Debug.Log ("I BELIEVE I CAN FLY!");
		}
	}
}
