using UnityEngine;
using System.Collections;

public class BounceMechanic : MonoBehaviour {
	
	public float bouncePower = 1000;
	private float currSpeed;
	private Vector3 forwardForce;
	private Vector3 playerDir;
	private GameObject player;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		forwardForce = transform.forward;
	}
	
	// Update is called once per frame
	void Update () {
		forwardForce = transform.forward;
	
	}
	
	void OnCollisionEnter(Collision collision){
		if(collision.gameObject.tag == "Bouncer"){
			rigidbody.AddForce(forwardForce * -bouncePower);
			currSpeed = 0;
		}
	}
}
