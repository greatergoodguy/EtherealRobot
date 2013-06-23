using UnityEngine;
using System.Collections;

public class LevelsButton : Button_Back {
	
	private GameObject head;
	private GameObject backButton;
	private GameObject levelMenu;
	private Vector3 headPos;
	private Vector3 headForward;
	private float backHitAmount;
	// Use this for initialization
	void Start () {
		head = GameObject.FindGameObjectWithTag("Head");
		backButton = GameObject.FindGameObjectWithTag("BackButton");
		levelMenu = GameObject.FindGameObjectWithTag("LevelMenu");
		levelMenu.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		headPos = head.transform.position;
		headForward = head.transform.forward;
		RaycastHit hit;
		if(Physics.Raycast(headPos, headForward, out hit, 15f)){
			if(Input.GetKeyDown(KeyCode.Return) && hit.collider.tag == "MenuButton"){
				backButton.renderer.enabled = true;
				backButton.collider.enabled = true;
				levelMenu.SetActive(true);
			}
		}
		backHitAmount = GetBackHitPresses();
		if(backHitAmount == 0){
			renderer.enabled = true;
			collider.enabled = true;
		}
		print(backHitAmount);
	}
}
