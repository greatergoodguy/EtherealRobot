using UnityEngine;
using System.Collections;

public class rotateSphere : MonoBehaviour {

	public float spin = 5f;
	// Update is called once per frame
	void Update () {
		transform.Rotate (spin, spin, 0f);
	}
}
