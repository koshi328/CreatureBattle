using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour {

    public enum KIND
    {
        Burn,

        MAX_NUM
    }


    PhotonView _myPhotonView;
    [SerializeField]
    GameObject[] _prefabs = new GameObject[(int)KIND.MAX_NUM];
	void Start () {
        _myPhotonView = GetComponent<PhotonView>();
	}

    [PunRPC]
    void CreateEffectRPC(Vector3 pos, float rotY, float size)
    {

    }
    [PunRPC]
    void CreateEffectRPC(Transform parent)
    {

    }

    public void CreateEffect(Vector3 pos, float rotY, float size)
    {
        _myPhotonView.RPC("CreateEffectRPC", PhotonTargets.AllViaServer, pos, rotY, size);
    }
    public void CreateEffect(Transform parent)
    {
        _myPhotonView.RPC("CreateEffectRPC", PhotonTargets.AllViaServer, parent);
    }
}
