using UnityEngine;
using System.Collections;

public class MechMouseLook : MonoBehaviour {

	// class variables for looking
	public bool invertedLook = false;
	public float lookSensitivity = 5.0f;
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
	public float tiltAngle = 12f;
	public float tiltSensitivity = 0.5f;
	public float tiltSpeed = 100f;
	public float tiltDamp = 0.2f;
	public float lookDirection;
	
	// Initialization
	//void Start () {
	//	zRotateCurrent = transform.localRotation.z;
	//}
	
	// Update is called once per frame
	void Update () {
	
		float mouseXLoc = Input.GetAxis ("Mouse X");
		
		// Get the target rotation based on mouse input
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
		//	Debug.Log (lookDirection, gameObject);
		} else if (lookDirection < -tiltSensitivity) {  // looking left
			zRotateTarget = tiltAngle;
		//	Debug.Log (lookDirection, gameObject);
		} else { 
			zRotateTarget = 0.0f;
		}
		
		// Smooth out movement
		// TODO: could probably just use a Vector3
		yRotateCurrent = Mathf.SmoothDamp (yRotateCurrent, yRotateTarget, ref yRotateSpeed, lookDamp);
		xRotateCurrent = Mathf.SmoothDamp (xRotateCurrent, xRotateTarget, ref xRotateSpeed, lookDamp);
		zRotateCurrent = Mathf.SmoothDamp (zRotateCurrent, zRotateTarget, ref zRotateSpeed, tiltDamp);
		
		// Debug.Log ("xRotateSpeed = " + xRotateSpeed);
		// Debug.Log ("yRotateSpeed = " + yRotateSpeed);
		// Debug.Log ("zRotateSpeed = " + zRotateSpeed);
		
		// Apply rotations to the camera
		transform.rotation = Quaternion.Euler (xRotateCurrent, yRotateCurrent, zRotateCurrent);
		Debug.Log ("done applying mouse look transforms");
	}
}
