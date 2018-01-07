using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUI : MonoBehaviour {

    float _time;
	void Start () {
        _time = 0.0f;
    }
	
	public void MyUpdate () {
        Vector3 dir = transform.position - Camera.main.transform.position;
        transform.LookAt(dir,Vector3.up);
        float size = Mathf.Sin(_time * 3.14f) + 0.5f;
        transform.localScale = new Vector3(size, size, size);
        _time += Time.deltaTime;
        if(_time >= 1.0f)
        {
            Dispose();
        }
	}

    public void Initialize(Vector3 pos, Color color)
    {
        gameObject.SetActive(true);
        transform.position = pos;
        GetComponent<TextMesh>().color = color;
        _time = 0.0f;
        float size = Mathf.Sin(_time * 2) + 0.5f;
        transform.localScale = new Vector3(size, size, size);
    }

    void Dispose()
    {
        gameObject.SetActive(false);
    }
}
