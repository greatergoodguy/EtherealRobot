using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
	
	private NetworkMainMenu_BB networkMainMenu;
	private GameObject networkLogicGO;
	
	
	void Awake () {
		networkMainMenu = GetComponentInChildren<NetworkMainMenu_BB>();
		Transform networkLogic_transform = transform.FindChild("NetworkLogic");
		networkLogicGO = networkLogic_transform.gameObject;
		
		DebugUtils.Assert(networkMainMenu != null);
	}
	
	// Use this for initialization
	void Start () {
		if (!PhotonNetwork.connected){
			networkMainMenu.enabled = true;
			networkLogicGO.SetActive(false);
		}
		else{
			networkMainMenu.enabled = false;
			networkLogicGO.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
