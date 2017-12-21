using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatusAilment;


/// <summary>
/// コライダー
/// </summary>
public class VariableCollider : MonoBehaviour
{
    public static readonly int COLLISION_PLAYER_ATTACK = 9;
    public static readonly int COLLISION_MONSTER_ATTACK = 10;
    public static readonly int COLLISION_PLAYER_BODY = 11;
    public static readonly int COLLISION_MONSTER_BODY = 12;
    
    // コライダー
    [SerializeField]
    CapsuleCollider _collider;

    // 扇か？
    bool _isFan;

    // 角度
    float _angleRange;

    // 残存時間
    float _limitTime;
    
    // 当たり判定を発生させたキャラクター
    Actor _owner;

    // この当たり判定と当たったオブジェクトのインスタンスIDのリスト
    List<int> _instanceIDs;


    /// <summary>
    /// コールバック用の関数
    /// </summary>
    /// <param name="actor"></param>
    public delegate void SkillDelegate(Actor actor);
    SkillDelegate HitDelegateFunction;

    /// <summary>
    /// コールバックの関数を登録する
    /// </summary>
    /// <param name="argDelgate"></param>
    public void SetDelegate(SkillDelegate argDelgate)
    {
        Debug.Log("Delegate was set.");
        HitDelegateFunction = argDelgate;
    }

    /// <summary>
    /// 生成時の初期化
    /// </summary>
    void Awake ()
    {
        _collider.gameObject.SetActive(false);
        _isFan = false;
        _limitTime = 0.0f;
    }


    /// <summary>
    /// 使う度に呼び出す初期化
    /// </summary>
    private void Initialize()
    {
        _instanceIDs = null;
        HitDelegateFunction = null;
    }


    /// <summary>
    /// 当たり判定を登録する
    /// </summary>
    /// <param name="radius"></param>
    /// <param name="height"></param>
    /// <param name="direction"></param>
    /// <param name="center"></param>
    /// <param name="owner"></param>
    /// <param name="layerName"></param>
    /// <param name="limitTime"></param>
    /// <returns></returns>
    public VariableCollider Entry(float radius, float height, int direction, Vector3 center, Actor owner, int layerName, float limitTime)
    {
        // 初期化
        Initialize();
        _owner = owner;

        // コライダーの形状を変える
        _collider.radius = radius;
        _collider.height = height;
        _collider.direction = direction;

        // 座標
        transform.position = center;

        // 回転
        _collider.transform.rotation = Quaternion.identity;

        // PhysicsLayer名
        gameObject.layer = layerName;

        // 当たり判定のリストを初期化
        _instanceIDs = new List<int>();

        // アクティブにする
        gameObject.SetActive(true);

        // 残存時間が経過した時に非アクティブにする
        Invoke("AutoDelete", limitTime);

        return this;
    }


    /// <summary>
    /// 当たった時の処理
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // 使用者が居なければ処理しない
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
        if (_isFan)
        {
            // 当たっていなければ処理しない
            if (!IsCollideFan(other.transform)) return;
        }

        // キャラでなければ処理しない
        Actor hitActor = other.GetComponent<Actor>();
        if (!hitActor) return;

        // スキル毎の当たった時のコールバックを呼ぶ
        if (HitDelegateFunction == null) return;
        HitDelegateFunction(hitActor);
    }


    /// <summary>
    /// 扇状の当たり判定をする
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    private bool IsCollideFan(Transform other)
    {
        // 座標
        Vector3 pos = transform.position;
        Vector3 targetDir = other.transform.position - pos;
        targetDir.y = 0.0f;

        // 角度
        float angle = Vector3.Angle(targetDir, transform.forward);

        // 扇の範囲内なら当たっている
        if(angle < _angleRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    /// <summary>
    /// 残存時間が経過した時に呼ばれる非アクティブにする処理
    /// </summary>
    private void AutoDelete()
    {
        // アクティブでなくする
        gameObject.SetActive(false);

        // インスタンスIDのリストを消す
        _instanceIDs = null;
    }


    /// <summary>
    /// コライダーの持ち主を返す
    /// </summary>
    /// <returns></returns>
    public Actor GetOwner()
    {
        return _owner;
    }
}
