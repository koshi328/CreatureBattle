using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandGuard : SkillBase {


    public StandGuard()
    {
        CAST_TIME = 0.5f;
        RECAST_TIME = 30.0f;
        ACTION_TIME = 2.0f;
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
        col.Initialize(actor, 2.0f, 2.0f, (argActor) =>
        {
            argActor.AddCondition(ActorCondition.KIND.STAND_GUARD, 10.0f, 0.0f);
        });
        col.SetFanCollider(actor.transform.position, 8.0f, actor.transform.forward * -1, 45.0f);
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
