using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageImpact : SkillBase {

    public RageImpact()
    {
        CAST_TIME = 1.0f;
        RECAST_TIME = 7.0f;
        ACTION_TIME = 1.0f;
    }

    protected override void EntryCast(Actor actor)
    {
        Debug.Log("RageImpact");
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
            argActor.TakeDamage(35.0f);
        });
        col.SetQubeCollider(actor.transform.position, actor.transform.rotation, new Vector3(2, 1, 1));
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
