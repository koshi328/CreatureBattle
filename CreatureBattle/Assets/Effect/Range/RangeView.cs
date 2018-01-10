using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeView : MonoBehaviour {

    [SerializeField]
    Shader shader;

    Material material;
    
    Vector3 _size;
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
    public void SetSize(Vector3 size)
    {
        _size = size;
    }
    public void Dispose()
    {

    }

    private void Update()
    {
        float sx = transform.localScale.x + (_size.x - transform.localScale.x) * 0.1f;
        float sy = transform.localScale.y + (_size.y - transform.localScale.y) * 0.1f;
        transform.localScale = new Vector3(sx, sy, 1);
    }

}
