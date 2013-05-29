using UnityEngine;
using System.Collections;

public class GuiManager : MonoBehaviour {
	
	private DebugGui debugGui;
	private StartGui startGui;
	private PauseGui pauseGui;
	
	// Use this for initialization
	void Start () {
		debugGui = GetComponentInChildren<DebugGui>();
		startGui = GetComponentInChildren<StartGui>();
		pauseGui = GetComponentInChildren<PauseGui>();
		
		pauseGui.ExitGui();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.P)){
			if(pauseGui.IsGuiOn()){
				pauseGui.ExitGui();
			}
			else{
				pauseGui.EnterGui();
			}
			
		}
		
	}
	
	public void SetDebugGui(GameObject activePlayerGO){
		debugGui.SetNewActivePlayer(activePlayerGO);
	}
}
