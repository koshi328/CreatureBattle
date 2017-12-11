using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatusAilment;

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
    [SerializeField]
    MeshFilter _meshFilter;
    [SerializeField]
    Mesh _sphereMesh;
    [SerializeField]
    Mesh _capsuleMesh;

    float _limitTime;

    int _damage;

    Actor _owner;

    // 角度
    float _angleRange;

    // この当たり判定と当たったオブジェクトのインスタンスIDのリスト
    List<int> _instanceIDs;

    // この当たり判定で付与する状態異常のリスト
    StatusAilmentBase[] _statusAilments;


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

    public SphereCollider EntrySphereCollider(int layerName, Actor owner, float limitTime, int damage, Vector3 center, float radius, StatusAilmentBase[] statusAilments)
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
        _sphereCollider.transform.localScale = new Vector3(radius * 2.0f, radius * 2.0f, radius * 2.0f);
        _meshFilter.mesh = _sphereMesh;
        _instanceIDs = new List<int>();
        _statusAilments = statusAilments;
        Invoke("AutoDelete", limitTime);

        return _sphereCollider;
    }

    public CapsuleCollider EntryCapsuleCollider(int layerName, Actor owner, float limitTime, int damage, Vector3 center, int direction, float height, float radius, StatusAilmentBase[] statusAilments)
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
        _meshFilter.mesh = _capsuleMesh;
        _instanceIDs = new List<int>();
        _statusAilments = statusAilments;
        Invoke("AutoDelete", limitTime);

        return _capsuleCollider;
    }

    public SphereCollider EntryFanCollider(int layerName, Actor owner, float limitTime, int damage, Vector3 center, float radius, Vector3 currentAngle, float angleRange, StatusAilmentBase[] statusAilments)
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
        _sphereCollider.transform.localScale = new Vector3(radius * 2.0f, radius * 2.0f, radius * 2.0f);
        _meshFilter.mesh = _sphereMesh;
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
            // カウンタースタン
            if (hitActor.HaveStatusAilment(KIND.COUNTER_STAN))
            {
                // 状態異常攻撃をした者にスタンをかけて状態異常を受けない
                _owner.CallAddStatusAilment((int)KIND.STAN, 5.0f);
                return;
            }

            for (int i = 0; i < _statusAilments.Length; i++)
            {
                switch(_statusAilments[i]._kind)
                {
                        // 状態異常
                    case KIND.STAN:
                    case KIND.SILENCE:
                    case KIND.BAN_REC:
                        hitActor.CallAddStatusAilment((int)_statusAilments[i]._kind, _statusAilments[i]._limitTime);
                        break;
                        
                        // スリップダメージ
                    case KIND.BURN:
                        var burn = (StatusBurn)_statusAilments[i];
                        hitActor.CallAddStatusAilment2((int)_statusAilments[i]._kind, _statusAilments[i]._limitTime, burn._damage, burn._damageInterval);
                        break;

                        // ステータスバフデバフ
                    case KIND.ATK_UP:
                    case KIND.DEF_UP:
                    case KIND.MOV_UP:
                    case KIND.REC_UP:
                    case KIND.ATK_SPD_UP:
                    case KIND.ATK_DOWN:
                    case KIND.DEF_DOWN:
                    case KIND.MOV_DOWN:
                    case KIND.REC_DOWN:
                    case KIND.MOV_DOWN_DUP:
                        var buff = (StatusBuff)_statusAilments[i];
                        hitActor.CallAddStatusAilment3((int)_statusAilments[i]._kind, _statusAilments[i]._limitTime, buff._rate);
                        break;
                }
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

        _instanceIDs = null;
        _statusAilments = null;
    }

    public Actor GetOwner()
    {
        return _owner;
    }
}
