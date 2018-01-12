using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveShout : SkillBase
{
    
    public AggressiveShout()
    {
        CAST_TIME = 0.0f;
        RECAST_TIME = 12.0f;
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
        SkillCollider col = ColliderManager.Instance.GetCollider();
        col.Initialize(actor, SkillCollider.HitTarget.Player, 2.0f, 2.0f, (argActor) =>
        {
            argActor.AddCondition(ActorCondition.KIND.AGGRESSIVE_SHOUT, 12.0f, 25.0f);
        });
        col.SetSphereCollider(actor.transform.position, 20.0f);
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
