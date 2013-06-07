using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	private PlayerManager 		playerManager;
	private GuiManager 			guiManager;
	private EnvironmentManager	environmentManager;
	
	// Use this for initialization
	void Start () {	
		playerManager 		= GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
		guiManager 			= GameObject.Find("GuiManager").GetComponent<GuiManager>();
		environmentManager	= GameObject.Find("EnvironmentManager").GetComponent<EnvironmentManager>();
		
		DebugUtils.Assert(playerManager != null);
		DebugUtils.Assert(guiManager != null);
		DebugUtils.Assert(environmentManager != null);
		
		guiManager.SetDebugGui(playerManager.GetActivePlayerDebugData());
		DebugUtils.Assert(playerManager.GetActivePlayerDebugData() != null);
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
		
		if(InputManager.activeInput.GetButtonDown_CycleActivePlayer()){
			//playerManager.CycleActivePlayer();			
		}	
		
		if(InputManager.activeInput.GetButtonDown_CycleActiveEnvironment()){
			playerManager.SetPosition(new Vector3(813, 115, 1174));
			environmentManager.CycleActiveEnvironment();
		}
		
		if(InputManager.activeInput.GetButtonDown_Pause()){
			guiManager.TogglePauseGui();
		}
		
		if(InputManager.activeInput.GetButtonDown_Debug()){
			guiManager.ToggleDebugGui();
		}	
		if(InputManager.activeInput.GetButtonDown_ToggleInputControls()){
			InputManager.ToggleInput();
		}
		
	}
}
