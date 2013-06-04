using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OVRHeadMotion : HeadMotion {
	
	CameraController_BB cameraController;
	
	private Queue<Quaternion> camRotationHistory = new Queue<Quaternion>();
	public int historySize = 15;

	private Quaternion lastCamRotation = Quaternion.identity;
	
	void Start(){
		cameraController = GetComponent<CameraController_BB>();
	
		
		Quaternion currCamRotation = GetCamRotation();		
		for(int i=0; i<historySize; i++){
			camRotationHistory.Enqueue(currCamRotation);
		}
		
		DebugUtils.Assert(cameraController != null);
	}
	
	
	void Update(){
		Quaternion camRotation = GetCamRotation();
		cameraController.SetSharedOrientation(camRotation);
		
		camRotationHistory.Enqueue(camRotation);
		camRotationHistory.Dequeue();
		
		lastCamRotation = camRotation;
	}
	
	public override float GetHeadHorizontalAxis(){	
		return lastCamRotation.y;
	}
	
	private Quaternion GetCamRotation(){
		Quaternion DirQ = Quaternion.identity;
				
		// Read sensor here (prediction on or off)
		if(cameraController.PredictionOn == false)
			OVRDevice.GetOrientation(ref DirQ);
		else
			OVRDevice.GetPredictedOrientation(ref DirQ);
		
		return DirQ;
	}
	
	private float GetVariance(){
		
		float result = 0;
		float average = GetAverage();
		
		foreach(Quaternion quat in camRotationHistory){
			result += (quat.y - average) * (quat.y - average);
		}
		
		result = result / (camRotationHistory.Count - 1);
		
		return result;
	}
	
	private float GetAverage(){
		float sum = 0;
		foreach(Quaternion quat in camRotationHistory){
			sum += quat.y;
		}
		
		float average = sum / camRotationHistory.Count;
		return average;
	}
	
}
