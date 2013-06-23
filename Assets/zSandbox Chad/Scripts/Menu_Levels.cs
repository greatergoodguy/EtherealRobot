using UnityEngine;
using System.Collections;

public class Menu_Levels : MonoBehaviour {
	
	private Transform levelOne;
	private Transform levelTwo;
	private GameObject head;
	private Vector3 headPos;
	private Vector3 headForward;
	private Color startColor = Color.blue;
	private Color hitColor = Color.green;
	private Color activateColor = Color.red;
	private RaycastHit hit;
	private bool hitButton;
	
	// Use this for initialization
	void Start () {
		head = GameObject.FindGameObjectWithTag("Head");
		levelOne = transform.FindChild("Level_Button1");
		levelTwo = transform.FindChild("Level_Button2");
		
		levelOne.renderer.material.color = startColor;
		levelTwo.renderer.material.color = startColor;
	}
	
	// Update is called once per frame
	void Update () {
		headPos = head.transform.position;
		headForward = head.transform.forward;
		
		hitButton = Physics.Raycast(headPos, headForward, out hit, 15f);
		if(hitButton){
			if(hit.collider.tag == "LevelOne"){
				levelOne.renderer.material.color = hitColor;
				if(Input.GetKey(KeyCode.Return))
					levelOne.renderer.material.color = activateColor;
				else
					levelOne.renderer.material.color = hitColor;
			}
			else if(hit.collider.tag == "LevelTwo"){
				levelTwo.renderer.material.color = hitColor;
				if(Input.GetKey(KeyCode.Return))
					levelTwo.renderer.material.color = activateColor;
				else
					levelTwo.renderer.material.color = hitColor;
			}
			else{
				levelOne.renderer.material.color = startColor;
				levelTwo.renderer.material.color = startColor;
		
			}
		}
	}
}
