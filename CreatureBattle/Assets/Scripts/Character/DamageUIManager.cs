using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUIManager : MonoBehaviour {
    [SerializeField]
    GameObject _damageUIPrefab;
    DamageUI[] _objectPool;
    int MAX_COUNT = 50;
    public static DamageUIManager Instance
    {
        get;
        private set;
    }


	void Start () {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        _objectPool = new DamageUI[MAX_COUNT];
        for (int i = 0; i < MAX_COUNT; i++)
        {
            _objectPool[i] = Instantiate(_damageUIPrefab).GetComponent<DamageUI>();
            _objectPool[i].gameObject.SetActive(false);
            _objectPool[i].transform.parent = transform;
        }
    }
	
	void Update () {
        for (int i = 0; i < MAX_COUNT; i++)
        {
            if (!_objectPool[i].gameObject.activeSelf) continue;
            _objectPool[i].MyUpdate();
        }
	}

    public void CreateUI(Vector3 pos, Color color)
    {
        for (int i = 0; i < MAX_COUNT; i++)
        {
            if (_objectPool[i].gameObject.activeSelf) continue;
            _objectPool[i].Initialize(pos, color);
            return;
        }
    }
}
