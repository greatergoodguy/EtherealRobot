using UnityEngine;
using System.Collections;

public class Button_Settings : MonoBehaviour {

	private GameObject head;
	private GameObject backButton;
	private GameObject settingsMenu;
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
		settingsMenu = GameObject.FindGameObjectWithTag("SettingsMenu");
		
		backHits = backButton.GetComponent<Button_Back>();
		settingsMenu.SetActive(false);
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
			settingsMenu.SetActive(false);
		}
		
		if(Physics.Raycast(headPos, headForward, out hit, 15f)){
			if(hit.collider.tag == "SettingsButton"){
				renderer.material.color = Color.Lerp(renderer.material.color, endColor, Time.deltaTime);
				if(renderer.material.color.a < 0.05f){
					backButton.renderer.enabled = true;
					backButton.collider.enabled = true;
					settingsMenu.SetActive(true);
				}
			}
			else 
				renderer.material.color = startColor;
		}
	
	}
}
