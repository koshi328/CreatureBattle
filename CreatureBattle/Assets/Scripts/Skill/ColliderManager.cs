using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// コライダー管理
/// </summary>
public class ColliderManager : MonoBehaviour
{
    // シングルトンのインスタンス
    static ColliderManager _instance;
    public static ColliderManager GetInstance()
    {
        if (_instance == null)
        {
            GameObject go = new GameObject();
            _instance = go.AddComponent<ColliderManager>();
        }
        return _instance;
    }

    // コライダーのプール数
    static readonly int MAX_NUM = 50;

    // プレハブ
    [SerializeField]
    private GameObject _variableColliderPrefab;

    // コライダーの配列
    [SerializeField]
    VariableCollider[] _colliders;


    /// <summary>
    /// 初期化
    /// </summary>
	void Awake ()
    {
        _instance = this;

        _colliders = new VariableCollider[(int)MAX_NUM];
        for (int i = 0; i < MAX_NUM; i++)
        {
            GameObject obj = Instantiate(_variableColliderPrefab, Vector3.zero, Quaternion.identity, this.transform);
            _colliders[i] = obj.GetComponent<VariableCollider>();
            _colliders[i].gameObject.SetActive(false);
        }
	}
	

    /// <summary>
    /// 当たり判定をアクティブにする
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
        // 使用中でないコライダーを使用中にして返す
        for (int i = 0; i < MAX_NUM; i++)
        {
            if (_colliders[i].gameObject.GetActive() == true) continue;
            return _colliders[i].Entry(radius, height, direction, center, owner, layerName, limitTime);
        }

        return null;
    }
}
