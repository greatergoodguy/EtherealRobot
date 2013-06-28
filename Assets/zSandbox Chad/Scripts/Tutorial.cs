using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour {
	
	private bool madeContact = false;
	public Texture2D image;
	private Color currentColor = new Color (1, 1, 1, 1);
	private Color fadeColor = new Color (1, 1, 1, 0);
	private Color solidColor = new Color (1, 1, 1, 1);
	private float tutorialDuration = 0;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		print(madeContact);
	}
	
	void OnTriggerEnter(Collider collision){
		if(collision.gameObject.tag == "Player"){
			madeContact = true;
		}
	}
	void OnTriggerExit(Collider collision){
		if(collision.gameObject.tag == "Player"){
			madeContact = false;
		}
	}
	
	void OnGUI(){
		if(madeContact){
			tutorialDuration += Time.deltaTime;
			if(tutorialDuration >= 3){
				currentColor = Color.Lerp(currentColor, fadeColor, Time.deltaTime);
				GUI.color = currentColor;
				_GuiUtilsORChad.GUIStereoTexture(125, 50, Screen.width/2, Screen.height/2, image);
				//GUI.DrawTexture(new Rect(125, 50, Screen.width/2, Screen.height/2), image);
			}
			else{
				GUI.color = solidColor;
				_GuiUtilsORChad.GUIStereoTexture(125, 50, Screen.width/2, Screen.height/2, image);
				//GUI.DrawTexture(new Rect(125, 50, Screen.width/2, Screen.height/2), image);
			}
		}
		else{
			tutorialDuration = 0;
			currentColor = solidColor;
		}
	}
}
