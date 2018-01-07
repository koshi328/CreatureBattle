using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFrost : SkillBase {

    public GroundFrost()
    {
        CAST_TIME = 2.0f;
        RECAST_TIME = 10.0f;
        ACTION_TIME = 3.0f;
    }

    protected override void EntryCast(Actor actor)
    {

    }

    protected override void Casting(Actor actor)
    {

    }

    protected override void EndCast(Actor actor)
    {
        //if (!actor.GetPhotonView().isMine) return;
        SkillCollider col = ColliderManager.Instance.GetCollider();
        col.Initialize(actor, 3.0f, 3.0f, (argActor) =>
        {
            argActor.AddCondition(ActorCondition.KIND.GROUND_FROST, 2.0f, 0.0f);
            argActor.TakeDamage(60.0f);
        });
        col.SetFanCollider(actor.transform.position, 8.0f, actor.transform.forward, 45.0f);
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
