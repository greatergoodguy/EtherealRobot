using UnityEngine;
using System.Collections;

public class _InputManagerHelper : MonoBehaviour {
	
	static bool isReadyToBeDown = false;
	
	void Update(){
		float menuAxis = Input.GetAxis("Menu Axis");
		if(menuAxis < 0.2f && menuAxis > -0.2f){
			SetIsReady(true);	
		}
	}
	
	public static bool IsReadyToBeDown(){
		return isReadyToBeDown;
	}
	
	public static void SetIsReady(bool isReady){
		isReadyToBeDown = isReady;	
	}

}
