using UnityEngine;
using System.Collections;

public class MouseHeadMotion : HeadMotion {
	public override float GetHeadHorizontalAxis(){
		return Input.GetAxis("Mouse X");	
	}
}
