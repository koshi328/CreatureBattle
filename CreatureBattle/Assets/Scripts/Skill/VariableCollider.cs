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

    // この当たり判定と当たったオブジェクトのインスタンスIDのリスト
    List<int> _instanceIDs;

    // この当たり判定で付与する状態異常のリスト
    List<StatusAilment.StatusAilmentBase> _statusAilments;


    void Awake ()
    {
        _type = COLLIDER_TYPE.SPHERE;
        _sphereCollider.gameObject.SetActive(false);
        _capsuleCollider.gameObject.SetActive(false);
        _limitTime = 0.0f;
        _damage = 0;
    }

    private void Initialize()
    {
        _instanceIDs = null;
        _statusAilments = null;
    }

    public SphereCollider EntrySphereCollider(int layerName, Actor owner, float limitTime, int damage, Vector3 center, float radius, List<StatusAilment.StatusAilmentBase> statusAilments)
    {
        Initialize();
        _type = COLLIDER_TYPE.SPHERE;
        _owner = owner;
        _damage = damage;
        this.gameObject.SetActive(true);
        transform.position = center;
        _sphereCollider.radius = radius;
        _sphereCollider.gameObject.SetActive(true);
        _sphereCollider.gameObject.layer = layerName;
        _instanceIDs = new List<int>();
        _statusAilments = statusAilments;
        Invoke("AutoDelete", limitTime);

        return _sphereCollider;
    }

    public CapsuleCollider EntryCapsuleCollider(int layerName, Actor owner, float limitTime, int damage, Vector3 center, int direction, float height, float radius, List<StatusAilment.StatusAilmentBase> statusAilments)
    {
        Initialize();
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
        _instanceIDs = new List<int>();
        _statusAilments = statusAilments;
        Invoke("AutoDelete", limitTime);

        return _capsuleCollider;
    }

    public SphereCollider EntryFanCollider(int layerName, Actor owner, float limitTime, int damage, Vector3 center, float radius, Vector3 currentAngle, float angleRange, List<StatusAilment.StatusAilmentBase> statusAilments)
    {
        Initialize();
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
        _instanceIDs = new List<int>();
        _statusAilments = statusAilments;
        Invoke("AutoDelete", limitTime);

        return _sphereCollider;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_owner == null) return;
        if (_owner.transform == other.transform) return;

        // 既に当たっていたら処理しない
        for (int i = 0; i < _instanceIDs.Count; i++)
        {
            if (_instanceIDs[i] == other.GetInstanceID()) return;
        }

        // 既に当たったリストに追加
        _instanceIDs.Add(other.GetInstanceID());

        // 扇形の判定の時
        if (_type == COLLIDER_TYPE.FAN)
        {
            if (!IsCollideFan(other.transform)) return;
        }

        // キャラかどうか
        Actor hitActor = other.GetComponent<Actor>();
        if (!hitActor) return;
        
        // 当たった時の処理
        // ダメージ
        hitActor.CallTakeDamage(_damage);
        // 状態異常を付与する
        if (_statusAilments != null)
        {
            for (int i = 0; i < _statusAilments.Count; i++)
            {
                // 当たったやつが状態異常になる
                _statusAilments[i].SetActor(hitActor);
                _owner.AddStatusAilment(_statusAilments[i]);
            }
        }
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
