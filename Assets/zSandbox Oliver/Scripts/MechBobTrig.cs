using UnityEngine;
using System.Collections;

public class MechBobTrig : MonoBehaviour {

	public float bobFrequency = 0.5f;
	public float xRange = 0.92f;
	public float yRange = 0.88f;
	public float bobX = 0.5f;
	public float bobY = 0.5f;
	public float tiltAngleX = 8f;
	public float tiltAngleZ = 10f;
	public float jerk = 0.85f;
	private float distance;
	private Vector3 prevPosition;
	
	
	// Awake is always called 
	void Awake () {
		prevPosition = transform.parent.parent.position;
	}
	
	// Update is called once per frame
	void Update () {
	
		Vector3 currPosition = transform.parent.parent.position;
	
		// if player is grounded, then get to bobbin'
		if (transform.parent.parent.GetComponent<MechPlayerControl>().grounded) {
			// TODO: need to handle when float overflows
			distance += Vector3.Distance(prevPosition, currPosition) * bobFrequency;
		}
		prevPosition = currPosition;
		
		// using sine to calculate head bob from distance traveled 
		float xMovement = Mathf.Sin (distance) * bobX;
		float yMovement = Mathf.Sin (distance * 2) * bobY;
		// flattens out bottom of downwards head movement
		if (yMovement < -jerk) yMovement = -jerk;
		
		// apply translations and rotation to camera's position relative to the parent
		transform.localPosition = new Vector3 (xMovement * xRange, yMovement * yRange, transform.localPosition.z);
		transform.localEulerAngles = new Vector3 (yMovement * tiltAngleX, transform.localEulerAngles.y, xMovement * tiltAngleZ);
		
		Debug.Log ("done applying head bob transforms at distance " + distance);
		// TODO: may be more efficient to translate the existing transform matrix than newing up one and assigning
		// transform.Translate(current sine pos - previous, current sine pos - previous, 0f);
	}
}
