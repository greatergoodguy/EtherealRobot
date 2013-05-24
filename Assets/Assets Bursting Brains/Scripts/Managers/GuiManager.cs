using UnityEngine;
using System.Collections;

public class GuiManager : MonoBehaviour {
	
	private DebugGui debugGui;
	private StartGui startGui;
	
	// Use this for initialization
	void Start () {
		debugGui = GetComponentInChildren<DebugGui>();
		startGui = GetComponentInChildren<StartGui>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void SetDebugGui(GameObject activePlayerGO){
		debugGui.SetNewActivePlayer(activePlayerGO);
	}
}
