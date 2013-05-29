using UnityEngine;
using System.Collections;

public class Chad_EtherealCrosshairGUI : MonoBehaviour {
	
	public Texture ImageCrosshair	= null;
	public Texture tick1			= null;
	public Texture tick2			= null;
	public Texture tick3			= null;
	
	private float centerX 		= Screen.width/2;
	private float centerY 		= Screen.height/2;
	private float tickSpacing 	= Screen.width/8;
	
	private MovementPC movementPC = null;
	
	void Start() {
		DebugUtils.Assert(ImageCrosshair != null);
		DebugUtils.Assert(tick1 != null);
		DebugUtils.Assert(tick2 != null);
		DebugUtils.Assert(tick3 != null);
		
		movementPC = GetComponent<MovementPC>();
		
		DebugUtils.Assert(movementPC != null);
	}

	void Update() {
	}
/*	
	void OnGUI(){
		float cursorX = Screen.width * ((movementPC.GetAngle() + 180)/360);
		
		DrawTextureCenter(cursorX, centerY, ImageCrosshair);
		
		DrawTextureCenter(centerX + 1 * tickSpacing, centerY, tick1);
		DrawTextureCenter(centerX + 2 * tickSpacing, centerY, tick2);
		DrawTextureCenter(centerX + 3 * tickSpacing, centerY, tick3);
		
		DrawTextureCenter(centerX - 1 * tickSpacing, centerY, tick1);
		DrawTextureCenter(centerX - 2 * tickSpacing, centerY, tick2);
		DrawTextureCenter(centerX - 3 * tickSpacing, centerY, tick3);
	}
	
	private void DrawTextureCenter(float posX, float posY, Texture texture){
		GUI.DrawTexture(new Rect(posX - texture.width/2, posY - texture.height/2, texture.width, texture.height), texture);
	}
*/
}
