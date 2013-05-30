using UnityEngine;
using System.Collections;

public class ButtonsNW : MonoBehaviour {
	
	private int xPos;
	private int yPos;
	private int xWidth;
	private int yWidth;
	private string origText;
	private string totalText;  // for dynamic GUI
	private Color buttonColor;
	private Color buttonColorOrig;
	

	public ButtonsNW(int X, int Y, int wX, int wY, string textInfo, Color color){
		xPos = X;
		yPos = Y;
		xWidth = wX;
		yWidth = wY;
		origText = textInfo;
		totalText = textInfo;
		buttonColorOrig = color;
		buttonColor = color;
	}
	
	public void Display(){
		GuiUtilsNW.GUIStereoButton(xPos, yPos, xWidth, yWidth, origText, buttonColor);
	}
	
	public void DynamicDisplay(float num){
		totalText = origText + num;
		GuiUtilsNW.GUIStereoButton(xPos, yPos, xWidth, yWidth, totalText, buttonColor);
	}
	
	public void ButtonSelected(){
		buttonColor = Color.yellow;		
	}
	
	public void ButtonDeselected(){
		buttonColor = buttonColorOrig;
	}

	
}
