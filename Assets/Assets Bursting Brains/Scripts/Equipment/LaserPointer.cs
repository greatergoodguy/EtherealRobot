using UnityEngine;
using System.Collections;

public class LaserPointer : MonoBehaviour {
	
	private const float LINE_WIDTH = 0.02f;
	
	public Color color1 = Color.red;
	public Color color2 = Color.magenta;
	
	public LineRenderer lrHead;
	public LineRenderer lrVelo;
	private GameObject head;
	
	void Start () {
		head = transform.Find("Head").gameObject;
		lrHead = (LineRenderer) head.AddComponent("LineRenderer");
		lrHead.material = new Material(Shader.Find("Particles/Additive"));
		lrHead.SetColors(color2, color2);
		lrHead.SetWidth(LINE_WIDTH, LINE_WIDTH);
		lrHead.SetVertexCount(2);
		
		lrVelo = (LineRenderer) gameObject.AddComponent("LineRenderer");
		lrVelo.material = new Material(Shader.Find("Particles/Additive"));
		lrVelo.SetColors(color1, color1);
		lrVelo.SetWidth(LINE_WIDTH, LINE_WIDTH);
		lrVelo.SetVertexCount(2);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 origin = head.transform.position;
		Vector3 forwardDirection = head.transform.forward;
		Vector3 endPoint = origin + forwardDirection * 888f;
		origin.y = origin.y + 0.1f;
		RaycastHit hit = new RaycastHit();
	
		lrHead.SetPosition(0, origin);
		//if (Physics.Raycast (origin, forwardDirection, out hit))
		//	endPoint = hit.point;
		lrHead.SetPosition(1, endPoint);
	
		Vector3 origin2 = transform.position;
		origin2.y = origin2.y + 0.1f;
		lrVelo.SetPosition(0, origin2);
		//lrVelo.SetPosition(1, origin2 + transform.rigidbody.velocity);	// Velocity Vector Laser
		lrVelo.SetPosition(1, origin2 + transform.forward * 888f);			// Forward Vector Laser
	}
}
