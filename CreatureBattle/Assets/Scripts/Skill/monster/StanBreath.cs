using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanBreath : SkillBase {

    GameObject _rangeObj;
    public StanBreath()
    {
        CAST_TIME = 2.5f;
        RECAST_TIME = 30.0f;
        ACTION_TIME = 2.0f;
    }

    protected override void EntryCast(Actor actor)
    {
        _rangeObj = EffectManager.Instance.FanRange(actor.transform.position + actor.transform.forward, actor.transform.eulerAngles.y, 30, 60, new Color(1, 0.5f, 0, 1));
    }

    protected override void Casting(Actor actor)
    {

    }

    protected override void EndCast(Actor actor)
    {
        if (!actor.GetPhotonView().isMine) return;
        SkillCollider col = ColliderManager.Instance.GetCollider();
        col.Initialize(actor, SkillCollider.HitTarget.Player, 2.0f, 0.0f, (argActor) =>
        {
            argActor.AddCondition(ActorCondition.KIND.STAN_BLESS, 8.0f, 0.0f);
            argActor.AddCondition(ActorCondition.KIND.STAN, 8.0f, 0.0f);
        });
        col.SetFanCollider(actor.transform.position + actor.transform.forward, 30.0f, actor.transform.forward, 60.0f);
    }

    protected override void Action(Actor actor)
    {

    }

    protected override void EndAction(Actor actor)
    {
        GameObject.Destroy(_rangeObj);
    }

    protected override void Cancel(Actor actor)
    {
        GameObject.Destroy(_rangeObj);
    }
}
