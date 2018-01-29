using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleEdgeRage : SkillBase {
    GameObject _rangeObj;
    ParticleSystem _effect;
    public DoubleEdgeRage()
    {
        CAST_TIME = 1.2f;
        RECAST_TIME = 10.0f;
        ACTION_TIME = 0.5f;
        GameObject prefab = Resources.Load("Effect/KY_effects/AMFX02/P_AMFX02_claw") as GameObject;
        _effect = GameObject.Instantiate(prefab).GetComponent<ParticleSystem>();
        _effect.transform.localScale = new Vector3(5.0f, 1.0f, 1.0f);
        _effect.Stop();
    }

    protected override void EntryCast(Actor actor)
    {
        _rangeObj = EffectManager.Instance.QuadRange(actor.transform.position + actor.transform.forward * 12.5f, actor.transform.eulerAngles.y, new Vector3(8, 25, 1), new Color(1, 0.5f, 0, 1));
        actor.GetAnimator().SetTrigger("SwapR");

    }

    protected override void Casting(Actor actor)
    {

    }

    protected override void EndCast(Actor actor)
    {
        Vector3 pos = actor.transform.position + actor.transform.forward * 15.0f;
        _effect.transform.position = pos;
        _effect.transform.eulerAngles = new Vector3(0, actor.transform.eulerAngles.y + 90, 0);
        _effect.Play();
    }

    protected override void Action(Actor actor)
    {

    }

    protected override void EndAction(Actor actor)
    {

        GameObject.Destroy(_rangeObj);
        if (!actor.GetPhotonView().isMine) return;
        actor.AddCondition(ActorCondition.KIND.DOUBLE_EDGE, 0.0f, 0.0f);
        SkillCollider col = ColliderManager.Instance.GetCollider();
        col.Initialize(actor, SkillCollider.HitTarget.Player, 0.1f, 99.0f, (defActor, atkActor) =>
        {
            float damage = atkActor.GetCondition(ActorCondition.KIND.DOUBLE_EDGE).GetStack() * 10;
            defActor.TakeDamage(60 + damage);
            defActor.TakeDamage(60 + damage);
        });
        col.SetQubeCollider(actor.transform.position + actor.transform.forward * 12.5f, actor.transform.rotation, new Vector3(8, 1, 25));
    }

    protected override void Cancel(Actor actor)
    {
        GameObject.Destroy(_rangeObj);
        _effect.Stop();
    }
}
