using UnityEngine;
using System.Collections;

public class HealthBarGui : MonoBehaviour {
	
	public Texture ImageCrosshair = null;
	public float   StereoSpread  	  = 0.0f;
	
	private float  LensOffsetLeft     = 0.0f;
	private float  LensOffsetRight    = 0.0f;
	
	private float XL = 0.0f;
	private float YL = 0.0f;
	
	void Start () {
		
		// Initialize screen location of cursor
		XL = Screen.width * 0.25f;
		YL = Screen.height * 0.5f;
		
		OVRDevice.GetPhysicalLensOffsets(ref LensOffsetLeft, ref LensOffsetRight);
	}
	
	void Update () {
	}
	
	void OnGUI(){
		/*
		if ((DisplayCrosshair == true) && (CollisionWithGeometry == false))
			FadeVal += Time.deltaTime / FadeTime;
		else
			FadeVal -= Time.deltaTime / FadeTime;
		
		FadeVal = Mathf.Clamp(FadeVal, 0.0f, 1.0f);
		
		// Check to see if crosshair influences mouse rotation
		OVRPlayerController.AllowMouseRotation = false;
		
		if ((ImageCrosshair != null) && (FadeVal != 0.0f))
		{
			// Assume cursor is on-screen (unless it goes into the dead-zone)
			// Other systems will check this to see if it is false for example 
			// allowing rotation to take place
			OVRPlayerController.AllowMouseRotation = true;

			GUI.color = new Color(1, 1, 1, FadeVal * FadeScale);
			
			float ah = StereoSpread / 2.0f  // required to adjust for physical lens shift
			      - 0.5f * ((LensOffsetLeft * (float)Screen.width / 2));
			
			// Calculate X
			XL += Input.GetAxis("Mouse X") * 0.5f * ScaleSpeedX;
			if(XL < DeadZoneX) 
			{
				OVRPlayerController.AllowMouseRotation = false;
				XL = DeadZoneX - 0.001f;	
			}
			else if (XL > (Screen.width * 0.5f) - DeadZoneX)
			{
				OVRPlayerController.AllowMouseRotation = false;
				XL = Screen.width * 0.5f - DeadZoneX + 0.001f;
			}
			
			// Calculate Y
			YL -= Input.GetAxis("Mouse Y") * ScaleSpeedY;
			if(YL < DeadZoneY) 
			{
				//CursorOnScreen = false;
				if(YL < 0.0f) YL = 0.0f;
			}
			else if (YL > Screen.height - DeadZoneY)
			{
				//CursorOnScreen = false;
				if(YL > Screen.height) YL = Screen.height;
			}
			*/
			// Finally draw cursor
			//if(OVRPlayerController.AllowMouseRotation == true)
			//{
				// Left
		
			float ah = StereoSpread / 2.0f  // required to adjust for physical lens shift
			      - 0.5f * ((LensOffsetLeft * (float)Screen.width / 2));
		
				GUI.DrawTexture(new Rect(	XL - (ImageCrosshair.width * 0.5f),
					                     	YL - (ImageCrosshair.height * 0.5f), 
											ImageCrosshair.width,
											ImageCrosshair.height), 
											ImageCrosshair);
				
				float XR = XL + Screen.width * 0.5f;
				float YR = YL;
				
				// Right
				GUI.DrawTexture(new Rect(	XR - (ImageCrosshair.width * 0.5f),
											YR - (ImageCrosshair.height * 0.5f), 
											ImageCrosshair.width,
											ImageCrosshair.height), 
											ImageCrosshair);
			//}
				
			//GUI.color = Color.white;
		}
	}
