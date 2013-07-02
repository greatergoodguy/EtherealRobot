using UnityEngine;
using System.Collections;

public class MechRightHand : MonoBehaviour {
	
	public bool RightGrabbed = false;	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other){	
		if(!RightGrabbed && other.tag.Equals("Player")){
			RightGrabbed = true;	
		}
	}	
}
