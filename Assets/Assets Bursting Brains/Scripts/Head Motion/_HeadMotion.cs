using UnityEngine;
using System.Collections;

public abstract class HeadMotion : MonoBehaviour {
	
	public abstract float GetHeadHorizontalAxis();
	public abstract float GetHeadVerticalAxis();
	public abstract string ToString();
}
