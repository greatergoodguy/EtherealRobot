using UnityEngine;
using System.Collections;

public class Chad_HoverPowerUp : MonoBehaviour {
	
	private float respawn;
	private bool madeContact = false;
	
	// Use this for initialization
	void Start () {
		respawn = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider collision){
		if (collision.gameObject.CompareTag("Player")){
			madeContact = true;
			renderer.enabled = false;
		}
	}
	
	void OnTriggerExit(Collider collision){
		if(collision.gameObject.CompareTag("Player"))
			renderer.enabled = true;		
	}
}
