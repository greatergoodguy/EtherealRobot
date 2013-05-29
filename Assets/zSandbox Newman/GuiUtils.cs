using UnityEngine;
using System.Collections;

public class GuiUtils : MonoBehaviour {
	
	static public Font 	FontReplaceSmall	= null;
	static public Font 	FontReplaceLarge	= null;
	static private int    	StereoSpreadX 	= -40;	
	
	/*public static void GUITexture(int X, int Y, Texture texture){
		texture.wi
		
	}*/
	
	// Creates GUI buttons
	public static void GUIStereoButton(int X, int Y, int wX, int wY, string text, Color color){
		//int X = (int) rect.x;
		//int Y = (int) rect.y;
		//int wX = (int) rect.width; 
		//int wY = (int) rect.height;
		
		float ploLeft = 0, ploRight = 0;
		float sSX = (float)Screen.width / 1280.0f;
		
		float sSY = ((float)Screen.height / 800.0f);
		OVRDevice.GetPhysicalLensOffsets(ref ploLeft, ref ploRight); 
		int xL = (int)((float)X * sSX);
		int sSpreadX = (int)((float)StereoSpreadX * sSX);
		int xR = (Screen.width / 2) + xL + sSpreadX
			      // required to adjust for physical lens shift
			      - (int)(ploLeft * (float)Screen.width / 2);
		int y = (int)((float)Y * sSY);
		
		//GUI.contentColor = color;
		
		int sWX = (int)((float)wX * sSX);
		int sWY = (int)((float)wY * sSY);
		
		// Change font size based on screen scale
		if(Screen.height > 800)
			GUI.skin.font = FontReplaceLarge;
		else
			GUI.skin.font = FontReplaceSmall;
		
		// Change color if selected by keyboard
		//if(curButton){//SelectedIndex == index){
		GUI.color = color;	
		//}
		GUI.Button(new Rect(xL, y, sWX, sWY), text);
		GUI.Button(new Rect(xR, y, sWX, sWY), text);		
		
		//GUI.color = Color.white;
		
		//print("xL: " + xL + "       xR: " + xR);
	}
}
