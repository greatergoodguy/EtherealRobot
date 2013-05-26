using UnityEngine;
using System.Collections;

public class LaserPointer : MonoBehaviour {
	
	private Color color1 = Color.red;
	private Color color2 = Color.red;
	
	private LineRenderer lineRenderer;
	
	void Start () {
		lineRenderer = (LineRenderer) gameObject.AddComponent("LineRenderer");
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.SetColors(color1, color2);
		lineRenderer.SetWidth(1, 1);
		lineRenderer.SetVertexCount(2);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 origin = transform.position;
		Vector3 forwardDirection = transform.forward;
		Vector3 endPoint = origin + forwardDirection * 10000;
		
		/*
		RaycastHit hit = null;
		
		lineRenderer.SetPosition(0, endPoint);
		
		if(Physics.Raycast(origin, forwardDirection, hit)){
			endPoint = hit.point;
			lineRenderer.SetPosition(1, endPoint);
		}
		*/
	
	}
}
