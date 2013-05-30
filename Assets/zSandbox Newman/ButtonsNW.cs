using UnityEngine;
using System.Collections;

public class ButtonsNW : MonoBehaviour {
	
	private int xPos;
	private int yPos;
	private int xWidth;
	private int yWidth;
	private string origText;
	private Color buttColor;
	
	private string totalText;
	
	public ButtonsNW(int X, int Y, int wX, int wY, string textInfo, Color color){
		xPos = X;
		yPos = Y;
		xWidth = wX;
		yWidth = wY;
		origText = textInfo;
		totalText = textInfo;
		buttColor = color;
	}
	
	public void Display(){
		GuiUtilsNW.GUIStereoButton(xPos, yPos, xWidth, yWidth, totalText, buttColor);
	}
		
	public void ChangeColor(Color color){
		buttColor = color;
	}
	
	public void UpdateText(float num){
		totalText = origText + num;
	}
}
