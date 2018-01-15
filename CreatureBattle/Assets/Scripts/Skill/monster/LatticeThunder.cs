using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LatticeThunder : SkillBase {

    GameObject _rangeObj;
    public LatticeThunder()
    {
        CAST_TIME = 0.5f;
        RECAST_TIME = 18.0f;
        ACTION_TIME = 2.0f;
    }

    protected override void EntryCast(Actor actor)
    {
        _rangeObj = EffectManager.Instance.QuadRange(actor.transform.position, 0.0f, new Vector3(50, 50, 50), new Color(0, 0.2f, 1.0f, 1));
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
            argActor.TakeDamage(374.0f);
            argActor.AddCondition(ActorCondition.KIND.SILENCE, 3.0f, 0.0f);
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
