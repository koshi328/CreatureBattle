using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {

    RectTransform _myTransform;
    private void Start()
    {
        _myTransform = GetComponent<RectTransform>();
    }
    void LateUpdate () {

        _myTransform.LookAt(Camera.main.transform, Vector3.up);
	}
}
