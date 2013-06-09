using UnityEngine;
using System.Collections;

public class LightDetection : MonoBehaviour {
	
	private float range = 100.3f;       //needs to be calculated for different sized spotlights
	private int lightIntensity = 0;
	public static bool IS_IN_LIGHT = true;
	private bool lightHitCenter = true;
	private bool lightHitLeft = true;
	private bool lightHitRight = true;
	private GameObject player;
	private Vector3 playerPos;
	private Vector3 lightPos;
	private Vector3 direction;
	private RaycastHit hitCenter;
	private RaycastHit hitLeft;
	private RaycastHit hitRight;
	
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	
	}
	
	// Update is called once per frame
	void Update () {
		playerPos = player.transform.position;
		lightPos = transform.position;
		direction = playerPos - lightPos;
		
		Vector3 lightPosLeft = new Vector3(transform.position.x + 4f, transform.position.y, transform.position.z);
		Vector3 lightPosRight = new Vector3(transform.position.x - 4f, transform.position.y, transform.position.z);
		Vector3 playerPosRight = new Vector3(player.transform.position.x - 0.45f, player.transform.position.y, player.transform.position.z);
		Vector3 playerPosLeft = new Vector3(player.transform.position.x + 0.45f, player.transform.position.y, player.transform.position.z);
		Vector3 directionLeft = playerPosLeft - lightPosLeft;
		Vector3 directionRight = playerPosRight - lightPosRight;
		
		Debug.DrawRay(lightPos, direction, Color.green);
		Debug.DrawRay(lightPosLeft, directionLeft, Color.green);
		Debug.DrawRay(lightPosRight, directionRight, Color.green);
		
		lightHitCenter = Physics.Raycast(lightPos, direction, out hitCenter, range);
		lightHitLeft = Physics.Raycast(lightPosLeft, directionLeft, out hitLeft, range);
		lightHitRight = Physics.Raycast(lightPosRight, directionRight, out hitRight, range);
		if(lightHitCenter || lightHitLeft || lightHitRight){
			// Ray hits player
			if(hitCenter.collider.gameObject.tag == "Player" || hitLeft.collider.gameObject.tag == "Player" || hitRight.collider.gameObject.tag == "Player"){
				IS_IN_LIGHT = true;
			}
			//Ray hits non-player
			else
				IS_IN_LIGHT = false;
		}
		else{
			IS_IN_LIGHT = false;
		}
	
	}
	public bool GetIsInLight(){
			return IS_IN_LIGHT;
		}
}
