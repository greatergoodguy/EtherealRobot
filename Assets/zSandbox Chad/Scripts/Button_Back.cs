using UnityEngine;
using System.Collections;

public class Button_Back : MonoBehaviour {
	
	public static float BACK_HIT_PRESSES = 0;
	public GameObject head;
	public GameObject levelMenu;
	public GameObject settingsMenu;
	public Vector3 headPos;
	public Vector3 headForward;
	// Use this for initialization
	void Start () {
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
			if(Input.GetKeyDown(KeyCode.Return) && hit.collider.tag == "BackButton"){
				BACK_HIT_PRESSES -= 1;
			}
		}
		if(BACK_HIT_PRESSES == 0 && levelMenu.activeInHierarchy){
			renderer.enabled = false;
			collider.enabled = false;
			levelMenu.SetActive(false);
		}
	
	}
	
	public float GetBackHitPresses(){
		return BACK_HIT_PRESSES;
	}
}
