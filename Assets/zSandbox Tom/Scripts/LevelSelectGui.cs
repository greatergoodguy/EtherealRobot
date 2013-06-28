using UnityEngine;
using System.Collections;

public class LevelSelectGui : MonoBehaviour {
	
	public bool isGuiOn = true;
	
	void Update(){
		if(Input.GetKeyDown(KeyCode.P)){
			isGuiOn = !isGuiOn;
		}
	}
	
	private void OnGUI(){
		if(isGuiOn){
			if (GUILayout.Button("Start Scene", GUILayout.Width(400))){
				PhotonNetwork.Disconnect();
				Application.LoadLevel ("EtherealPrototype"); 
        	}
			if (GUILayout.Button("Obstacle Terrain", GUILayout.Width(400))){
				Application.LoadLevel ("ObstacleScene"); 
        	}
        	if (GUILayout.Button("TechBuild Finalv3 (May run really really slow)", GUILayout.Width(400))) {
				Application.LoadLevel ("Oculus_TechBuild_finalv03"); 
        	}
			if (GUILayout.Button("Hover Obstacle Course", GUILayout.Width(400))) {
				Application.LoadLevel ("HoverGameScene"); 
        	}
			if (GUILayout.Button("Warehouse", GUILayout.Width(400))) {
				Application.LoadLevel ("WarehouseScene"); 
        	}
		}
	}
}
