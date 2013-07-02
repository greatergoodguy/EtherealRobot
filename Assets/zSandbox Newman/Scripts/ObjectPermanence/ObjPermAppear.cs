using UnityEngine;
using System.Collections;

public class ObjPermAppear : MonoBehaviour {
	
	public GameObject phaseInObject;
	
	private GameObject head;
	private bool seesAnObject;
	private RaycastHit hit;	
	
	private Color startColor;	
	private Color endColor;	
	
	private bool objectDoesntExist = true;
	private bool isFadingIn = false;	
	
	// Use this for initialization
	void Start () {
		head = GameObject.FindGameObjectWithTag("Head");	
		endColor = phaseInObject.renderer.material.color;
		
		//Color tempColor = phaseInObject.renderer.material.color;
		
		startColor = new Color (endColor.r, endColor.g, endColor.b, 0);		
		phaseInObject.renderer.material.color = startColor;			
	}
	
	// Update is called once per frame
	void Update () {
		if(objectDoesntExist){
			isBeingLookedAt();
		}
		else if(!objectDoesntExist){
			//set collider to true;    Use isTrigger?
		}
	}
	
	void isBeingLookedAt(){
		Vector3 headPos = head.transform.position;
		Vector3 headFor = head.transform.forward;	
		
		seesAnObject = Physics.Raycast(headPos, headFor, out hit, Mathf.Infinity);
		if(seesAnObject && hit.collider.gameObject == gameObject){
			
			Debug.Log ("is reappearing");
			
			phaseInObject.renderer.material.color = Color.Lerp(phaseInObject.renderer.material.color, endColor, Time.deltaTime * 1.2f);  // .a = tranzColor;
			isFadingIn = true;
			
			if(phaseInObject.renderer.material.color.a > 0.89f){
				//phaseObject.SetActive(false);
				phaseInObject.renderer.material.color = endColor;
				phaseInObject.collider.isTrigger = false;
				objectDoesntExist = false;	
				isFadingIn = false;
			}
		}
		else{
			if(isFadingIn){
				phaseInObject.renderer.material.color = Color.Lerp(phaseInObject.renderer.material.color, startColor, Time.deltaTime * 3);  // .a = tranzColor;
			}
		}	
	}
}
