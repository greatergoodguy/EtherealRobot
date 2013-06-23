using UnityEngine;
using System.Collections;

//public class MouseLook_Ethereal : MonoBehaviour {
public class MouseLook_Ethereal : PlayerController {

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 2F;
    public float sensitivityY = 2F;
    public float maxAngleVert = 80F;
    public float maxAngleHori = 80F;
    public float maxAngle = 80F;
	private float smoothTime = 5f;
	private float xTarget, xCurrent = 0f;
	private float yTarget, yCurrent = 0f;
	private float xVelo, yVelo;
    private float rotationY = 0F;		// TODO: DEPRECATE ME
	private HeadMotion mouseHeadMotion;
	private HeadMotion ovrHeadMotion;
	private HeadMotion activeHeadMotion;
	//private Quaternion initialQuaternion;
	//private Vector3 initialForward;
	
	void Start () {
		mouseHeadMotion = transform.FindChild("Camera").GetComponent<HeadMotion>();
		ovrHeadMotion = transform.FindChild("OVRCameraController").GetComponent<HeadMotion>();
		activeHeadMotion = mouseHeadMotion;
		
		DebugUtils.Assert(mouseHeadMotion != null);
		DebugUtils.Assert(ovrHeadMotion != null);
		
		//initialQuaternion = transform.localRotation;
		//initialForward = transform.forward;
    }
	
	void FixedUpdate () {
	}
	
    void Update () {
    
        if (axes == RotationAxes.MouseXAndY) {
            //float rotationX = transform.localEulerAngles.y + Input.GetAxis("Xbox_Horizontal") * sensitivityX;
			if (activeHeadMotion.ToString().Equals("MouseHeadMotion")) {
				//Debug.Log(activeHeadMotion.ToString ());
				/* Get horizontal rotation */
				yTarget += activeHeadMotion.GetHeadHorizontalAxis() * sensitivityX;
				yTarget = Mathf.Clamp (yTarget, -maxAngleHori, maxAngleHori);
				yCurrent = Mathf.SmoothDamp (yCurrent, yTarget, ref yVelo, Time.deltaTime * smoothTime);
				/* Get vertical rotation: current coded for non-inverted look only */
				xTarget += activeHeadMotion.GetHeadVerticalAxis() * -sensitivityY;
				xTarget = Mathf.Clamp (xTarget, -maxAngleVert, maxAngleVert);
				xCurrent = Mathf.SmoothDamp (xCurrent, xTarget, ref xVelo, Time.deltaTime * smoothTime);
				transform.localRotation = Quaternion.Euler (xCurrent, yCurrent, 0f);
				//Debug.Log ("Head's Forward Vector = "+transform.forward.x+", "+transform.forward.y+", "+transform.forward.z);
				//Debug.Log ("Head's Up Vector = "+transform.up.x+", "+transform.up.y+", "+transform.up.z);
			} else {
				//Debug.Log(activeHeadMotion.ToString ());
				/* Get horizontal rotation */
				float ovrAxisY = 180f*activeHeadMotion.GetHeadHorizontalAxis();
				/* Get vertical rotation: current coded for non-inverted look only */
				float ovrAxisX = 180f*activeHeadMotion.GetHeadVerticalAxis();
				ovrAxisX = Mathf.Clamp (ovrAxisX, -85f, 85f);
				transform.localRotation = Quaternion.Euler (
					ovrAxisX, 
					ovrAxisY, 
					transform.localRotation.z);
			}
		}	
        /* Only horizontal or only vertical control modes
         * TODO: MIGHT NOT NEED THESE LAST TWO CONTROL MODES 
        } else if (axes == RotationAxes.MouseX) {
			//Debug.Log("In MouseX - MouseLook_Ethereal.cs");
			if (activeHeadMotion == mouseHeadMotion) {
				float horizontalAxis = activeHeadMotion.GetHeadHorizontalAxis() * sensitivityX;
				transform.Rotate (0, horizontalAxis, 0);	
			} else {
				float ovrAxis = activeHeadMotion.GetHeadHorizontalAxis();
				transform.localRotation = Quaternion.AngleAxis(180 * ovrAxis, transform.up);
			}
        } else {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp (rotationY, -maxAngleVert, maxAngleVert);
            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }
        */
    }
	
	/* Helper Methods
	 * TODO: IS DEPRECATED? WHEN ARE THESE CALLED? */
	public void SwitchHeadMotion () {
		if (activeHeadMotion.ToString().Equals("MouseHeadMotion")) 
			activeHeadMotion = ovrHeadMotion;
		else 
			activeHeadMotion = mouseHeadMotion;
	}
	public float GetOculusAngleFromAnchor () {
		return AngleUtils.GetSignedAngle(transform.parent.forward, transform.forward, transform.up);
	}
	public float GetCamAngleFromLaser (Vector3 camForward) {
		return AngleUtils.GetSignedAngle(camForward, transform.parent.forward, transform.up);		
	}
	public float GetLaserAngleFromAnchor () {
		return AngleUtils.GetSignedAngle(transform.parent.forward, Vector3.forward, Vector3.up);
	}
	/* TODO: REFACTOR ARCHITECTURE- 
	 * Empty inherited methods from abstract parent */
	public override string GetControllerName() { return "EtherealLook"; }
	public override void SwitchCameraController() {}
	public override DebugData GetDebugData() { return debugData; }
}