using UnityEngine;
using System.Collections;

public abstract class MenuGui : MonoBehaviour {

	public abstract bool IsGuiOn();
	public abstract void EnterGui();
	public abstract void ExitGui();
}
