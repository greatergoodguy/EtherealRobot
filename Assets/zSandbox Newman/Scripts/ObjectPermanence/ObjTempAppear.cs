using UnityEngine;
using System.Collections;

public class ObjTempAppear : MonoBehaviour {
	
	public GameObject phaseInObject;

	public float AppearSpeed = 2.0f;
	public float DisappearSpeed = 1.0f;
	public float ActivateObjectParamater = 0.05f;	
	
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
		
		startColor = new Color (endColor.r, endColor.g, endColor.b, 0);		
		phaseInObject.renderer.material.color = startColor;			
	}
	
	// Update is called once per frame
	void Update () {
		if(objectDoesntExist){
			isBeingLookedAt();
		}
		//else if(!objectDoesntExist){
			//set collider to true;    Use isTrigger?
		//}
	}
	
	void isBeingLookedAt(){
		Vector3 headPos = head.transform.position;
		Vector3 headFor = head.transform.forward;	
		
		seesAnObject = Physics.Raycast(headPos, headFor, out hit, Mathf.Infinity);
		if(seesAnObject && hit.collider.gameObject == gameObject){
			
			phaseInObject.renderer.material.color = Color.Lerp(phaseInObject.renderer.material.color, endColor, Time.deltaTime * AppearSpeed);  // .a = tranzColor;
			isFadingIn = true;
			
			if(phaseInObject.renderer.material.color.a > ActivateObjectParamater){
				//phaseObject.SetActive(false);
				phaseInObject.collider.isTrigger = false;
			}			
		}
		else{
			if(isFadingIn){
				phaseInObject.renderer.material.color = Color.Lerp(phaseInObject.renderer.material.color, startColor, Time.deltaTime * DisappearSpeed);  // .a = tranzColor;
			}
			if(phaseInObject.renderer.material.color.a <= ActivateObjectParamater){
				//phaseOutObject.SetActive(true);
				phaseInObject.collider.isTrigger = true;
			}				
		}	
	}
}
