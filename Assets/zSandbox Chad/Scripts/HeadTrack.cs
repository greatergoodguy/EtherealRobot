using UnityEngine;
using System.Collections;

public class HeadTrack : MonoBehaviour {

public Transform target;

    void Start () {

    }

    void Update () {

       transform.rotation = target.rotation;

    }

}
