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

    float _limitTime;

    int _damage;

    int _damageRange;

    Actor _owner;

    // 角度
    float _angleRange;

    // この当たり判定と当たったオブジェクトのインスタンスIDのリスト
    List<int> _instanceIDs;

    // この当たり判定で付与する状態異常のリスト
    StatusAilmentBase[] _statusAilments;
    
    public delegate void SkillDelegate(Actor actor);
    SkillDelegate hitDelegate = null;

    public void SetDelegate(SkillDelegate argDelgate)
    {
        hitDelegate = argDelgate;
    }
    // デバッグ用　あとで消すべし
    [SerializeField]
    MeshFilter _meshFilter;
    [SerializeField]
    Mesh _sphereMesh;
    [SerializeField]
    Mesh _capsuleMesh;
    

    void Awake ()
    {
        _type = COLLIDER_TYPE.SPHERE;
        _sphereCollider.gameObject.SetActive(false);
        _capsuleCollider.gameObject.SetActive(false);
        _limitTime = 0.0f;
        _damage = 0;
        _damageRange = 0;
    }

    private void Initialize()
    {
        _instanceIDs = null;
        _statusAilments = null;
        hitDelegate = null;
    }

    public SphereCollider EntrySphereCollider(int layerName, Actor owner, float limitTime, int damage, int damageRange, Vector3 center, float radius, StatusAilmentBase[] statusAilments)
    {
        Initialize();
        _type = COLLIDER_TYPE.SPHERE;
        _owner = owner;
        _damage = damage;
        _damageRange = damageRange;
        this.gameObject.SetActive(true);
        transform.position = center;
        //_sphereCollider.radius = radius;
        _sphereCollider.gameObject.SetActive(true);
        _sphereCollider.gameObject.layer = layerName;
        _sphereCollider.transform.rotation = Quaternion.identity;
        _meshFilter.transform.localScale = new Vector3(radius * 2.0f, radius * 2.0f, radius * 2.0f);
        _meshFilter.mesh = _sphereMesh;
        _instanceIDs = new List<int>();
        _statusAilments = statusAilments;
        Invoke("AutoDelete", limitTime);

        return _sphereCollider;
    }

    public CapsuleCollider EntryCapsuleCollider(int layerName, Actor owner, float limitTime, int damage, int damageRange, Vector3 center, int direction, float height, float radius, StatusAilmentBase[] statusAilments)
    {
        Initialize();
        _type = COLLIDER_TYPE.CAPSULE;
        _owner = owner;
        _damage = damage;
        _damageRange = damageRange;
        this.gameObject.SetActive(true);
        transform.position = center;
        _capsuleCollider.direction = direction;
        _capsuleCollider.height = height;
        //_capsuleCollider.radius = radius;
        _capsuleCollider.gameObject.SetActive(true);
        _capsuleCollider.gameObject.layer = layerName;
        _capsuleCollider.transform.rotation = Quaternion.identity;
        _meshFilter.transform.localScale = new Vector3(radius * 2.0f, height, radius * 2.0f);
        _meshFilter.mesh = _capsuleMesh;
        _instanceIDs = new List<int>();
        _statusAilments = statusAilments;
        Invoke("AutoDelete", limitTime);

        return _capsuleCollider;
    }

    public SphereCollider EntryFanCollider(int layerName, Actor owner, float limitTime, int damage, int damageRange, Vector3 center, float radius, Vector3 currentAngle, float angleRange, StatusAilmentBase[] statusAilments)
    {
        Initialize();
        _type = COLLIDER_TYPE.FAN;
        _owner = owner;
        _damage = damage;
        _damageRange = damageRange;
        this.gameObject.SetActive(true);
        transform.position = center;
        //_sphereCollider.radius = radius;
        _sphereCollider.gameObject.SetActive(true);
        _sphereCollider.gameObject.layer = layerName;
        _sphereCollider.transform.rotation = Quaternion.identity;
        _meshFilter.transform.localScale = new Vector3(radius * 2.0f, radius * 2.0f, radius * 2.0f);
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

        // プロテクト状態なら
        if(hitActor.HaveStatusAilment(KIND.STADY_PROTECT))
        {
            hitActor.CallRefreshStatusAilment((int)KIND.STADY_PROTECT);
            return;
        }

        // 当たった時の処理
        // ダメージ
        int damage = _damage + Random.Range(-_damageRange, _damageRange + 1);
        hitActor.CallTakeDamage(damage);
        
        // 状態異常を付与する
        if (_statusAilments != null)
        {
            // カウンタースタン
            if (hitActor.HaveStatusAilment(KIND.COUNTER_STAN))
            {
                // 状態異常攻撃をした者にスタンをかけて、された方は状態異常を受けない
                _owner.CallAddStatusAilment((int)KIND.STAN, 5.0f);
                return;
            }

            for (int i = 0; i < _statusAilments.Length; i++)
            {
                // クレンズシステム状態なら炎上以外の状態異常は受けない
                if (hitActor.HaveStatusAilment(KIND.CLEANSE_SYSTEM))
                {
                    if (_statusAilments[i]._kind != KIND.BURN)
                    {
                        continue;
                    }
                }

                switch (_statusAilments[i]._kind)
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
                    case KIND.DAMAGE_CUT:
                    case KIND.MOV_UP:
                    case KIND.REC_UP:
                    case KIND.ATK_SPD_UP:
                    case KIND.ATK_DOWN:
                    case KIND.DAMAGE_UP:
                    case KIND.MOV_DOWN:
                    case KIND.REC_DOWN:
                    case KIND.MOV_DOWN_DUP:
                        var buff = (StatusBuff)_statusAilments[i];
                        hitActor.CallAddStatusAilment3((int)_statusAilments[i]._kind, _statusAilments[i]._limitTime, buff._rate);
                        break;
                }
            }
        }
        if (hitDelegate == null) return;
        hitDelegate(hitActor);
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
