using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : MonoBehaviour {
    [SerializeField]
    GameObject colliderPrefab;

    int _colliderNum;
    SkillCollider[] _colliders;
    public static ColliderManager Instance
    {
        get;
        private set;
    }
    void Awake() {
		if(Instance != null)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this.gameObject);
        Instance = this;
        Initialize(100);
    }

    private void Update()
    {
        for (int i = 0; i < _colliderNum; i++)
        {
            if (_colliders[i].gameObject.activeSelf) continue;
            _colliders[i].MyUpdate();
        }
    }

    public void Initialize(int colliderNum)
    {
        _colliderNum = colliderNum;
        _colliders = new SkillCollider[_colliderNum];
        for (int i = 0; i < _colliderNum; i++)
        {
            GameObject obj = Instantiate(colliderPrefab);
            _colliders[i] = obj.GetComponent<SkillCollider>();
            _colliders[i].Create();
            obj.gameObject.transform.SetParent(transform);
            obj.gameObject.SetActive(false);
        }
    }

    public SkillCollider GetCollider()
    {
        for (int i = 0; i < _colliderNum; i++)
        {
            if (_colliders[i].gameObject.activeSelf) continue;
            return _colliders[i];
        }
        Debug.Log("当たり判定が足りなくなりました");
        return null;
    }
}
