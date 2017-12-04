using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableCollider : MonoBehaviour
{
    public static readonly int COLLISION_PLAYER_ATTACK = 9;
    public static readonly int COLLISION_MONSTER_ATTACK = 10;
    public static readonly int COLLISION_PLAYER_BODY = 11;
    public static readonly int COLLISION_MONSTER_BODY = 12;

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

    int _damage;

    Actor _owner;


    void Awake ()
    {
        _sphereCollider.gameObject.SetActive(false);
        _capsuleCollider.gameObject.SetActive(false);
        _limitTime = 0.0f;
        _damage = 0;
    }

    public SphereCollider ActiveSphereCollider(int layerName, Actor owner, float limitTime, int damage, Vector3 center, float radius)
    {
        _owner = owner;
        _damage = damage;
        this.gameObject.SetActive(true);
        transform.position = center;
        _sphereCollider.radius = radius;
        _sphereCollider.gameObject.SetActive(true);
        _sphereCollider.gameObject.layer = layerName;
        Invoke("AutoDelete", limitTime);

        return _sphereCollider;
    }

    public CapsuleCollider ActiveCapsuleCollider(int layerName, Actor owner, float limitTime, int damage, Vector3 center, int direction, float height, float radius)
    {
        _owner = owner;
        _damage = damage;
        this.gameObject.SetActive(true);
        transform.position = center;
        _capsuleCollider.direction = direction;
        _capsuleCollider.height = height;
        _capsuleCollider.radius = radius;
        _capsuleCollider.gameObject.SetActive(true);
        _capsuleCollider.gameObject.layer = layerName;
        Invoke("AutoDelete", limitTime);

        return _capsuleCollider;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_owner == null) return;
        if (_owner.transform == other.transform) return;
        Actor hitActor = other.GetComponent<Actor>();
        if (!hitActor) return;
        hitActor.CallTakeDamage(_damage);

        this.gameObject.SetActive(false);
        _sphereCollider.gameObject.SetActive(false);
        _capsuleCollider.gameObject.SetActive(false);

        Debug.Log(other.name);
    }

    private void AutoDelete()
    {
        this.gameObject.SetActive(false);
        _sphereCollider.gameObject.SetActive(false);
        _capsuleCollider.gameObject.SetActive(false);
    }

    public Actor GetOwner()
    {
        return _owner;
    }
}
