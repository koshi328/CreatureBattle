using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour {

    public enum KIND
    {
        GroundFrost,
        MeteoImpact,
        ThnderTrap,
        ChaseFlame,
        ThnderTrapHit,
        MAX_NUM
    }

    PhotonView _myPhotonView;
    [SerializeField]
    GameObject[] _prefabs = new GameObject[(int)KIND.MAX_NUM];
    [SerializeField]
    GameObject[] _rangeObj = new GameObject[3];
    public static EffectManager Instance
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
        _myPhotonView = GetComponent<PhotonView>();
	}

    [PunRPC]
    void CreateEffectRPC(Vector3 pos, float rotY, float size)
    {

    }

    public void CreateEffect(Vector3 pos, float rotY, float size)
    {
        _myPhotonView.RPC("CreateEffectRPC", PhotonTargets.AllViaServer, pos, rotY, size);
    }

    public GameObject SphereRange(Vector3 pos, float range, Color color)
    {
        GameObject obj = Instantiate(_rangeObj[0], pos, Quaternion.identity);
        RangeView script = obj.GetComponent<RangeView>();
        obj.transform.rotation = Quaternion.Euler(90, 0, 0);
        script.SetColor(color);
        script.SetSize(new Vector3(range, range, 1));
        return obj.gameObject;
    }
    public GameObject FanRange(Vector3 pos, float rotY, float range, float angle, Color color)
    {
        GameObject obj = Instantiate(_rangeObj[1], pos,Quaternion.identity);
        RangeView script = obj.GetComponent<RangeView>();
        obj.transform.rotation = Quaternion.Euler(90, rotY, 0);
        script.SetColor(color);
        script.SetFan_Range(angle);
        script.SetSize(new Vector3(range, range, 1));
        return obj.gameObject;
    }
    public GameObject QuadRange(Vector3 pos, float rotY, Vector3 size, Color color)
    {
        GameObject obj = Instantiate(_rangeObj[2], pos, Quaternion.identity);
        RangeView script = obj.GetComponent<RangeView>();
        obj.transform.rotation = Quaternion.Euler(90, rotY, 0);
        script.SetColor(color);
        script.SetSize(size);
        return obj.gameObject;
    }

    public GameObject GetEffectInstance(KIND kind)
    {
        return Instantiate(_prefabs[(int)kind]);
    }
}
