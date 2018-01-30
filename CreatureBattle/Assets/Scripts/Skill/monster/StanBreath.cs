using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanBreath : SkillBase {

    GameObject _rangeObj;
    ParticleSystem _effect;
    public StanBreath()
    {
        CAST_TIME = 1.5f;
        RECAST_TIME = 30.0f;
        ACTION_TIME = 2.0f;
        GameObject prefab = Resources.Load("Effect/ItoEffects/monsterStanBless") as GameObject;
        _effect = GameObject.Instantiate(prefab).GetComponent<ParticleSystem>();
        _effect.Stop();
    }

    protected override void EntryCast(Actor actor)
    {
        _rangeObj = EffectManager.Instance.FanRange(actor.transform.position + actor.transform.forward, actor.transform.eulerAngles.y, 40, 60, _myColor);
    }

    protected override void Casting(Actor actor)
    {

    }

    protected override void EndCast(Actor actor)
    {
        actor.GetAnimator().SetTrigger("Breath");
        _effect.transform.position = actor.transform.position + Vector3.up;
        _effect.transform.rotation = actor.transform.rotation;
        _effect.Play();
        GameObject.Destroy(_rangeObj);
        if (!actor.GetPhotonView().isMine) return;
        SkillCollider col = ColliderManager.Instance.GetCollider();
        col.Initialize(actor, SkillCollider.HitTarget.Player, 2.0f, 0.0f, (defActor, atkActor) =>
        {
            defActor.AddCondition(ActorCondition.KIND.STAN_BLESS, 5.0f, 0.0f);
            defActor.AddCondition(ActorCondition.KIND.STAN, 5.0f, 0.0f);
        });
        col.SetFanCollider(actor.transform.position + actor.transform.forward, 40.0f, actor.transform.forward, 60.0f);
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
        _effect.Stop();
    }
}
