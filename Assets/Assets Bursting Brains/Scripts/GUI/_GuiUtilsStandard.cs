using UnityEngine;
using System.Collections;

public class _GuiUtilsStandard : MonoBehaviour {
	
	static public Font 	FontReplaceSmall	= null;
	static public Font 	FontReplaceLarge	= null;	
	
	private static float centerX 		= Screen.width/4 + 60;  // Need to figure out true center of Oculus
	private static float centerY 		= Screen.height/2;
	
	public static void GUIStereoButton(int X, int Y, int wX, int wY, string text, Color color){
		if(Screen.height > 800)
			GUI.skin.font = FontReplaceLarge;
		else
			GUI.skin.font = FontReplaceSmall;
		GUI.color = color;
		GUI.Box(new Rect(X, Y, wX, wY), text);			
	}
	
	public static void GUIStereoTexture(int X, int Y, int wX, int wY, Texture texture){
		GUI.color = Color.white;	
		GUI.DrawTexture(new Rect(X, X, wX, wY), texture);			
	}
	
	public static void GUIStereoTextureCentered(float X, float Y, Texture texture){
		// Change this
		int wX = texture.width;
		int wY = texture.height;
		int cX = (int)(centerX + X - wX/2);
		int cY = (int)(centerY + Y - wY/2);
		
		GUI.color = Color.white;	
		GUI.DrawTexture(new Rect(X, X, wX, wY), texture);			
		
	}

	// Move this to be accessed elsewhere later
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
