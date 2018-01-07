using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseFlame : SkillBase {

    int _stack;
    public ChaseFlame()
    {
        CAST_TIME = 0.0f;
        RECAST_TIME = 5.0f;
        ACTION_TIME = 1.0f;
        _stack = 0;
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
            argActor.AddCondition(ActorCondition.KIND.METEO_IMPACT, 5.0f, 0.0f);
            argActor.TakeDamage(5.0f + (_stack * 1.0f));
        });
        col.SetQubeCollider(actor.transform.position, actor.transform.rotation, new Vector3(1, 1, 1));
        _stack++;
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
