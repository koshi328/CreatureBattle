using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleEdgeRage : SkillBase {

    public DoubleEdgeRage()
    {
        CAST_TIME = 0.0f;
        RECAST_TIME = 10.0f;
        ACTION_TIME = 1.0f;
    }

    protected override void EntryCast(Actor actor)
    {

    }

    protected override void Casting(Actor actor)
    {

    }

    protected override void EndCast(Actor actor)
    {
        if (!actor.GetPhotonView().isMine) return;
        actor.AddCondition(ActorCondition.KIND.DOUBLE_EDGE, 1.0f, 0.0f);
        SkillCollider col = ColliderManager.Instance.GetCollider();
        col.Initialize(actor, SkillCollider.HitTarget.Player, 2.0f, 0.0f, (argActor) =>
        {
            float damage = argActor.GetCondition(ActorCondition.KIND.DOUBLE_EDGE).GetStack() * 10;
            argActor.TakeDamage(60 * 2 + damage);
        });
        col.SetFanCollider(actor.transform.position + actor.transform.forward, 20.0f, actor.transform.forward, 45.0f);
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
