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

    public GameObject HitEffect(Vector3 pos)
    {
        GameObject obj = Instantiate(hitEffect, pos, Quaternion.identity);
        Destroy(obj, 0.5f);
        return obj;
    }
    public GameObject SlashEffect(Vector3 pos, Vector3 rot)
    {
        Quaternion r = Quaternion.Euler(rot);
        GameObject obj = Instantiate(slashEffect, pos, r);
        Destroy(obj, 0.5f);
        return obj;
    }
    public GameObject SlashEffect(Vector3 pos, Quaternion rot)
    {
        GameObject obj = Instantiate(slashEffect, pos, rot);
        Destroy(obj, 0.5f);
        return obj;
    }
}
