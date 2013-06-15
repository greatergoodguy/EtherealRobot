using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
	
	private NetworkMainMenu_BB networkMainMenu;
	private GameObject networkLogicGO;
	
	
	void Awake () {
		networkMainMenu = transform.FindChild("NetworkEnabler").transform.GetComponentInChildren<NetworkMainMenu_BB>();
		networkLogicGO = transform.FindChild("NetworkLogic").gameObject;
		
		DebugUtils.Assert(networkMainMenu != null);
		
		if (!PhotonNetwork.connected){
			networkMainMenu.enabled = true;
			networkLogicGO.SetActive(false);
		}
		else{
			networkMainMenu.enabled = false;
			networkLogicGO.SetActive(true);
			GameObject.Find ("Ethereal(Clone)").SetActive(false);
		}
	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
