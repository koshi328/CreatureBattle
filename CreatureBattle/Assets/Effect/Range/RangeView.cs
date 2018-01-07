using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeView : MonoBehaviour {

    [SerializeField]
    Shader shader;

    Material material;
	void Start () {
        material = new Material(shader);
        GetComponent<MeshRenderer>().material = material;
	}
	
}
