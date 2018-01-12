using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageImpact : SkillBase {
    static GameObject _effectPrefab;
    public RageImpact()
    {
        CAST_TIME = 1.0f;
        RECAST_TIME = 7.0f;
        ACTION_TIME = 1.0f;
        if (_effectPrefab == null)
            _effectPrefab = Resources.Load("Prefabs/Effect/RageImpactEffect") as GameObject;
    }

    protected override void EntryCast(Actor actor)
    {

    }

    protected override void Casting(Actor actor)
    {

    }

    protected override void EndCast(Actor actor)
    {
        GameObject effect = GameObject.Instantiate(_effectPrefab, actor.transform.position + Vector3.up * 2, actor.transform.rotation);
        FlyingObject flyingObj = effect.GetComponent<FlyingObject>();
        flyingObj.SetDirection(actor.transform.forward, 40.0f);
        if (!actor.GetPhotonView().isMine)
        {
            flyingObj.SetChildCollider(null, 10.0f);
            return;
        }
        SkillCollider col = ColliderManager.Instance.GetCollider();
        col.Initialize(actor, SkillCollider.HitTarget.Monster, 10.0f, 10.0f, (argActor) =>
        {
            argActor.TakeDamage(35.0f);
        });
        flyingObj.SetChildCollider(col, 10.0f);
        col.SetQubeCollider(Vector3.zero, Quaternion.identity, new Vector3(2, 1, 1));
    }

    protected override void Action(Actor actor)
    {

    }

    protected override void EndAction(Actor actor)
    {

    }

    protected override void Cancel(Actor actor)
    {

    }
}
