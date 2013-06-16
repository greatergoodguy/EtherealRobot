using UnityEngine;
using System.Collections;

public class EnvironmentManager : MonoBehaviour {
	
	private ArrayList environments = new ArrayList();
	
	private int activeEnvironmentIndex;
	private GameObject activeEnvironment;
	
	// Use this for initialization
	void Start () {
		foreach(Transform childTransform in transform){
			environments.Add(childTransform.gameObject);	
		}
		
		
		activeEnvironmentIndex = 0;
		for(int i=1; i<environments.Count; i++){
			(environments[i] as GameObject).SetActive(false);
		}
		activeEnvironment = environments[0] as GameObject;
		
		DebugUtils.Assert(environments != null);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void CycleActiveEnvironment(){
		activeEnvironment.SetActive(false);
		
		activeEnvironmentIndex++;
		if(activeEnvironmentIndex >= environments.Count){			
			activeEnvironmentIndex = 0;
		}
		
		activeEnvironment = (GameObject) environments[activeEnvironmentIndex];
		activeEnvironment.SetActive(true);
	}
	
	public Vector3 GetEnvironmentStartPos(){
		Transform startPos_transform = activeEnvironment.transform.FindChild("StartPos");
		
		if(startPos_transform == null){
			print ("No start pos");
			return Vector3.zero;
		}
		else{
			print ("There is a start pos");
			return startPos_transform.position;
		}
	}
}
