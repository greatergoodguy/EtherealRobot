using UnityEngine;
using System.Collections;

public class MouseLook_Ethereal : MonoBehaviour {

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationY = 0F;
	
	private HeadMotion mouseHeadMotion;
	private HeadMotion ovrHeadMotion;
	
	private HeadMotion activeHeadMotion;
	
	private Quaternion initialQuaternion;
	private Vector3 initialForward;
	
	void Start () {
		mouseHeadMotion = transform.FindChild("Camera").GetComponent<HeadMotion>();
		ovrHeadMotion = transform.FindChild("OVRCameraController").GetComponent<HeadMotion>();
		
		activeHeadMotion = mouseHeadMotion;
		
		DebugUtils.Assert(mouseHeadMotion != null);
		DebugUtils.Assert(ovrHeadMotion != null);
		
		initialQuaternion = transform.localRotation;
		initialForward = transform.forward;
		
		//print (initialQuaternion);
		//print (initialForward);
    }
	
    void Update ()
    {
        if (axes == RotationAxes.MouseXAndY) {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Xbox_Horizontal") * sensitivityX;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
        else if (axes == RotationAxes.MouseX) {
			if(activeHeadMotion == mouseHeadMotion){
				float horizontalAxis = activeHeadMotion.GetHeadHorizontalAxis();
				transform.Rotate(0, horizontalAxis * sensitivityX, 0);	
			}
			else{
				float ovrAxis = activeHeadMotion.GetHeadHorizontalAxis();
				transform.localRotation = Quaternion.AngleAxis(180 * ovrAxis, transform.up);
			}
        }
        else
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }
    }
	
	public void SwitchHeadMotion(){
		if(activeHeadMotion == mouseHeadMotion)
			activeHeadMotion = ovrHeadMotion;
		else
			activeHeadMotion = mouseHeadMotion;
	}
	
	public float GetOculusAngleFromAnchor(){
		float result = AngleUtils.GetSignedAngle(transform.parent.forward, transform.forward, transform.up);
		return result;
	}
	
	public float GetCamAngleFromLaser(Vector3 camForward){
		float result = AngleUtils.GetSignedAngle(transform.parent.forward, camForward, transform.up);
		print (result);
		
		return result;
	}
	
	public float GetLaserAngleFromAnchor(){
		float result = AngleUtils.GetSignedAngle(transform.parent.forward, Vector3.forward, Vector3.up);
		
		print (result);
		
		return result;
	}
}