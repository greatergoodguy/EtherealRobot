using UnityEngine;
using System.Collections;

public class GuiManager : MonoBehaviour {
	
	private DebugGui debugGui;
	private StartMenuGui startGui;
	private PauseGui pauseGui;
	
	// Use this for initialization
	void Start () {
		debugGui = GetComponentInChildren<DebugGui>();
		startGui = GetComponentInChildren<StartMenuGui>();
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
	
	public void SetDebugGui(){
		debugGui.SetNewActivePlayer();
	}
}
