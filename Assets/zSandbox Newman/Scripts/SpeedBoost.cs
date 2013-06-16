using UnityEngine;
using System.Collections;

public class SpeedBoost : MonoBehaviour {
	
	private bool boostEffect = false;
	private float boostTime = 3.0f;
	private Time beginTime;
	
	
	GameObject cam;
	private CameraMotionBlur CMB;
	private EdgeDetectEffect EDE;
	

	// Use this for initialization
	void Start () {
		CMB = transform.GetComponent("CameraMotionBlur") as CameraMotionBlur;
		//CMB = new CameraMotionBlur();
		EDE = transform.GetComponent("EdgeDetectEffect") as EdgeDetectEffect;
		//CMB = transform.GetComponent("CameraMotionBlur") as MonoBehaviour;

		//CMB = GetComponent<CameraMotionBlur>();
		//cam.GetComponent("CameraMotionBlur")
	}
	
	// Update is called once per frame
	void Update () {


		
		if(Input.GetKeyDown(KeyCode.B)){
			EDE.enabled = !EDE.enabled;
			//(GetComponent("CameraMotionBlur") as MonoBehaviour).enabled = false;
			//GetComponent("CameraMotionBlur") as
			//SendMessage ("DisableEnableController", 12.0f);			
			//gameObject.GetComponent(typeof (CMB) as CameraMotionBlur)
			
			Debug.Log("Pressed B");
			
			//boostEffect = true;
			
			//gameObject.GetComponent("CameraMotionBlur").

			//CS.GetComponent("CameraMotionBlur").enabled = true;
			//CMB.en
			//beginTime = Time.time; 
			
		}
		/*
		else if(boostEffect && Time.time > (beginTime + boostTime)){
			CS.GetComponent("CameraMotionBlur").enabled = false;		
			boostEffect = false;
		}
		*/
	}

}
