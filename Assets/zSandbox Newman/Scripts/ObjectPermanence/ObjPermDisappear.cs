using UnityEngine;
using System.Collections;

// Requires an attached gameobject and correct material

public class ObjPermDisappear : MonoBehaviour {
	
	public GameObject phaseOutObject;
	
	private GameObject head;
	private bool seesAnObject;
	private RaycastHit hit;
	private Color startColor;	
	private Color transColor;

	private bool objectStillExists = true;
	private bool isFadingOut = false;
	
	private float transparency = 1.0f;
	
	
	// Use this for initialization
	void Start () {
		head = GameObject.FindGameObjectWithTag("Head");
		startColor = phaseOutObject.renderer.material.color;	
		transColor = new Color (startColor.r, startColor.g, startColor.b, 0);	

	}
	
	// Update is called once per frame
	void Update () {
		if(objectStillExists)
			isBeingLookedAt();		

	}
	
	void isBeingLookedAt(){
		Vector3 headPos = head.transform.position;
		Vector3 headFor = head.transform.forward;
		
		seesAnObject = Physics.Raycast(headPos, headFor, out hit, Mathf.Infinity);
		if(seesAnObject && hit.collider.CompareTag("LookIcon")){
			
			phaseOutObject.renderer.material.color = Color.Lerp(phaseOutObject.renderer.material.color, transColor, Time.deltaTime);  // .a = tranzColor;
			isFadingOut = true;
			
			if(phaseOutObject.renderer.material.color.a < 0.05){
				phaseOutObject.SetActive(false);	
				objectStillExists = false;	
				isFadingOut = false;
			}
		}
		else{
			if(isFadingOut){
				phaseOutObject.renderer.material.color = Color.Lerp(phaseOutObject.renderer.material.color, startColor, Time.deltaTime * 3);  // .a = tranzColor;
			}
		}	
	}
	
	void TransparencyOverTime(){
		// Possibly add better time functionality of disappearance	
	}
	
}


