using UnityEngine;
using System.Collections;

public class LaserPointerReflector_Tom : MonoBehaviour {
	
	private const float LINE_WIDTH = 0.02f;
	
	public Color color1 = Color.red;
	public Color color2 = Color.red;
	
	public LineRenderer lineRenderer;
	
	public GameObject reflectionLightGO;
	
	void Start () {
		lineRenderer = (LineRenderer) gameObject.AddComponent("LineRenderer");
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.SetColors(color1, color2);
		lineRenderer.SetWidth(LINE_WIDTH, LINE_WIDTH);
		lineRenderer.SetVertexCount(3);
		
		reflectionLightGO = transform.parent.FindChild("ReflectionLight").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 origin = transform.position;
		Vector3 forwardDirection = transform.forward;
		
		//print(forwardDirection);
		
		Vector3 endPoint = origin + forwardDirection * 10000;
		
		
		RaycastHit hit = new RaycastHit();
		
		lineRenderer.SetPosition(0, origin);
		
		if(Physics.Raycast(origin, forwardDirection, out hit)){
			endPoint = hit.point;
				
			lineRenderer.SetPosition(1, endPoint);
			Vector3 reflectEndPoint = endPoint + hit.normal * 10000;
			lineRenderer.SetPosition(2, reflectEndPoint);
			
			reflectionLightGO.SetActive(true);
			reflectionLightGO.transform.position = hit.point;
			reflectionLightGO.transform.LookAt(reflectEndPoint);	
		}
		else{
			reflectionLightGO.SetActive(false);
			lineRenderer.SetPosition(1, endPoint);	
		}
		
		lineRenderer.SetPosition(1, endPoint);
	
	}
}
