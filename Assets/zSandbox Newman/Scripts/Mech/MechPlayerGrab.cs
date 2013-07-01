using UnityEngine;
using System.Collections;

public class MechPlayerGrab : MechInfo {
	
	GameObject player;
	
	//private bool playerIsGrabbed = false;
	//private int grabDistance = 10;
	
	
	// Use this for initialization
	void Start () {
		//player = GameObject.Find("Ethereal");
		
	}
	
	// Update is called once per frame
	/*void Update () {
		
		
		
		if(playerIsGrabbed){
			Vector3 handPos = this.transform.position;
			player.transform.position = handPos;
		}
		
	}*/
	
	void OnTriggerStay(Collider other){
		if(!playerIsGrabbed && other.tag.Equals("Player")){
			Debug.Log("player being grabbed");
			player = other.gameObject;
			playerIsGrabbed = true;
		}
	}
	
	
	/*void grabPlayer(){
			
			
		Vector3 handPos = this.transform.position;
		Vector3 playerPos = player.transform.position;
		float xDif = Mathf.Abs(handPos.x - playerPos.x);
		float yDif = Mathf.Abs(handPos.y - playerPos.y);
		float zDif = Mathf.Abs(handPos.z - playerPos.z);
		if(xDif < grabDistance && yDif < grabDistance && zDif < grabDistance){
			isGrabbed = true;	
			Debug.Log("Player's been grabbed");
		}

		
	}*/
	
}
