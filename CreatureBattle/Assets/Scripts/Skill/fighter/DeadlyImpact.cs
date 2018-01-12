using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyImpact : SkillBase {

    GameObject _rangeObj;
    public DeadlyImpact()
    {
        CAST_TIME = 2.0f;
        RECAST_TIME = 15.0f;
        ACTION_TIME = 1.0f;
    }

    protected override void EntryCast(Actor actor)
    {
        _rangeObj = EffectManager.Instance.FanRange(actor.transform.position, actor.transform.eulerAngles.y, 6, 45, new Color(1, 0.5f, 0, 1));
        actor.AddCondition(ActorCondition.KIND.DEADLY_IMPACT, 0.0f, 0.0f, false);
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
            float damage = 50.0f + (50.0f * (argActor.GetCondition(ActorCondition.KIND.DEADLY_IMPACT).GetStack() / 100.0f));
            argActor.TakeDamage(damage);
        });
        col.SetFanCollider(actor.transform.position, 6.0f, actor.transform.forward, 45.0f);
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
