using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using antilunchbox;

public class StandGuard : SkillBase {

    GameObject _rangeObj;
    public StandGuard()
    {
        CAST_TIME = 1.0f;
        RECAST_TIME = 30.0f;
        ACTION_TIME = 2.0f;
    }

    protected override void EntryCast(Actor actor)
    {
        _rangeObj = EffectManager.Instance.FanRange(actor.transform.position, actor.transform.eulerAngles.y + 180, 28, 45, _myColor);
    }

    protected override void Casting(Actor actor)
    {

    }

    protected override void EndCast(Actor actor)
    {
        actor.GetAnimator().SetTrigger("Cry");
        if (!actor.GetPhotonView().isMine) return;
        actor.AddCondition(ActorCondition.KIND.STAND_GUARD, 10.0f, 0.0f);
        SkillCollider col = ColliderManager.Instance.GetCollider();
        col.Initialize(actor, SkillCollider.HitTarget.Player, 2.0f, 0.1f, (defActor, atkActor) =>
        {
            defActor.AddCondition(ActorCondition.KIND.STAND_GUARD, 10.0f, 0.0f);
        });
        col.SetFanCollider(actor.transform.position, 28.0f, actor.transform.forward * -1, 45.0f);
		SoundManager.PlaySFX("se_007");
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
