using UnityEngine;
using System.Collections;

public class GravityMechanic : MonoBehaviour {
	
	private bool counterGrav = true;
	private Vector3 oppositeGrav;
	// Use this for initialization
	void Start () {
		oppositeGrav = new Vector3(0f, 9.8f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		if (counterGrav){
			rigidbody.AddForce(oppositeGrav);
		}
		print (counterGrav);
	}
}
