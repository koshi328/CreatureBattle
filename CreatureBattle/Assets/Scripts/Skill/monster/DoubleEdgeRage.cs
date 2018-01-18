using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleEdgeRage : SkillBase {
    GameObject _rangeObj;
    public DoubleEdgeRage()
    {
        CAST_TIME = 0.0f;
        RECAST_TIME = 10.0f;
        ACTION_TIME = 1.0f;
    }

    protected override void EntryCast(Actor actor)
    {
        _rangeObj = EffectManager.Instance.QuadRange(actor.transform.position + actor.transform.forward * 7.5f, actor.transform.eulerAngles.y, new Vector3(4, 15, 1), new Color(1, 0.5f, 0, 1));
    }

    protected override void Casting(Actor actor)
    {

    }

    protected override void EndCast(Actor actor)
    {
        if (!actor.GetPhotonView().isMine) return;
        actor.AddCondition(ActorCondition.KIND.DOUBLE_EDGE, 1.0f, 0.0f);
        SkillCollider col = ColliderManager.Instance.GetCollider();
        col.Initialize(actor, SkillCollider.HitTarget.Player, 2.0f, 2.0f, (argActor) =>
        {
            float damage = argActor.GetCondition(ActorCondition.KIND.DOUBLE_EDGE).GetStack() * 10;
            argActor.TakeDamage(60 + damage);
            argActor.TakeDamage(60 + damage);
        });
        col.SetQubeCollider(actor.transform.position + actor.transform.forward * 7.5f, actor.transform.rotation, new Vector3(4, 1, 15));
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
