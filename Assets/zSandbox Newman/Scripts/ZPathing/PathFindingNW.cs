using UnityEngine;
using System.Collections;

public class PathFindingNW : MonoBehaviour
{
    public Transform Target;
    public float MoveSpeed = 3f;
    public float RotationSpeed = 3f;

    private Transform _myTransform;

    void Awake()
    {
        _myTransform = transform;
    }

    void Start()
    {
        Target = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        _myTransform.rotation = Quaternion.Slerp(_myTransform.rotation, Quaternion.LookRotation(Target.position - _myTransform.position), RotationSpeed * Time.deltaTime);
        _myTransform.position += _myTransform.forward*MoveSpeed*Time.deltaTime;
    }
}

/*using UnityEngine;
using System.Collections;

public class ThrowScriptNW : MonoBehaviour {
	
	public Transform target;
	public float speed = 3f;
	public float rotation = 3f;
	
	private Transform myTransform;
	
	void Awake () {
		
		myTransform = transform;
	}
	
	// Use this for initialization
	void Start () {
		 target = GameObject.FindWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(Vector3(target.position.x, transform.position.y, target.position.z));

	}
}*/
