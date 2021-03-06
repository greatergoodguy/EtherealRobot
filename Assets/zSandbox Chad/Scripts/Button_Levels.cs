using UnityEngine;
using System.Collections;

public class Button_Levels : MonoBehaviour {
	
	private GameObject head;
	private GameObject backButton;
	private GameObject settingsButton;
	private GameObject levelMenu;
	private Vector3 headPos;
	private Vector3 headForward;
	private Button_Back backHits;
	private float backHitAmount;
	
	//Fade Stuff
	private Color startColor;
	private Color endColor;
	private float duration;
	
	// Use this for initialization
	void Start () {
		
		startColor = renderer.material.color;
		endColor = new Color(startColor.r, startColor.g, startColor.b, 0);
		
		head = GameObject.FindGameObjectWithTag("Head");
		backButton = GameObject.FindGameObjectWithTag("BackButton");
		settingsButton = GameObject.FindGameObjectWithTag("SettingsButton");
		levelMenu = GameObject.FindGameObjectWithTag("LevelMenu");
		backHits = backButton.GetComponent<Button_Back>();
		levelMenu.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		headPos = head.transform.position;
		headForward = head.transform.forward;
		RaycastHit hit;
		
		backHitAmount = backHits.GetBackHitPresses();
		if(backHitAmount == 0){
			renderer.enabled = true;
			collider.enabled = true;
			levelMenu.SetActive(false);
		}
		
		if(backHitAmount == 1){
			renderer.enabled = false;
			collider.enabled = false;
		}
		
		if(Physics.Raycast(headPos, headForward, out hit, 15f)){
			//duration = 0;
			if(hit.collider.tag == "MenuButton"){
				renderer.material.color = Color.Lerp(renderer.material.color, endColor, Time.deltaTime);
				if(renderer.material.color.a < 0.025f){
					backButton.renderer.enabled = true;
					backButton.collider.enabled = true;
					settingsButton.renderer.enabled = false;
					settingsButton.renderer.enabled = false;
					levelMenu.SetActive(true);
				}
			}
			else 
				renderer.material.color = startColor;
		}
	}
}
