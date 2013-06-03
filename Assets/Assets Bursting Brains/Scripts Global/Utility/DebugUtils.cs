using UnityEngine;
using System.Collections;

public static class DebugUtils{

	public static void Assert(bool condition) {
    	if (!condition) throw new System.Exception();
	}
}
