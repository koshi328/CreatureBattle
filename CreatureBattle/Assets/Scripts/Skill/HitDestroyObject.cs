using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDestroyObject : MonoBehaviour {

    public void SetParentCollider(SkillCollider col)
    {
        col.SetGenericDelegate((Actor argActor)=> { PhotonNetwork.Destroy(this.gameObject); });
    }
}
