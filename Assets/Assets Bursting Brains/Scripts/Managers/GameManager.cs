using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	private GameObject pauseMenuManagerGO;
	private GameObject playerManagerGO;
	
	private PauseMenuManager pauseMenuManager;
	private PlayerManager playerManager;
	private GuiManager guiManager;
	
	//private DebugGui debugGui;
	
	// Use this for initialization
	void Start () {
		pauseMenuManagerGO = GameObject.Find("PauseMenuManager");
		playerManagerGO = GameObject.Find("PlayerManager");
		GameObject guiManagerGO = GameObject.Find("GuiManager");
		
		//GameObject debugGuiGO = GameObject.Find("DebugGui");
		
		//debugGui = debugGuiGO.GetComponent<DebugGui>();
		pauseMenuManager = pauseMenuManagerGO.GetComponent<PauseMenuManager>();
		playerManager = playerManagerGO.GetComponent<PlayerManager>();
		guiManager = guiManagerGO.GetComponent<GuiManager>();
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
			//debugGui.SetNewActivePlayer(playerManager.GetActivePlayer());
			
			guiManager.SetDebugGui(playerManager.GetActivePlayer());
		}	
	}
}
