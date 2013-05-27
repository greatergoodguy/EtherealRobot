using UnityEngine;
using System.Collections;

public class EtherealCrosshairGUI : MonoBehaviour {
	
	public Texture ImageCrosshair 	  = null;
	
	private float posX = Screen.width/2;
	private float posY = Screen.height/2;
	
	// Use this for initialization
	void Start () {
		DebugUtils.Assert(ImageCrosshair != null);
	}
	
	// Update is called once per frame
	void Update () {
		//GUI.DrawTexture(new Rect(posX, posY, ImageCrosshair.width, ImageCrosshair.height), ImageCrosshair);
		GUI.DrawTexture(new Rect(posX, posY, 100, 100), ImageCrosshair);		
	}
}
