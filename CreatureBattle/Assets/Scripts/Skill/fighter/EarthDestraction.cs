using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthDestraction : SkillBase {

    GameObject _rangeObj;
    public EarthDestraction()
    {
        CAST_TIME = 1.0f;
        RECAST_TIME = 15.0f;
        ACTION_TIME = 1.0f;
    }

    protected override void EntryCast(Actor actor)
    {
        _rangeObj = EffectManager.Instance.SphereRange(actor.transform.position, 10.0f, new Color(1, 0.5f, 0, 1));
    }

    protected override void Casting(Actor actor)
    {

    }

    protected override void EndCast(Actor actor)
    {
        GameObject.Destroy(_rangeObj.gameObject);
        if (!actor.GetPhotonView().isMine) return;
        SkillCollider col = ColliderManager.Instance.GetCollider();
        col.Initialize(actor, SkillCollider.HitTarget.Monster, 0.6f, 0.6f, (argActor) =>
        {
            argActor.AddCondition(ActorCondition.KIND.EARTH_DESTRACTION, 10.0f, 0.0f);
            argActor.TakeDamage(75.0f);
        });
        col.SetSphereCollider(actor.transform.position, 6.0f);
    }

    protected override void Action(Actor actor)
    {

    }

    protected override void EndAction(Actor actor)
    {

    }

    protected override void Cancel(Actor actor)
    {
        GameObject.Destroy(_rangeObj.gameObject);
    }
}
