using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {

    Transform _myTransform;
    private void Start()
    {
        _myTransform = transform;
    }
    public virtual void Movement(Vector3 dir, float speed)
    {
        _myTransform.LookAt(Vector3.Lerp(_myTransform.position + dir, _myTransform.position + _myTransform.forward, 0.2f));
        _myTransform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public virtual void Action()
    {

    }
}
