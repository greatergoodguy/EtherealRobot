using UnityEngine;
using System.Collections;

public class Chad_FadeMechanic : LightDetection {
	
	//Health Mechanic Variables
	private static float MIN_HEALTH = 0;
	private static float MAX_HEALTH = 100;
	private float currentHealth;
	public float regenRate = 0.5f;
	private bool isInLight;
	
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
		
		isInLight = GetIsInLight();
		currentHealth = Mathf.Clamp(currentHealth, MIN_HEALTH, MAX_HEALTH);
	
		print (isInLight);
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
}
