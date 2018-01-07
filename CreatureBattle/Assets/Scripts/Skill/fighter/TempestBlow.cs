using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempestBlow : SkillBase {

    public TempestBlow()
    {
        CAST_TIME = 0.0f;
        RECAST_TIME = 10.0f;
        ACTION_TIME = 0.6f;
    }
    
    protected override void EntryCast(Actor actor)
    {
        Debug.Log("TempestBlow");
    }

    protected override void Casting(Actor actor)
    {
        Debug.Log("Casting");
    }

    protected override void EndCast(Actor actor)
    {
        Debug.Log("EndCast");
        if (!actor.GetPhotonView().isMine) return;
        SkillCollider col = ColliderManager.Instance.GetCollider();
        col.Initialize(actor, 0.6f, 0.1f, (argActor) =>
        {
            argActor.AddCondition(ActorCondition.KIND.TEMPEST_BLOW, 10.0f, 0.0f);
            argActor.TakeDamage(8.0f);
        });
        col.SetFanCollider(actor.transform.position, 3.0f, actor.transform.forward, 45.0f);

        actor.GetAnimator().SetTrigger("React");
    }

    protected override void Action(Actor actor)
    {
        Debug.Log("Action");
    }

    protected override void EndAction(Actor actor)
    {
        Debug.Log("EndAction");
    }

    protected override void Cancel(Actor actor)
    {
        Debug.Log("Cancel");
    }
}
