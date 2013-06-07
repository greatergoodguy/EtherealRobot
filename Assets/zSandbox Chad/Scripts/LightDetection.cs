using UnityEngine;
using System.Collections;

public class LightDetection : MonoBehaviour {
	
	private float range = 100.3f;       //needs to be calculated for different sized spotlights
	private int lightIntensity = 0;
	private bool isInLight = true;
	private GameObject player;
	private Vector3 playerPos;
	private Vector3 lightPos;
	private Vector3 direction;
	private RaycastHit hit;
	
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	
	}
	
	// Update is called once per frame
	void Update () {
		playerPos = player.transform.position;
		lightPos = transform.position;
		direction = playerPos - lightPos;
		
		Vector3 lightPosLeft = new Vector3(transform.position.x - 5.5f, transform.position.y, transform.position.z);
		Vector3 lightPosRight = new Vector3(transform.position.x + 5.5f, transform.position.y, transform.position.z);
		Vector3 playerPosRight = new Vector3(player.transform.position.x - 0.7f, player.transform.position.y, player.transform.position.z);
		Vector3 playerPosLeft = new Vector3(player.transform.position.x + 0.7f, player.transform.position.y, player.transform.position.z);
		Vector3 directionLeft = playerPosLeft - lightPos;
		Vector3 directionRight = playerPosRight - lightPos;
		
		Debug.DrawRay(lightPos, direction, Color.green);
		Debug.DrawRay(lightPos, directionLeft, Color.green);
		Debug.DrawRay(lightPos, directionRight, Color.green);
		if(Physics.Raycast(lightPos, direction, out hit, range)){
			// Ray hits player
			if(hit.collider.gameObject.tag == "Player"){
				isInLight = true;
			}
			//Ray hits non-player
			else
				isInLight = false;
		}
		else{
			isInLight = false;
		}
		print (isInLight);
	
	}
}
