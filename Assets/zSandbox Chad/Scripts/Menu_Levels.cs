using UnityEngine;
using System.Collections;

public class Menu_Levels : MonoBehaviour {
	
	private Transform levelOne;
	private Transform levelTwo;
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
		levelOne = transform.FindChild("Level_Button1");
		levelTwo = transform.FindChild("Level_Button2");
		
		startColor = levelOne.renderer.material.color;
		hitColor = new Color(startColor.r, startColor.g, startColor.b, 0);
	}
	
	// Update is called once per frame
	void Update () {
		headPos = head.transform.position;
		headForward = head.transform.forward;
		
		hitButton = Physics.Raycast(headPos, headForward, out hit, 15f);
		if(hitButton){
			if(hit.collider.gameObject.name == "Level_Button1"){
				levelOne.renderer.material.color = Color.Lerp(levelOne.renderer.material.color, hitColor, Time.deltaTime);
				/*
				if(Input.GetKey(KeyCode.Return))
					levelOne.renderer.material.color = activateColor;
				else
					levelOne.renderer.material.color = hitColor;
				*/
			}
			else if(hit.collider.gameObject.name == "Level_Button2"){
				levelTwo.renderer.material.color = Color.Lerp(levelTwo.renderer.material.color, hitColor, Time.deltaTime);;
				/*
				if(Input.GetKey(KeyCode.Return))
					levelTwo.renderer.material.color = activateColor;
				else
					levelTwo.renderer.material.color = hitColor;
				*/
			}
			else{
				levelOne.renderer.material.color = startColor;
				levelTwo.renderer.material.color = startColor;
		
			}
		}
	}
}
