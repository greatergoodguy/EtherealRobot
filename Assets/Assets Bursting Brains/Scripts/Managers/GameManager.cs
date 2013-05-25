using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	private PauseMenuManager 	pauseMenuManager;
	private PlayerManager 		playerManager;
	private GuiManager 			guiManager;
	private InputManager 		inputManager;
	
	private Input_BB			activeInput;
	
	// Use this for initialization
	void Start () {	
		pauseMenuManager 	= GameObject.Find("PauseMenuManager").GetComponent<PauseMenuManager>();
		playerManager 		= GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
		guiManager 			= GameObject.Find("GuiManager").GetComponent<GuiManager>();
		inputManager 		= GameObject.Find("InputManager").GetComponentInChildren<InputManager>();
		
		activeInput = inputManager.GetActiveInput();
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
		
		if(Input.GetKeyDown(KeyCode.X)){
			playerManager.IncrementActivePlayer();			
			guiManager.SetDebugGui(playerManager.GetActivePlayer());
		}	
	}
}
