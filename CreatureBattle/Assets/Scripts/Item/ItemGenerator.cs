using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour {

    [SerializeField]
    GameObject[] _portionPrefab;

    List<GameObject> _portionInstance = new List<GameObject>();
    [SerializeField]
    float _generateInterval;
    [SerializeField]
    float _timer;

    PhotonView _myPhotonView;

	// Use this for initialization
	void Start () {
        _myPhotonView = GetComponent<PhotonView>();
        _timer = _generateInterval;
    }
	
	// Update is called once per frame
	void Update () {
        if (!PhotonNetwork.isMasterClient) return;

        for (int i = 0; i < _portionInstance.Count; i++)
        {
            if (_portionInstance[i] == null)
            {
                _portionInstance.RemoveAt(i);
            }
        }

        if (3 < _portionInstance.Count) return;
        _timer -= Time.deltaTime;
        if (_timer < 0.0f)
        {
            _timer = _generateInterval;
            Vector3 pos = new Vector3(Random.Range(-9.0f, 20.0f), -1.47f, Random.Range(-12.4f, -2.5f));
            if (Random.Range(0, 2) == 0)
            {
                _portionInstance.Add(PhotonNetwork.InstantiateSceneObject("Items/LifePot_tex", pos, Quaternion.identity, 0, null));
            }
            else
            {
                _portionInstance.Add(PhotonNetwork.InstantiateSceneObject("Items/ManaPot_tex", pos, Quaternion.identity, 0, null));
            }
        }
    }
}
