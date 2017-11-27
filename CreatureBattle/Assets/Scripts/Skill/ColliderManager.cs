using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : MonoBehaviour
{
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

    static readonly int MAX_NUM = 50;

    [SerializeField]
    private GameObject _variableCollider;
    [SerializeField]
    VariableCollider[] _colliders;

	void Awake ()
    {
        _instance = this;

        _colliders = new VariableCollider[(int)MAX_NUM];
        for (int i = 0; i < MAX_NUM; i++)
        {
            GameObject obj = Instantiate(_variableCollider, Vector3.zero, Quaternion.identity, this.transform);
            _colliders[i] = obj.GetComponent<VariableCollider>();
            _colliders[i].gameObject.SetActive(false);
            Debug.Log(_colliders[i]);
        }
	}
	
    public void ActiveSphereCollider(float limitTime, Vector3 center, float radius)
    {
        for (int i = 0; i < MAX_NUM; i++)
        {
            if (_colliders[i].gameObject.GetActive() == true) continue;
            _colliders[i].ActiveSphereCollider(limitTime, center, radius);
            break;
        }
    }

    public void ActiveCapsuleCollider(float limitTime, Vector3 center, int direction, float height, float radius)
    {
        for (int i = 0; i < MAX_NUM; i++)
        {
            if (_colliders[i].gameObject.GetActive() == true) continue;
            _colliders[i].ActiveCapsuleCollider(limitTime, center, direction, height, radius);
            break;
        }
    }
}
