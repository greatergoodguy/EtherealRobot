using UnityEngine;
using System.Collections;

public class Chad_LightMechanic : MonoBehaviour {
	
	private RaycastHit hit;
	private GameObject player;
	private GameObject spotlight;
	private GameObject lightCollider;
	/*
	private float playerPosX;
	private float playerPosZ;
	private float lightXRange;
	private float lightZRange;
	*/
	private float range;
	private int lightIntensity = 0;
	private bool isInLight = false;
	

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		spotlight = GameObject.FindGameObjectWithTag("Light");
		lightCollider = GameObject.FindGameObjectWithTag("Light Boundary");
		range = spotlight.light.range;
		
	
	}
	
	// Update is called once per frame
	void Update () {
		float dist = Vector3.Distance(player.transform.position, spotlight.transform.position);
		Vector3 rayDirection = new Vector3(spotlight.transform.position.x, 0, spotlight.transform.position.z);
		
		Debug.DrawLine(spotlight.transform.position, hit.point, Color.green);
		
		/*
		//Point light recognition
		if (dist < range){
			print ("You are in light");
			
		}
		else{
			print ("You are not in light");
		}
		*/
		
		//Spotlight recognition
		if(isInLight){
			print ("Light is good");
			if(lightIntensity > 2)
				print ("You must love light");			
		}
		else{
			print ("Get in the light!");
		}
	}
	
	void OnTriggerEnter(Collider collision){
		lightIntensity ++;
		if (lightIntensity > 0){
			isInLight = true;
		}
    }
	void OnTriggerExit(Collider collision){
		lightIntensity --;
		if (lightIntensity <= 0){
			isInLight = false;
		}
    }
}
