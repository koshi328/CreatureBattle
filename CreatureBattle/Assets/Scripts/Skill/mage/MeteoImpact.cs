using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoImpact : SkillBase {
    static GameObject _effectPrefab;
    public MeteoImpact()
    {
        CAST_TIME = 1.0f;
        RECAST_TIME = 12.0f;
        ACTION_TIME = 2.0f;
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
        if (!actor.GetPhotonView().isMine) return;
        SkillCollider col = ColliderManager.Instance.GetCollider();
        col.Initialize(actor, SkillCollider.HitTarget.Monster, 2.0f, 2.0f, (argActor) =>
        {
            argActor.AddCondition(ActorCondition.KIND.METEO_IMPACT, 5.0f, 0.0f);
            argActor.TakeDamage(54.0f);
        });
        col.SetFanCollider(actor.transform.position, 8.0f, actor.transform.forward, 45.0f);
        flyingObj.SetChildCollider(col, 10.0f);
        col.SetSphereCollider(Vector3.zero, 1.0f);
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
