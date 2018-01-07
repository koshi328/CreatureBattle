using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorUI : MonoBehaviour {

    Transform _myTransform;
    void Start()
    {
        _myTransform = transform;
    }

	void Update () {
        _myTransform.LookAt(Camera.main.transform, Vector3.up);
	}
}
