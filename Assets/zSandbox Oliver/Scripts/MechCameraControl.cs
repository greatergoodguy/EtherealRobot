using UnityEngine;
using System.Collections;

public class MechCameraControl : MonoBehaviour {

	// class variables for looking
	public bool invertedLook = false;
	public float lookSensitivity = 2.0f;
	public float lookDamp = 0.1f;
		
	private float xRotateTarget;
	private float yRotateTarget;
	private float zRotateTarget;
	private float xRotateCurrent;
	public float yRotateCurrent;  // is accessed by player control script
	private float zRotateCurrent = 0.0f;
	private float xRotateSpeed;
	private float yRotateSpeed;
	private float zRotateSpeed;
	
	// class variables for look tilting
	public float tiltAngle = 8f;
	public float tiltSensitivity = 0.5f;
	public float tiltSpeed = 100f;
	public float tiltDamp = 0.2f;
	private float lookDirection;
	
	// class variables for head bob control
	public float bobFrequency = 0.5f;	// 0..1 - Adjusts stride
	public float bobX = 0.4f;	// 0..1 - Adjust amount of horizontal bob
	public float bobY = 0.35f;	// 0..1 - Adjust amount of vertical bob
	public float tiltAngleX = 8f;
	public float tiltAngleZ = 9f;
	public float jerk = 0.25f;	// 0..1 - Adjust jerky-ness of movement, 1 is smoothest
	private float xMovement;
	private float yMovement;
	private float distance;
	private Vector3 prevPosition;
	
	
	// Awake is always called 
	void Awake () {
		prevPosition = transform.parent.parent.position;
	}
			
	// Initialization
	//void Start () {
	//	zRotateCurrent = transform.localRotation.z;
	//}
	
	// Update is called once per frame
	void Update () {
		
		// TODO: make boolean to detect user input, and run transforms if true
		// TODO: if no user input, resettle the camera to default position
		mouseLook(); headBob();
				
		// Apply headbob translations camera's position relative to the parent
		transform.localPosition = new Vector3 (xMovement, yMovement, transform.localPosition.z);
		// TODO: may be more efficient to translate the existing transform matrix than newing up one and assigning
		// transform.Translate(current sine pos - previous, current sine pos - previous, 0f);	
		// Debug.Log ("done applying head bob translations at distance " + distance);
		
		// Apply headbob and mouselook rotations to camera
		transform.rotation = Quaternion.Euler (
			xRotateCurrent + (yMovement * tiltAngleX), 
			yRotateCurrent, 
			zRotateCurrent + (xMovement * tiltAngleZ));
		// Debug.Log ("done applying mouse look transforms");
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
		
		if (lookDirection > tiltSensitivity) {  // looking right
			zRotateTarget = -tiltAngle;
		} else if (lookDirection < -tiltSensitivity) {  // looking left
			zRotateTarget = tiltAngle;
		} else { 
			zRotateTarget = 0.0f;
		}
		
		// Smooth out movement
		// TODO: could probably just use a Vector3
		yRotateCurrent = Mathf.SmoothDamp (yRotateCurrent, yRotateTarget, ref yRotateSpeed, lookDamp);
		xRotateCurrent = Mathf.SmoothDamp (xRotateCurrent, xRotateTarget, ref xRotateSpeed, lookDamp);
		zRotateCurrent = Mathf.SmoothDamp (zRotateCurrent, zRotateTarget, ref zRotateSpeed, tiltDamp);
		// Debug.Log ("xRotateSpeed = "+xRotateSpeed"\nyRotateSpeed = "+yRotateSpeed+"\nzRotateSpeed = "+zRotateSpeed);
	}
	
	// Headbob method bobs head
	void headBob () {
	
		// Get current camera position
		Vector3 currPosition = transform.parent.parent.position;
	
		// If player is grounded, then get to bobbin'
		if (transform.parent.parent.GetComponent<MechPlayerControl>().grounded) {
			// TODO: need to handle when float overflows
			distance += Vector3.Distance(prevPosition, currPosition) * bobFrequency;
		}
		prevPosition = currPosition;
		
		// Using sine to calculate head bob from distance traveled 
		// NOTE: sin (x*2) ** 0.5 may also be used for vertical head movement
		xMovement = Mathf.Sin (distance) * bobX;
		yMovement = Mathf.Sin (distance * 2) * bobY;
		// Flattens out and jerks downward head movement
		float yThreshold = -bobY * jerk;
		if (yMovement < yThreshold) yMovement = yThreshold * jerk/4;
	}
}
