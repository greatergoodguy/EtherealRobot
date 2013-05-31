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
	
	private int oculScreenAdjust = 60;
	
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
		
		// This code currently breaks when using the Oculus
		/* float cursorX = Screen.width * ((etherealPC.GetAngle() + 180)/360);
		   DrawTextureCenter(cursorX, centerY, ImageCrosshair);*/
		
		DrawTextureCenter(centerX/2 + oculScreenAdjust + 1 * tickSpacing, centerY, tick1);
		DrawTextureCenter(centerX/2 + oculScreenAdjust - 1 * tickSpacing, centerY, tick1);
		DrawTextureCenter(centerX/2 + oculScreenAdjust + 2 * tickSpacing, centerY, tick2);
		DrawTextureCenter(centerX/2 + oculScreenAdjust - 2 * tickSpacing, centerY, tick2);
		DrawTextureCenter(centerX/2 + oculScreenAdjust + 3 * tickSpacing, centerY, tick3);
		DrawTextureCenter(centerX/2 + oculScreenAdjust - 3 * tickSpacing, centerY, tick3);
	}
	
	private void DrawTextureCenter(float posX, float posY, Texture texture){
		
		// Changed for Oculus View
		GuiUtilsNW.GUIStereoTexture((int)(posX - texture.width/2), (int)(posY - texture.height/2), texture.width, texture.height, texture);
		//GUI.DrawTexture(new Rect(posX - texture.width/2, posY - texture.height/2, texture.width, texture.height), texture);
	}
}
