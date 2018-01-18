using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBillboard : MonoBehaviour {

	void Start () {
		
	}
	
	void LateUpdate () {
        this.transform.LookAt(transform.position + Camera.main.transform.forward, Vector3.up);
    }
}
