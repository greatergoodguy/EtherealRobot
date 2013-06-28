using UnityEngine;
using System.Collections;

public class Menu_Settings : MonoBehaviour {

	private Transform settingsOne;
	private Transform settingsTwo;
	private GameObject head;
	private Vector3 headPos;
	private Vector3 headForward;
	private Color startColor;
	private Color hitColor;
	//private Color activateColor = Color.red;
	private RaycastHit hit;
	private bool hitButton;
	
	// Use this for initialization
	void Start () {
		head = GameObject.FindGameObjectWithTag("Head");
		settingsOne = transform.FindChild("Settings_Button1");
		settingsTwo = transform.FindChild("Settings_Button2");
		
		startColor = settingsOne.renderer.material.color;
		hitColor = new Color(startColor.r, startColor.g, startColor.b, 0);
	}
	
	// Update is called once per frame
	void Update () {
		headPos = head.transform.position;
		headForward = head.transform.forward;
		
		hitButton = Physics.Raycast(headPos, headForward, out hit, 15f);
		if(hitButton){
			if(hit.collider.gameObject.name == "Settings_Button1"){
				settingsOne.renderer.material.color = Color.Lerp(settingsOne.renderer.material.color, hitColor, Time.deltaTime);
				/*
				if(Input.GetKey(KeyCode.Return))
					levelOne.renderer.material.color = activateColor;
				else
					levelOne.renderer.material.color = hitColor;
				*/
			}
			else if(hit.collider.gameObject.name == "Settings_Button2"){
				settingsTwo.renderer.material.color = Color.Lerp(settingsTwo.renderer.material.color, hitColor, Time.deltaTime);;
				/*
				if(Input.GetKey(KeyCode.Return))
					levelTwo.renderer.material.color = activateColor;
				else
					levelTwo.renderer.material.color = hitColor;
				*/
			}
			else{
				settingsOne.renderer.material.color = startColor;
				settingsTwo.renderer.material.color = startColor;
		
			}
		}
	}
}
