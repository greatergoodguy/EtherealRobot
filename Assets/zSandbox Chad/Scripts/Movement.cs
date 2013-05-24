using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	
	public float moveSpeed = 0.25f;
	private float angle;
	
	private Vector3 cube;
	private Vector3 sphere;
	private Vector3 cubeForward;
	private Vector3 sphereForward;
	private Transform head;
	
	private Vector3 forward;
	
	// Use this for initialization
	void Start () {
		cube = transform.position;
		sphereForward = transform.position;
		head = transform.FindChild("Sphere");
	}
	
	// Update is called once per frame
	void Update () {
		cubeForward = transform.forward;
		sphereForward = head.forward;
		float angle = Vector3.Angle (cubeForward,sphereForward);
		if(Input.GetKey(KeyCode.W)){
			Vector3 tempPos = transform.position;
			tempPos += cubeForward * moveSpeed;
			transform.position = tempPos;
		}
		if(Input.GetKey(KeyCode.S)){
			Vector3 tempPos = transform.position;
			tempPos -= cubeForward * moveSpeed;
			transform.position = tempPos;
		}
		print (angle);
	}
}
