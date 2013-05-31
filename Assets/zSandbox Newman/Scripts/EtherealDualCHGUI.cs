using UnityEngine;
using System.Collections;

public class EtherealDualCHGUI : MonoBehaviour {

	public Texture ImageCrosshair	= null;
	public Texture tick1			= null;
	public Texture tick2			= null;
	public Texture tick3			= null;
	
	private float centerX 		= Screen.width/2;
	private float centerY 		= Screen.height/2;
	private float tickSpacing 	= Screen.width/24;
	
	private EtherealPC etherealPC = null;
	
	void Start() {
		DebugUtils.Assert(ImageCrosshair != null);
		DebugUtils.Assert(tick1 != null);
		DebugUtils.Assert(tick2 != null);
		DebugUtils.Assert(tick3 != null);
		
		etherealPC = GetComponent<EtherealPC>();
	}

	void Update() {
	}
	
	void OnGUI(){
		
		
		GuiUtilsNW.GUIStereoTexture((int)((centerX/2 + 60 + 1 * tickSpacing) - tick1.width/2), (int)(centerY - tick1.height/2), tick1.width, tick1.height, tick1);	
		GuiUtilsNW.GUIStereoTexture((int)((centerX/2 + 60 - 1 * tickSpacing) - tick1.width/2), (int)(centerY - tick1.height/2), tick1.width, tick1.height, tick1);	
		GuiUtilsNW.GUIStereoTexture((int)((centerX/2 + 60 + 2 * tickSpacing) - tick2.width/2), (int)(centerY - tick2.height/2), tick2.width, tick2.height, tick2);
		GuiUtilsNW.GUIStereoTexture((int)((centerX/2 + 60 - 2 * tickSpacing) - tick2.width/2), (int)(centerY - tick2.height/2), tick2.width, tick2.height, tick2);
		GuiUtilsNW.GUIStereoTexture((int)((centerX/2 + 60 + 3 * tickSpacing) - tick3.width/2), (int)(centerY - tick3.height/2), tick3.width, tick3.height, tick3);
		GuiUtilsNW.GUIStereoTexture((int)((centerX/2 + 60 - 3 * tickSpacing) - tick3.width/2), (int)(centerY - tick3.height/2), tick3.width, tick3.height, tick3);
		/*
		GuiUtilsNW.GUIStereoTexture((int)(centerX + 1 * tickSpacing), (int)(centerY), tick1.width, tick1.height, tick1);
		GuiUtilsNW.GUIStereoTexture((int)(centerX - 1 * tickSpacing), (int)(centerY), tick1.width, tick1.height, tick1);
		GuiUtilsNW.GUIStereoTexture((int)(centerX + 2 * tickSpacing), (int)(centerY), tick2.width, tick2.height, tick2);
		GuiUtilsNW.GUIStereoTexture((int)(centerX - 2 * tickSpacing), (int)(centerY), tick2.width, tick2.height, tick2);
		GuiUtilsNW.GUIStereoTexture((int)(centerX + 3 * tickSpacing), (int)(centerY), tick3.width, tick3.height, tick3);
		GuiUtilsNW.GUIStereoTexture((int)(centerX - 3 * tickSpacing), (int)(centerY), tick3.width, tick3.height, tick3);
		*/
		
		
		//float cursorX = Screen.width * ((etherealPC.GetAngle() + 180)/360);
		
		//DrawTextureCenter(cursorX, centerY, ImageCrosshair);
		
		/*
		DrawTextureCenter(centerX + 1 * tickSpacing, centerY, tick1);
		DrawTextureCenter(centerX + 2 * tickSpacing, centerY, tick2);
		DrawTextureCenter(centerX + 3 * tickSpacing, centerY, tick3);
		
		DrawTextureCenter(centerX - 1 * tickSpacing, centerY, tick1);
		DrawTextureCenter(centerX - 2 * tickSpacing, centerY, tick2);
		DrawTextureCenter(centerX - 3 * tickSpacing, centerY, tick3);
		*/
	}
	
	private void DrawTextureCenter(float posX, float posY, Texture texture){
		GUI.DrawTexture(new Rect(posX - texture.width/2, posY - texture.height/2, texture.width, texture.height), texture);
	}
}
