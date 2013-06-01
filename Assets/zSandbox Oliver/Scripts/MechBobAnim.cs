using UnityEngine;
using System.Collections;

public class MechBobAnim : MonoBehaviour {
	public CharacterController playerController; 
	public Animation anim;	// Animation component
	private bool isMoving;
	private bool left;
              
	void CameraAnimations() {
		if (playerController.isGrounded == true) {
			if (isMoving == true) {
				if (left == true) { 
					// Waits for any previous animation to finish 
					if (!anim.isPlaying) anim.Play("leftStep");
				} else {
					// Waits for any previous animation to finish 
					if (!anim.isPlaying) anim.Play("rightStep");
				}
				left = !left;
			}
		}
	}
                
	void Start () { 
		left = true;
		isMoving = false;
	}
                  
	void Update () {
		float inputX = Input.GetAxis("Horizontal");	
		float inputY = Input.GetAxis("Vertical");
		
		if (inputX  != 0 || inputY != 0) isMoving = true;  
		else isMoving = false; 
		
		CameraAnimations();
	}
}
