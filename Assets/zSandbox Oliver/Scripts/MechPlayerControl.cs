using UnityEngine;
using System.Collections;

public class MechPlayerControl : MonoBehaviour {

	public float walkAccel = 20.0f;
	public float maxWalkSpeed = 4.5f;
	public float deaccel = 15.0f;
	public float maxJumpAngle = 60.0f;
	public bool canJump = false;
	public bool grounded = false;
	private float deaccelVeloX;
	private float deaccelVeloZ;
	private Vector2 currentHorizontalVelo;
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
		// stop/deaccelerate the player
		} else {
			rigidbody.velocity = new Vector3 (
				Mathf.SmoothDamp (currentHorizontalVelo.x, 0f, ref deaccelVeloX, deaccel),
				rigidbody.velocity.y, 
				Mathf.SmoothDamp (currentHorizontalVelo.y, 0f, ref deaccelVeloZ, deaccel));
		}
		// rotates the player model with mouse movement
		transform.rotation = Quaternion.Euler (0, stdCamObj.GetComponent<MechCameraControl>().yRotateCurrent, 0);
		// transform.rotation = Quaternion.Euler (0, stdCamObj.GetComponent<MechMouseLook>().yRotateCurrent, 0);
	}

	// detects if player is in contact with the ground
	void OnCollisionEnter (Collision collisionInfo) {
	
		// TODO: consider adding a non-rigidbody obj to bottom of mech for grounded buffer
		
		string objName = collisionInfo.gameObject.name;
		if (objName.Equals("Plane") || objName.Equals("HillyTerrain")) grounded = true;
		
		Debug.Log ("OnCollisionStay object collided with is: " + objName);
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
