using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandGuard : SkillBase {

    GameObject _rangeObj;
    public StandGuard()
    {
        CAST_TIME = 0.5f;
        RECAST_TIME = 30.0f;
        ACTION_TIME = 2.0f;
    }

    protected override void EntryCast(Actor actor)
    {
        _rangeObj = EffectManager.Instance.FanRange(actor.transform.position + actor.transform.forward, actor.transform.eulerAngles.y + 180, 20, 45, new Color(0, 0.2f, 1.0f, 1));
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
            argActor.AddCondition(ActorCondition.KIND.STAND_GUARD, 10.0f, 0.0f);
        });
        col.SetFanCollider(actor.transform.position + actor.transform.forward, 20.0f, actor.transform.forward * -1, 45.0f);
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
