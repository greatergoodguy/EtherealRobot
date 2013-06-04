using UnityEngine;
using System.Collections;

public class GuiManager : MonoBehaviour {
	
	private DebugGui debugGui;
	private StartMenuGui startGui;
	private PauseMenuGui pauseGui;
	
	void Awake(){
		debugGui = GetComponentInChildren<DebugGui>();
		startGui = GetComponentInChildren<StartMenuGui>();
		pauseGui = GetComponentInChildren<PauseMenuGui>();
		
		pauseGui.ExitGui();
		debugGui.ExitGui();
		
		startGui.EnterGui();
	}
		
	// Use this for initialization
	void Start() {
	}
	
	// Update is called once per frame
	void Update() {
		
	}
	
	public void TogglePauseGui(){
		if(pauseGui.IsGuiOn()){
			pauseGui.ExitGui();
		}
		else{
			debugGui.ExitGui();
			pauseGui.EnterGui();
		}	
	}
	
	public void SetDebugGui(DebugData debugData){
		debugGui.SetDebugData(debugData);
	}
	
	public void ToggleDebugGui(){
		if(debugGui.IsGuiOn()){
			debugGui.ExitGui();
		}
		else{
			pauseGui.ExitGui();
			debugGui.EnterGui();
		}	
	}
}
