using UnityEngine;
using System.Collections;

public class Mirror : MonoBehaviour {
	
	private Transform light;
	private Vector3 origin;
	private Vector3 mirrorForward;
	private float distance;

	
	// Use this for initialization
	void Start () {
		light = GameObject.Find("ReflectedLight").transform;
		origin = light.position;

		
		//Vector3();
	}
	
	// Update is called once per frame
	void Update () {
		//Vector3 mirrorLightOrigin = (transform.position);
		//Debug.DrawLine(light.position, transform.position, Color.red);	
		/*
		if(Physics.Raycast(light.position, light.forward, Vector3.Distance(light.position, transform.position)) && gameObject.tag == "Mirror"){
			print("Hit Mirror");	
		}
		else{
			print ("Did not hit mirror");
		}*/
		
		
		//Debug.DrawLine(mirrorLightOrigin, Color.red);
	}
}
