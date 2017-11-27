using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableCollider : MonoBehaviour
{
    public enum TYPE
    {
        SPHERE,
        CAPSULE,
        TYPE_NUM,
    }

    [SerializeField]
    SphereCollider _sphereCollider;
    [SerializeField]
    CapsuleCollider _capsuleCollider;

    float _limitTime;

    Actor _owner;


    void Awake ()
    {
        _sphereCollider.gameObject.SetActive(false);
        _capsuleCollider.gameObject.SetActive(false);
        _limitTime = 0.0f;
    }

    public void ActiveSphereCollider(float limitTime, Vector3 center, float radius)
    {
        this.gameObject.SetActive(true);
        _sphereCollider.center = center;
        _sphereCollider.radius = radius;
        _sphereCollider.gameObject.SetActive(true);
        Invoke("AutoDelete", limitTime);
    }

    public void ActiveCapsuleCollider(float limitTime, Vector3 center, int direction, float height, float radius)
    {
        this.gameObject.SetActive(true);
        _capsuleCollider.center = center;
        _capsuleCollider.direction = direction;
        _capsuleCollider.height = height;
        _capsuleCollider.radius = radius;
        _capsuleCollider.gameObject.SetActive(true);
        Invoke("AutoDelete", limitTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_owner.transform == other.transform) return;
        Actor hitActor = other.GetComponent<Actor>();
        if (!hitActor) return;
        hitActor.CallTakeDamage(10);

        this.gameObject.SetActive(false);
        _sphereCollider.gameObject.SetActive(false);
        _capsuleCollider.gameObject.SetActive(false);
    }

    private void AutoDelete()
    {
        this.gameObject.SetActive(false);
        _sphereCollider.gameObject.SetActive(false);
        _capsuleCollider.gameObject.SetActive(false);
    }
}
