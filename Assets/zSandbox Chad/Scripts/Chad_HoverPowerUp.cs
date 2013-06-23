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
		if(madeContact){
			renderer.enabled = false;
			collider.enabled = false;
			madeContact = false;
			respawn = 0;
		}
		
		if(!renderer.enabled){
			respawn += Time.deltaTime;
		}
		
		if(respawn >= 3){
			renderer.enabled = true;
			collider.enabled = true;
		}
		print(respawn);
	}
	
	void OnTriggerEnter(Collider collision){
		if (collision.gameObject.CompareTag("Player")){
			madeContact = true;
		}
	}
}
