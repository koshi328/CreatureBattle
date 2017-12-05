using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableCollider : MonoBehaviour
{
    public static readonly int COLLISION_PLAYER_ATTACK = 9;
    public static readonly int COLLISION_MONSTER_ATTACK = 10;
    public static readonly int COLLISION_PLAYER_BODY = 11;
    public static readonly int COLLISION_MONSTER_BODY = 12;

    public enum COLLIDER_TYPE
    {
        SPHERE,
        CAPSULE,
        FAN,
        TYPE_NUM,
    }

    COLLIDER_TYPE _type;

    [SerializeField]
    SphereCollider _sphereCollider;
    [SerializeField]
    CapsuleCollider _capsuleCollider;

    float _limitTime;

    int _damage;

    Actor _owner;

    // 角度
    float _angleRange;


    void Awake ()
    {
        _type = COLLIDER_TYPE.SPHERE;
        _sphereCollider.gameObject.SetActive(false);
        _capsuleCollider.gameObject.SetActive(false);
        _limitTime = 0.0f;
        _damage = 0;
    }

    public SphereCollider EntrySphereCollider(int layerName, Actor owner, float limitTime, int damage, Vector3 center, float radius)
    {
        _type = COLLIDER_TYPE.SPHERE;
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

    public CapsuleCollider EntryCapsuleCollider(int layerName, Actor owner, float limitTime, int damage, Vector3 center, int direction, float height, float radius)
    {
        _type = COLLIDER_TYPE.CAPSULE;
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

    public SphereCollider EntryFanCollider(int layerName, Actor owner, float limitTime, int damage, Vector3 center, float radius, Vector3 currentAngle, float angleRange)
    {
        _type = COLLIDER_TYPE.FAN;
        _owner = owner;
        _damage = damage;
        this.gameObject.SetActive(true);
        transform.position = center;
        _sphereCollider.radius = radius;
        _sphereCollider.gameObject.SetActive(true);
        _sphereCollider.gameObject.layer = layerName;
        transform.eulerAngles = currentAngle;
        _angleRange = angleRange;
        Invoke("AutoDelete", limitTime);

        return _sphereCollider;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_owner == null) return;
        if (_owner.transform == other.transform) return;

        // 扇形の判定の時
        if (_type == COLLIDER_TYPE.FAN)
        {
            if (!IsCollideFan(other.transform)) return;
        }
        Debug.Log("hit");
        Actor hitActor = other.GetComponent<Actor>();
        if (!hitActor) return;
        hitActor.CallTakeDamage(_damage);

        this.gameObject.SetActive(false);
        _sphereCollider.gameObject.SetActive(false);
        _capsuleCollider.gameObject.SetActive(false);
    }

    private bool IsCollideFan(Transform other)
    {
        Vector3 pos = transform.position;
        Vector3 targetDir = other.transform.position - pos;
        targetDir.y = 0.0f;
        float angle = Vector3.Angle(targetDir, transform.forward);

        if(angle < _angleRange)
        {
            return true;
        }
        else
        {
            return false;
        }
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
