using UnityEngine;
using System.Collections;

public class Button_Back : MonoBehaviour {
	
	public static float BACK_HIT_PRESSES = 0;
	private GameObject head;
	private GameObject levelMenu;
	private GameObject settingsMenu;
	private Vector3 headPos;
	private Vector3 headForward;
	private Color startColor;
	private Color endColor;
	private float duration;
	
	// Use this for initialization
	void Start () {
		startColor = renderer.material.color;
		endColor = new Color(startColor.r, startColor.g, startColor.b, 0);
		
		head = GameObject.FindGameObjectWithTag("Head");
		levelMenu = GameObject.FindGameObjectWithTag("LevelMenu");
		settingsMenu = GameObject.FindGameObjectWithTag("SettingsMenu");
	}
	
	// Update is called once per frame
	void Update () {
		headPos = head.transform.position;
		headForward = head.transform.forward;
		RaycastHit hit;
		if(renderer.enabled){
			BACK_HIT_PRESSES = 1;
		}
		
		if(Physics.Raycast(headPos, headForward, out hit, 20f)){
			if(hit.collider.tag == "BackButton"){
				renderer.material.color = Color.Lerp(renderer.material.color, endColor, Time.deltaTime);
				if(renderer.material.color.a < 0.05f){
					renderer.enabled = false;
					collider.enabled = false;
					BACK_HIT_PRESSES -= 1;
				}
			}
			else{
				renderer.material.color = startColor;
			}
		}
		
		/*
		if(BACK_HIT_PRESSES == 0){
			renderer.enabled = false;
			collider.enabled = false;
		}
		*/
	
	}
	
	public float GetBackHitPresses(){
		return BACK_HIT_PRESSES;
	}
}
