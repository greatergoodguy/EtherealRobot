using UnityEngine;
using System.Collections;

public class MechLeftHand : MonoBehaviour {
	
	public bool LeftGrabbed = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){	
		if(!LeftGrabbed && other.tag.Equals("Player")){
			LeftGrabbed = true;	
		}
	}
}
