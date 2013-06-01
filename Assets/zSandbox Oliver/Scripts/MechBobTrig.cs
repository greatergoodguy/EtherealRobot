using UnityEngine;
using System.Collections;

public class MechBobTrig : MonoBehaviour {

	public float bobFrequency = 0.5f;
	public float xRange = 0.92f;
	public float yRange = 0.88f;
	public float bobX = 0.5f;
	public float bobY = 0.5f;
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
		if (yMovement < -jerk) yMovement = -jerk;
		transform.localPosition = new Vector3 (xMovement * xRange, yMovement * yRange, transform.localPosition.z);
	
	}
}
