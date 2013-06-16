using UnityEngine;
using System.Collections;

public class CameraBlackout : MonoBehaviour {
	
	public Texture startTexture;	
	public Texture startTexture2;
	

	bool Animating = false;
	float beginAnimTime = 0;
	float fullAnimTime = 4;
	bool BlackFlash = true;
	float FlashTime = 1;
	

	float nextPossAnim = 0;
	float BlackoutAnimTimer = 0;
	float FullAnimationTime = 4;
	
	bool curFrameBlack = false;
	bool FlashAnim = false;
	
	
	// Use this for initialization
	void Start () {
		//_GuiUtilsStandard.GUIStereoTexture(Screen.width/2, Screen.height/2, Screen.width, Screen.height, startTexture);
	}
	
	// Update is called once per frame
	void Update () {
		//GUI.DrawTexture(new Rect (Screen.width/2, Screen.height/2, Screen.width, Screen.height), startTexture);
		//_GuiUtilsStandard.GUIStereoTexture(Screen.width/2, Screen.height/2, Screen.width, Screen.height, startTexture);
	}
	
	void OnGUI(){
		if(!Animating)
			KeyboardBlackoutPress();
		else
			AnimateBlackout ();
		//GUI.DrawTexture(new Rect (Screen.width/2, 0/*Screen.height*/, Screen.width, Screen.height), startTexture);
		//_GuiUtilsStandard.GUIStereoTexture(Screen.width/2, Screen.height/2, Screen.width, Screen.height, startTexture);
	}
	
	void KeyboardBlackoutPress(){
		//GUI.DrawTexture(new Rect (Screen.width/2, 0/*Screen.height*/, Screen.width, Screen.height), startTexture);
		
		//if(Input.GetKey(KeyCode.Y)){
		//if(!Animating){
			if(Input.GetKeyDown(KeyCode.Y)){
				Animating = true;	
				beginAnimTime = Time.time;
				//	GUI.DrawTexture(new Rect (Screen.width/2, 0/*Screen.height*/, Screen.width, Screen.height), startTexture);
				//if(Time.time < 
			}	
		//}
	}	
	
	void AnimateBlackout(){
		/*float curTime = Time.time - beginAnimTime;
		
		if(curTime <= fullAnimTime){
			if(BlackFlash){
				GUI.DrawTexture(new Rect (Screen.width/2, 0, Screen.width, Screen.height), startTexture);
				BlackFlash = false;
				print ("Inside Blackflash");
			}
			else{
				GUI.DrawTexture(new Rect (Screen.width/2, 0, Screen.width, Screen.height), startTexture2);
				BlackFlash = true;
				print ("Inside Noflash");
			}
			
		}
		else{
			Animating = false;	
		}
		*/
		
		
		print ("Inside AnimateBlackout");
		float curTime = Time.time - beginAnimTime;
		
		if(!FlashAnim){
			print ("Should Get Here");
			BlackoutAnimTimer = curTime + 0.2f;
			FlashAnim = true;
		}
		else if(FlashAnim && ((BlackoutAnimTimer - curTime) > 0.1f) && ((BlackoutAnimTimer - curTime) <= 0.2f)){
			print ("Drawing");
			GUI.DrawTexture(new Rect (Screen.width/2, 0, Screen.width, Screen.height), startTexture);				
		}
		else if(FlashAnim && ((BlackoutAnimTimer < curTime) && (curTime >= FullAnimationTime))){
			print ("Reset");
			FlashAnim = false;
			Animating = false;
		}
		else if (FlashAnim && ((BlackoutAnimTimer < curTime))){
			print ("Blackout Reset");
			BlackoutAnimTimer = curTime + 0.2f;
		}
		else if(FlashAnim){
			print ("Not Drawing");
			// No GUI overlay (Allows for flashing effect)
		}
		
	}
}
