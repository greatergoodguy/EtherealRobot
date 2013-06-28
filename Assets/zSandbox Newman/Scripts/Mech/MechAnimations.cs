using UnityEngine;
using System.Collections;

public class MechAnimations : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKeyDown(KeyCode.Alpha9)){
			animation.Play ("mech_walk");
		}
		else if(Input.GetKeyDown(KeyCode.Alpha0)){
			animation.Play ("mech_grab");
		}
			
	}
}
