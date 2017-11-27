using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayMarchController : MonoBehaviour {

    Material material;
	// Use this for initialization
	void Start () {
        material = GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, 1, 0));
        material.SetVector("_Scale", transform.localScale);
	}
}
