using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeView : MonoBehaviour {

    [SerializeField]
    Shader shader;

    Material material;
	void Awake () {
        material = new Material(shader);
        GetComponent<MeshRenderer>().material = material;
	}

    public void SetColor(Color color)
    {
        material.SetColor("_Color", color);
    }
    public void SetFan_Range(float range)
    {
        material.SetFloat("_Range", range);
    }
}
