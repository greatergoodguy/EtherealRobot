using UnityEngine;
using System.Collections;

public class Chad_LightMechanic : MonoBehaviour {
	
	//Light detection variables
	private float range = 10.3f;       //needs to be calculated for different sized spotlights
	private int lightIntensity = 0;
	private bool isInLight = true;
	public static bool IS_IN_LIGHT = false;
	private GameObject light;
	private Vector3 playerPos;
	private Vector3 lightPos;
	private Vector3 direction;
	private RaycastHit hit;
	
	
	//Health Mechanic Variables
	private static float MIN_HEALTH = 0;
	private static float MAX_HEALTH = 100;
	private float currentHealth;
	public float regenRate = 0.5f;
	
	//Fading mechanic variables
	Texture2D fader;
	private Color currentColor = new Color (0, 0, 0, 0);
	private Color fromColor = new Color (0, 0, 0, 0);
	private Color toColor = new Color (0, 0, 0, 1);
	private float fadeSpeed = 0.5f;

	// Use this for initialization
	void Start () {
		fader = new Texture2D(1, 1);
		fader.SetPixel(0, 0, Color.black);
		currentHealth = MAX_HEALTH;
		light = GameObject.Find("Spotlight");
	
	}
	
	/*
	 * CHAD: I messed around in your sandbox. I didn't really do much, just commented
	 * out code you weren't using anymore from this script and was testing out using cones
	 * 
	 * Found cone editor script here: http://wiki.unity3d.com/index.php?title=CreateCone 
	 * The script is in our Assets/Editor folder. It probably needs some editing to get 
	 * it to do light detection more correctly. 
	 */
	
	// Update is called once per frame
	void Update () {
		lightPos = light.transform.position;
		playerPos = transform.position;
		
		
		direction = playerPos - lightPos;
		Debug.DrawRay(lightPos, direction, Color.green);
		//print(Vector3.Distance(playerPos, lightPos));
		currentHealth = Mathf.Clamp(currentHealth, MIN_HEALTH, MAX_HEALTH);
		//Light recognition and Health implementation
		/*
		if(isInLight){
			currentHealth += regenRate;
			if(lightIntensity > 1)
				currentHealth += regenRate;
		}
		else
			currentHealth -= regenRate;
		//print("Health: " + currentHealth);
		*/
		if(Physics.Raycast(lightPos, direction, out hit, range)){
			// Ray hits player
			if(hit.collider.gameObject.CompareTag("Player")){
				isInLight = true;
			}
			//Ray hits non-player
			else
				isInLight = false;
			
			print (isInLight);
		}
	}
	
	void OnGUI(){
		if (!isInLight){
			currentColor = Color.Lerp(currentColor, toColor, Time.deltaTime * fadeSpeed);
			currentHealth -= regenRate;
		}
		
		else if (isInLight){
			currentColor = Color.Lerp(currentColor, fromColor, Time.deltaTime * fadeSpeed);
			currentHealth += regenRate;
		}
		GUI.color = currentColor;
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fader);
	}
	
	/*
	void OnTriggerEnter(Collider collision){
		if(collision.gameObject.tag == "Light Boundary")
			lightIntensity ++;
		
		if (lightIntensity > 0)
			isInLight = true;
		
    }
	
	void OnTriggerExit(Collider collision){
		if(collision.gameObject.tag == "Light Boundary")
			lightIntensity --;
		
		if (lightIntensity <= 0)
			isInLight = false;
    
	}
	*/
	
	public bool GetIsInLight(){
		return IS_IN_LIGHT;
	}
}
