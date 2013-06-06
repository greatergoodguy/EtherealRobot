using UnityEngine;
using System.Collections;

public class _GuiUtilsOR : MonoBehaviour {
	
	static public Font 	FontReplaceSmall	= null;
	static public Font 	FontReplaceLarge	= null;
	static private int    	StereoSpreadX 	= -40;	
	
	private static float ploLeft = 0, ploRight = 0;
	private static float sSX = (float)Screen.width / 1280.0f;
	private static float sSY = ((float)Screen.height / 800.0f);
	private static int sSpreadX = (int)((float)StereoSpreadX * sSX);
	
	private static float centerX 		= Screen.width/4 + 60;  // Need to figure out true center of Oculus
	private static float centerY 		= Screen.height/2;
	
	// Creates GUI buttons
	public static void GUIStereoButton(int X, int Y, int wX, int wY, string text, Color color){
		//int X = (int) rect.x;
		//int Y = (int) rect.y;
		//int wX = (int) rect.width; 
		//int wY = (int) rect.height;

		OVRDevice.GetPhysicalLensOffsets(ref ploLeft, ref ploRight); 
		int xL = (int)((float)X * sSX);
		int xR = (Screen.width / 2) + xL + sSpreadX
			      // required to adjust for physical lens shift
			      - (int)(ploLeft * (float)Screen.width / 2);
		int y = (int)((float)Y * sSY);
		
		//GUI.contentColor = color;
		
		int sWX = (int)((float)wX * sSX);
		int sWY = (int)((float)wY * sSY);
		
		FontSizeHelper();

		GUI.color = color;	

		GUI.Box(new Rect(xL, y, sWX, sWY), text);
		GUI.Box(new Rect(xR, y, sWX, sWY), text);		

	}
	
	// Places a GUI texture
	public static void GUIStereoTexture(int X, int Y, int wX, int wY, Texture texture){
		
		DrawTextureHelper(X, Y, wX, wY, texture);
	}	
	
	// Places a GUI texture based off center of screen
	public static void GUIStereoTextureCentered(float X, float Y, Texture texture){
		
		int wX = texture.width;
		int wY = texture.height;
		int cX = (int)(centerX + X - wX/2);
		int cY = (int)(centerY + Y - wY/2);
		
		DrawTextureHelper(cX, cY, wX, wY, texture);	
	}
	
	public static void DrawTextureHelper(int X, int Y, int wX, int wY, Texture texture){
		
		OVRDevice.GetPhysicalLensOffsets(ref ploLeft, ref ploRight); 
		int xL = (int)((float)X * sSX);									//*
		
		int xR = (Screen.width / 2) + xL + sSpreadX						//*
			      // required to adjust for physical lens shift
			      - (int)(ploLeft * (float)Screen.width / 2);
		int y = (int)((float)Y * sSY);									//*
		
		int sWX = (int)((float)wX * sSX);								//*
		int sWY = (int)((float)wY * sSY);
		
		FontSizeHelper();
		
		GUI.color = Color.white;
		
		GUI.DrawTexture(new Rect(xL, y, sWX, sWY), texture);
		GUI.DrawTexture(new Rect(xR, y, sWX, sWY), texture);
	}
	
	// Helper function for GUIStereoButton and GUIStereoTexture
	public static void FontSizeHelper(){  // Change font size based on screen scale
				
		if(Screen.height > 800)
			GUI.skin.font = FontReplaceLarge;
		else
			GUI.skin.font = FontReplaceSmall;
	}
	
	public static int GUIKeyboardUpDown(int index, ArrayList ButtonsList){
		
		// Moves between buttons with arrows keys
		if(InputManager.activeInput.GetButtonDown_MenuUp() && index < ButtonsList.Count - 1){
			((Button) ButtonsList[index]).ButtonDeselected();
			index++;
			((Button) ButtonsList[index]).ButtonSelected();
		}
		if(InputManager.activeInput.GetButtonDown_MenuDown() && index > 0){
			((Button) ButtonsList[index]).ButtonDeselected();
			index--;
			((Button) ButtonsList[index]).ButtonSelected();
		}
		
		return index;	
	}
	
}
