using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingObject : MonoBehaviour {

    static ColliderManager colManager;
    BoxCollider collider;
    Vector3 direction = Vector3.zero;
    Transform _child;
    float _speed;

    private void Start()
    {
        if (colManager != null) return;
        colManager = ColliderManager.Instance;
    }
    void Update () {
        transform.position += direction * _speed * Time.deltaTime;
	}

    public void SetChildCollider(SkillCollider child,float destroyTime)
    {
        _child = child.transform;
        child.transform.parent = transform;
        child.AddDelegate((Actor argActor) => { Dispose(); });
        Invoke("Dispose", destroyTime);
        collider = GetComponent<BoxCollider>();
        collider.enabled = false;
        Invoke("EnableCollider", 0.5f);
    }

    public void SetDirection(Vector3 dir, float speed)
    {
        direction = dir;
        _speed = speed;
    }

    void Dispose()
    {
        _child.parent = colManager.transform;
        Destroy(this.gameObject);
    }

    void EnableCollider()
    {
        collider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Dispose();
    }
}
