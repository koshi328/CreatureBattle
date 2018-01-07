using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyImDeadlyImpact : SkillBase {

    public DeadlyImDeadlyImpact()
    {
        CAST_TIME = 2.0f;
        RECAST_TIME = 15.0f;
        ACTION_TIME = 1.0f;
    }

    protected override void EntryCast(Actor actor)
    {
        Debug.Log("DeadlyImDeadlyImpact");
    }

    protected override void Casting(Actor actor)
    {

    }

    protected override void EndCast(Actor actor)
    {
        //if (!actor.GetPhotonView().isMine) return;
        SkillCollider col = ColliderManager.Instance.GetCollider();
        col.Initialize(actor, 0.6f, 0.6f, (argActor) =>
        {
            argActor.TakeDamage(50.0f);
        });
        col.SetFanCollider(actor.transform.position, 3.0f, actor.transform.forward, 45.0f);
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
