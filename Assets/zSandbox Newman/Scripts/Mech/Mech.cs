using UnityEngine;
using System.Collections;

public class Mech : MonoBehaviour {
	
	GameObject player;
	GameObject LeftHand;
	GameObject RightHand;
	
	bool LeftGrab = false;
	bool RightGrab = false;
	bool playerIsGrabbed = false;
	
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		LeftHand = GameObject.Find("l_finger_anim");
		RightHand = GameObject.Find("r_finger_anim");	
	}
	
	// Update is called once per frame
	void Update () {
		
		if(!playerIsGrabbed){
			if(LeftHand.GetComponent<MechLeftHand>().LeftGrabbed){
				LeftGrab = true;
				playerIsGrabbed = true;			
			}
			else if(RightHand.GetComponent<MechRightHand>().RightGrabbed){
				RightGrab = true;	
				playerIsGrabbed = true;			
			}
		}
		else if(playerIsGrabbed){
			MechHoldingPlayer();
		}
	}
		
	void MechHoldingPlayer(){
		Vector3 handPos;
		if(LeftGrab){
			handPos = LeftHand.transform.position;
		}
		else{  //RightGrab
		//else if(RightGrab){
			handPos = RightHand.transform.position;
		}	
		player.transform.position = handPos;		
	}
}
