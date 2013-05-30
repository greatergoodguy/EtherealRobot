using UnityEngine;
using System.Collections.Generic;

public abstract class PlayerController_Deprecated : MonoBehaviour{
	protected float DeltaTime = 1.0f;

	// Awake
	public virtual void Awake(){
	}

	// Start
	public virtual void Start(){
	}

	// Update
	public virtual void Update(){
		// If we are running at 60fps, DeltaTime will be set to 1.0 
		DeltaTime = (Time.deltaTime * 60.0f);
	}
	
	// LateUpdate
	public virtual void LateUpdate(){
	}
	
	public abstract string GetControllerName();
}



