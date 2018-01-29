using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDestroyObject : MonoBehaviour {

    public void SetParentCollider(SkillCollider col)
    {
        col.SetGenericDelegate((Actor defActor, Actor atkActor)=> { PhotonNetwork.Destroy(this.gameObject); });
    }
}
