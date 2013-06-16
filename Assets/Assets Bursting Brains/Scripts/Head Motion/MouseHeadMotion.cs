using UnityEngine;
using System.Collections;

public class MouseHeadMotion : HeadMotion {
	public override float GetHeadHorizontalAxis() {
		return InputManager.activeInput.GetAxis_MouseX();
	}
	public override float GetHeadVerticalAxis() {
		return InputManager.activeInput.GetAxis_MouseY();
	}
	public override string ToString () {
		return string.Format ("MouseHeadMotion");
	}
}
