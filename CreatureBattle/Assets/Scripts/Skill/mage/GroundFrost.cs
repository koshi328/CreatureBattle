using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFrost : SkillBase {

    GameObject _rangeObj;
    ParticleSystem _effect;
    public GroundFrost()
    {
        CAST_TIME = 2.0f;
        RECAST_TIME = 10.0f;
        ACTION_TIME = 3.0f;
        _effect = EffectManager.Instance.GetEffectInstance(EffectManager.KIND.GroundFrost).GetComponent<ParticleSystem>();
        _effect.Stop();
    }

    protected override void EntryCast(Actor actor)
    {
        _rangeObj = EffectManager.Instance.FanRange(actor.transform.position, actor.transform.eulerAngles.y, 15, 30, new Color(1, 0.5f, 0, 1));
    }

    protected override void Casting(Actor actor)
    {

    }

    protected override void EndCast(Actor actor)
    {
        GameObject.Destroy(_rangeObj);
        if (!actor.GetPhotonView().isMine) return;
        SkillCollider col = ColliderManager.Instance.GetCollider();
        col.Initialize(actor, SkillCollider.HitTarget.Monster, 3.0f, 3.0f, (argActor) =>
        {
            argActor.AddCondition(ActorCondition.KIND.GROUND_FROST, 2.0f, 0.0f);
            argActor.TakeDamage(60.0f);
        });
        col.SetFanCollider(actor.transform.position, 15.0f, actor.transform.forward, 30.0f);
        _effect.transform.position = actor.transform.position;
        _effect.transform.rotation = actor.transform.rotation;
        _effect.Play();
    }

    protected override void Action(Actor actor)
    {

    }

    protected override void EndAction(Actor actor)
    {
        _effect.Stop();
    }

    protected override void Cancel(Actor actor)
    {
        GameObject.Destroy(_rangeObj);
    }
}
