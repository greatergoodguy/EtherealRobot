using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	
	private GameObject head;
	private GameObject backButton;
	private GameObject levelButton;
	private GameObject settingsButton;
	private GameObject settingsMenu;
	private GameObject settingsOne;
	private GameObject settingsTwo;
	private GameObject levelMenu;
	private GameObject levelOne;
	private GameObject levelTwo;
	
	private Vector3 headPos;
	private Vector3 headForward;
	private bool hitButton;
	private float backHitAmount;
	private RaycastHit hit;
	
	//Fade Stuff
	private Color startColor;
	private Color endColor;
	private float duration;
	
	// Use this for initialization
	void Start () {
		head = GameObject.FindGameObjectWithTag("Head");
		backButton = transform.FindChild("Button_Back").gameObject;
		levelOne = transform.FindChild("Button_Levels").gameObject;
		settingsButton = transform.FindChild("Button_Settings").gameObject;
		levelMenu = transform.FindChild("Menu_Levels").gameObject;
		levelOne = transform.FindChild("Level_Button1").gameObject;
		levelTwo = transform.FindChild("Level_Button2").gameObject;
		settingsMenu = transform.FindChild("Menu_Settings").gameObject;
		settingsOne = transform.FindChild("Settings_Button1").gameObject;
		settingsTwo = transform.FindChild("Settings_Button2").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		headPos = head.transform.position;
		headForward = head.transform.forward;
		
		hitButton = Physics.Raycast(headPos, headForward, out hit, 15f);
	}
}
