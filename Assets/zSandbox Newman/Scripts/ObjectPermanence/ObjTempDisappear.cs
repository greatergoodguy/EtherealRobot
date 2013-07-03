using UnityEngine;
using System.Collections;

// Requires an attached gameobject and correct material

public class ObjTempDisappear : MonoBehaviour {
	
	public GameObject phaseOutObject;
	
	public float DisappearSpeed = 2.0f;
	public float AppearSpeed = 1.5f;
	public float ActivateObject = 0.95f;
	
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
		if(seesAnObject && hit.collider.gameObject == gameObject){
			
			phaseOutObject.renderer.material.color = Color.Lerp(phaseOutObject.renderer.material.color, transColor, Time.deltaTime * DisappearSpeed);  // .a = tranzColor;
			isFadingOut = true;
			
			if(phaseOutObject.renderer.material.color.a < ActivateObject){
				//phaseOutObject.SetActive(false);
				phaseOutObject.collider.isTrigger = true;
			}
		}
		else{
			if(isFadingOut){
				phaseOutObject.renderer.material.color = Color.Lerp(phaseOutObject.renderer.material.color, startColor, Time.deltaTime * AppearSpeed);  // .a = tranzColor;
			}
			if(phaseOutObject.renderer.material.color.a >= ActivateObject){
				//phaseOutObject.SetActive(true);
				phaseOutObject.collider.isTrigger = false;
			}
		}	
	}
	
}


