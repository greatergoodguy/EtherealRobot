using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	private PlayerManager 		playerManager;
	private GuiManager 			guiManager;
	private InputManager 		inputManager;
	private EnvironmentManager	environmentManager;
	
	// Use this for initialization
	void Start () {	
		playerManager 		= GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
		guiManager 			= GameObject.Find("GuiManager").GetComponent<GuiManager>();
		inputManager 		= GameObject.Find("InputManager").GetComponent<InputManager>();
		environmentManager	= GameObject.Find("EnvironmentManager").GetComponent<EnvironmentManager>();
		
		DebugUtils.Assert(playerManager != null);
		DebugUtils.Assert(guiManager != null);
		DebugUtils.Assert(inputManager != null);
		DebugUtils.Assert(environmentManager != null);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.RightShift)){
			GameObject camera = playerManager.GetCamera();
			
			if(playerManager.isPlayerFrozen()){
				playerManager.UnfreezePlayer();	
			}
			else{
				playerManager.FreezePlayer();
			}
		}
		
		if(inputManager.activeInput.GetButtonDown_CycleActivePlayer()){
			playerManager.CycleActivePlayer();			
			guiManager.SetDebugGui();
		}	
		
		if(inputManager.activeInput.GetButtonDown_CycleActiveEnvironment()){
			playerManager.SetPosition(new Vector3(813, 115, 1174));
			environmentManager.CycleActiveEnvironment();
		}
		
	}
}
