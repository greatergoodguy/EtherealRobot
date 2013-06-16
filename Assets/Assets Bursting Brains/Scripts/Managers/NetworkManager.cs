using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
	
	private GameObject networkEnablerGO;
	private GameObject networkLogicGO;
	
	
	void Awake () {
		networkEnablerGO = transform.FindChild("NetworkEnabler").gameObject;
		networkLogicGO = transform.FindChild("NetworkLogic").gameObject;
		
		DebugUtils.Assert(networkEnablerGO != null);
		DebugUtils.Assert(networkLogicGO != null);
		
		if (!PhotonNetwork.connected){
			networkEnablerGO.SetActive(true);
			networkLogicGO.SetActive(false);
		}
		else{
			networkEnablerGO.SetActive(false);
			networkLogicGO.SetActive(true);
			
			// hack by tom - contact Tom and tell him to remove his mess
			GameObject singlePlayerEtherealGO = GameObject.Find ("Ethereal(Clone)");
			if(singlePlayerEtherealGO != null){
				singlePlayerEtherealGO.SetActive(false);
			}
		}
	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
