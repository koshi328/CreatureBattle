using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyImpact : SkillBase {

    GameObject _rangeObj;
    ParticleSystem _effect;
    bool init;
    public DeadlyImpact()
    {
        CAST_TIME = 0.0f;
        RECAST_TIME = 15.0f;
        ACTION_TIME = 0.6f;
        GameObject prefab = Resources.Load("Effect/KY_effects/AMFX02/P_AMFX02_claw") as GameObject;
        _effect = GameObject.Instantiate(prefab).GetComponent<ParticleSystem>();
        _effect.Stop();
        init = false;
    }

    protected override void EntryCast(Actor actor)
    {
        _rangeObj = EffectManager.Instance.FanRange(actor.transform.position, actor.transform.eulerAngles.y, 6, 45, new Color(1, 0.5f, 0, 1));
    }

    protected override void Casting(Actor actor)
    {

    }

    protected override void EndCast(Actor actor)
    {
        actor.GetAnimator().SetTrigger("Stan");

        if (!actor.GetPhotonView().isMine) return;
        SkillCollider col = ColliderManager.Instance.GetCollider();
        col.Initialize(actor, SkillCollider.HitTarget.Monster, 0.6f, 0.6f, (argActor) =>
        {
            float damage = 50.0f + (50.0f * (argActor.GetCondition(ActorCondition.KIND.DEADLY_IMPACT).GetStack() * 0.01f));
            argActor.TakeDamage(damage);
        });
        col.SetFanCollider(actor.transform.position, 6.0f, actor.transform.forward, 45.0f);
    }

    protected override void Action(Actor actor)
    {

    }

    protected override void EndAction(Actor actor)
    {
        GameObject.Destroy(_rangeObj.gameObject);
        _effect.transform.position = actor.transform.position + actor.transform.forward * 2;
        _effect.transform.rotation = actor.transform.rotation;
        _effect.Play();
    }

    protected override void Cancel(Actor actor)
    {
        GameObject.Destroy(_rangeObj.gameObject);
    }

    protected override void Update(Actor actor)
    {
        if (init) return;
        init = true;
        if (!actor.GetPhotonView().isMine) return;
        actor.AddCondition(ActorCondition.KIND.DEADLY_IMPACT, 3.0f, 0.0f, false);
    }
}
