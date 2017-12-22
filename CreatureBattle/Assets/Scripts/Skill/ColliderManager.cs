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
        }
	}
	
    public VariableCollider EntrySphereCollider(int layerName, Actor owner, float limitTime, int damage, int damageRange, Vector3 center, float radius, StatusAilment.StatusAilmentBase[] statusAilments)
    {
        for (int i = 0; i < MAX_NUM; i++)
        {
            if (_colliders[i].gameObject.GetActive() == true) continue;
            return _colliders[i].EntrySphereCollider(layerName, owner, limitTime, damage, damageRange, center, radius, statusAilments);
        }

        return null;
    }

    public VariableCollider EntryCapsuleCollider(int layerName, Actor owner, float limitTime, int damage, int damageRange, Vector3 center, int direction, float height, float radius, StatusAilment.StatusAilmentBase[] statusAilments)
    {
        for (int i = 0; i < MAX_NUM; i++)
        {
            if (_colliders[i].gameObject.GetActive() == true) continue;
            return _colliders[i].EntryCapsuleCollider(layerName, owner, limitTime, damage, damageRange, center, direction, height, radius, statusAilments);
        }

        return null;
    }

    public VariableCollider EntryFanCollider(int layerName, Actor owner, float limitTime, int damage, int damageRange, Vector3 center, float radius, Vector3 currentAngle, float angleRange, StatusAilment.StatusAilmentBase[] statusAilments)
    {
        for (int i = 0; i < MAX_NUM; i++)
        {
            if (_colliders[i].gameObject.GetActive() == true) continue;
            return _colliders[i].EntryFanCollider(layerName, owner, limitTime, damage, damageRange, center, radius, currentAngle, angleRange, statusAilments);
        }

        return null;
    }
}
