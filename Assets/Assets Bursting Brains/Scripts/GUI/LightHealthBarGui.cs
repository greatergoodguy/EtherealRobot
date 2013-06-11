using UnityEngine;
using System.Collections;

public class LightHealthBarGui : MonoBehaviour {
	
	public Texture healthBarTexture;
	
	private int healthBarMaxLength = 200;
	private int healthBarCurLength;
	
	private int healthDecrease = 2;
	private int healthIncrease = 4;
	
	private bool 	isGuiOn			= true;
	
	// Use this for initialization
	void Start () {
		
		//DebugUtils.Assert(healthBarTexture != null);	
		
		// healthBarMaxLength = 200;
		healthBarCurLength = healthBarMaxLength;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI(){
		if(!isGuiOn){
			return;	
		}
		
		KeyboardHealthReduction();
		
		_GuiUtilsOR.GUIStereoTexture(200, 150, healthBarCurLength, 20, healthBarTexture);
		
	}
	
	// Temporary function to show Light Health Bar go down
	void KeyboardHealthReduction(){
		
		//if(healthBarCurLength <= 0){
			// Death 	
		//}
        if (Input.GetKey(KeyCode.J) && healthBarCurLength > 0){
			healthBarCurLength -= healthDecrease;	
		}
		else{
			if(healthBarCurLength < healthBarMaxLength)
			healthBarCurLength += healthIncrease;	
		}
	}
	
	
	
	//public override bool IsGuiOn(){
	//	return isGuiOn;	
	//}	

	
}
