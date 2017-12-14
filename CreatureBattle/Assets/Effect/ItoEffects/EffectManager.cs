using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour {

    public static EffectManager Instance
    {
        get;
        private set;
    }

    [SerializeField]
    GameObject hitEffect;
    [SerializeField]
    GameObject slashEffect;
    [SerializeField]
    GameObject iceTornadoEffect;
    [SerializeField]
    GameObject iceEnergyEffect;
    [SerializeField]
    GameObject lightningEffect;

    private void Start()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    // hit
    public GameObject HitEffect(Vector3 pos)
    {
        GameObject obj = Instantiate(hitEffect, pos, Quaternion.identity);
        Destroy(obj, 0.5f);
        return obj;
    }
    // 斬撃
    public GameObject SlashEffect(Vector3 pos, Quaternion rot)
    {
        GameObject obj = Instantiate(slashEffect, pos, rot);
        Destroy(obj, 0.5f);
        return obj;
    }
    // 氷の竜巻
    public GameObject IceTornadoEffect(Vector3 pos, float time)
    {
        GameObject obj = Instantiate(iceTornadoEffect, pos, Quaternion.identity);
        Destroy(obj, time);
        return obj;
    }
    // 冷気
    public GameObject IceEnergyEffect(Vector3 pos, Quaternion rot, float time)
    {
        GameObject obj = Instantiate(iceEnergyEffect, pos, rot);
        obj.GetComponent<ParticleSystem>().time = time;
        Destroy(obj, time);
        return obj;
    }
    // 雷
    public GameObject LightningEffect(Vector3 pos)
    {
        GameObject obj = Instantiate(iceTornadoEffect, pos, Quaternion.identity);
        Destroy(obj, 0.5f);
        return obj;
    }
}
