using UnityEngine;
using System.Collections;

public class PauseMenuManager : MonoBehaviour {

	private Vector3 idlePos;
	
	private GameObject resumeGO;
	private Vector3 cameraPos;
	
	// Use this for initialization
	void Start () {
		idlePos = transform.FindChild("IdlePosition").transform.localPosition;
		cameraPos = idlePos;
		
		
		resumeGO = transform.FindChild("Resume").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			RaycastHit hitInfo = new RaycastHit();
			if (Physics.Linecast(cameraPos, resumeGO.transform.position, out hitInfo) && hitInfo.transform == resumeGO.transform){
				//resumeGO.SetActive(false);
				
				print("true");
			}
			else{
				print("false");	
			}
		}
	}
	
	public void HideMenu(){
		transform.position = idlePos;
		
		cameraPos = idlePos;
	}
	
	
	private Vector3 verticalOffset = new Vector3(0, 5, 0);
	public void DisplayMenu(GameObject camera){
		transform.position = camera.transform.position + camera.transform.forward * 5 + verticalOffset;
		
		cameraPos = camera.transform.position;
		transform.LookAt(cameraPos);	
	}
}
