using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	private GameObject pauseMenuManagerGO;
	private GameObject playerManagerGO;
	
	private DebugGuiManager debugGuiManager;
	private PauseMenuManager pauseMenuManager;
	private PlayerManager playerManager;
	
	// Use this for initialization
	void Start () {
		pauseMenuManagerGO = GameObject.Find("PauseMenuManager");
		playerManagerGO = GameObject.Find("PlayerManager");
		GameObject debugGuiManagerGO = GameObject.Find("DebugGuiManager");
		
		debugGuiManager = debugGuiManagerGO.GetComponent<DebugGuiManager>();
		pauseMenuManager = pauseMenuManagerGO.GetComponent<PauseMenuManager>();
		playerManager = playerManagerGO.GetComponent<PlayerManager>();
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
			debugGuiManager.SetNewActivePlayer(playerManager.GetActivePlayer());
		}	
	}
}
