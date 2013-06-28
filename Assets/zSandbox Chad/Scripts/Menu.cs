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
	private float backHitAmount = 0;
	private RaycastHit hit;
	
	//Fade Stuff
	private Color menuColor;
	private Color subMenuColor;
	private Color backColor;
	private Color endMenuColor;
	private Color endSubMenuColor;
	private Color endBackColor;
	private float duration;
	
	// Use this for initialization
	
	void Start () {
		head = GameObject.FindGameObjectWithTag("Head");
		backButton = transform.FindChild("Button_Back").gameObject;
		levelButton = transform.FindChild("Button_Levels").gameObject;
		settingsButton = transform.FindChild("Button_Settings").gameObject;
		levelMenu = transform.FindChild("Menu_Levels").gameObject;
		levelOne = transform.FindChild("Level_Button1").gameObject;
		levelTwo = transform.FindChild("Level_Button2").gameObject;
		settingsMenu = transform.FindChild("Menu_Settings").gameObject;
		settingsOne = transform.FindChild("Settings_Button1").gameObject;
		settingsTwo = transform.FindChild("Settings_Button2").gameObject;
		
		menuColor = levelButton.renderer.material.color;
		subMenuColor = levelOne.renderer.material.color;
		backColor = backButton.renderer.material.color;
		endMenuColor = new Color(menuColor.r, menuColor.g, menuColor.b, 0);
		endSubMenuColor = new Color(subMenuColor.r, subMenuColor.g, subMenuColor.b, 0);
		endBackColor = new Color(backColor.r, backColor.g, backColor.b, 0);
		
		levelMenu.SetActive(false);
		settingsMenu.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		headPos = head.transform.position;
		headForward = head.transform.forward;
		
		hitButton = Physics.Raycast(headPos, headForward, out hit, 15f);
		if (hitButton){
			if(hit.collider.gameObject.name == "Button_Levels"){
				levelButton.renderer.material.color = Color.Lerp(levelButton.renderer.material.color, endMenuColor, Time.deltaTime);
				if(levelButton.renderer.material.color.a < 0.025f){
					backButton.renderer.enabled = true;
					backButton.collider.enabled = true;
					settingsButton.renderer.enabled = false;
					settingsButton.renderer.enabled = false;
					levelMenu.SetActive(true);
				}
				else
					levelButton.renderer.material.color = menuColor;
			}
		}
	}
}
