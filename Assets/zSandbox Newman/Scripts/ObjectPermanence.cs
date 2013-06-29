using UnityEngine;
using System.Collections;

// Requires an attached gameobject and correct material

public class ObjectPermanence : MonoBehaviour {
	
	public GameObject phaseObject;
	
	private GameObject head;
	private bool seesAnObject;
	private RaycastHit hit;
	private Color startColor;	
	private Color transColor;

	private bool objectStillExists = true;
	private bool isFading = false;
	
	private float transparency = 1.0f;
	
	
	// Use this for initialization
	void Start () {
		head = GameObject.FindGameObjectWithTag("Head");
		startColor = phaseObject.renderer.material.color;	
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
		
		seesAnObject = Physics.Raycast(headPos, headFor, out hit, Mathf.Infinity);  // change to infinite
		if(seesAnObject && hit.collider.CompareTag("LookIcon")){
			
			phaseObject.renderer.material.color = Color.Lerp(phaseObject.renderer.material.color, transColor, Time.deltaTime);  // .a = tranzColor;
			isFading = true;
			
			if(phaseObject.renderer.material.color.a < 0.05){
				phaseObject.SetActive(false);	
				objectStillExists = false;	
				isFading = false;
			}
		}
		else{
			if(isFading){
				phaseObject.renderer.material.color = Color.Lerp(phaseObject.renderer.material.color, startColor, Time.deltaTime * 3);  // .a = tranzColor;
			}
		}	
	}
	
	void TransparencyOverTime(){
		// Possibly add better time functionality of disappearance	
	}
	
}


