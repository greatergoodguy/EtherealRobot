using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	private PauseMenuManager 	pauseMenuManager;
	private PlayerManager 		playerManager;
	private GuiManager 			guiManager;
	private InputManager 		inputManager;
	private EnvironmentManager	environmentManager;
	
	private Input_BB			activeInput;
	
	// Use this for initialization
	void Start () {	
		pauseMenuManager 	= GameObject.Find("PauseMenuManager").GetComponent<PauseMenuManager>();
		playerManager 		= GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
		guiManager 			= GameObject.Find("GuiManager").GetComponent<GuiManager>();
		inputManager 		= GameObject.Find("InputManager").GetComponent<InputManager>();
		environmentManager	= GameObject.Find("EnvironmentManager").GetComponent<EnvironmentManager>();
		
		activeInput = inputManager.GetActiveInput();
		
		DebugUtils.Assert(pauseMenuManager != null);
		DebugUtils.Assert(playerManager != null);
		DebugUtils.Assert(guiManager != null);
		DebugUtils.Assert(inputManager != null);
		DebugUtils.Assert(environmentManager != null);
		DebugUtils.Assert(activeInput != null);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.RightShift)){
			GameObject camera = playerManager.GetCamera();
			
			if(playerManager.isPlayerFrozen()){
				playerManager.UnfreezePlayer();	
				pauseMenuManager.HideMenu();
			}
			else{
				playerManager.FreezePlayer();
				pauseMenuManager.DisplayMenu(camera);
			}
		}
		
		if(activeInput.GetButtonDown_CycleActivePlayer()){
			playerManager.CycleActivePlayer();			
			guiManager.SetDebugGui(playerManager.GetActivePlayer());
		}	
		
		if(activeInput.GetButtonDown_CycleActiveEnvironment()){
			playerManager.SetPosition(new Vector3(813, 115, 1174));
			environmentManager.CycleActiveEnvironment();
		}
		
	}
}
