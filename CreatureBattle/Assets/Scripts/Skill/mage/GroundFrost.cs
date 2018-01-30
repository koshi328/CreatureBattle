using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFrost : SkillBase {

    GameObject _rangeObj;
    ParticleSystem _effect;
    public GroundFrost()
    {
        CAST_TIME = 0.5f;
        RECAST_TIME = 10.0f;
        ACTION_TIME = 1.0f;
        GameObject prefab = Resources.Load("Effect/ItoEffects/frost") as GameObject;
        _effect = GameObject.Instantiate(prefab).GetComponent<ParticleSystem>();
        _effect.Stop();
    }

    protected override void EntryCast(Actor actor)
    {
        _rangeObj = EffectManager.Instance.FanRange(actor.transform.position, actor.transform.eulerAngles.y, 40, 30, _myColor);
    }

    protected override void Casting(Actor actor)
    {

    }

    protected override void EndCast(Actor actor)
    {
        actor.GetAnimator().SetTrigger("Cast02");
        GameObject.Destroy(_rangeObj);
        _effect.transform.position = actor.transform.position + Vector3.up;
        _effect.transform.rotation = actor.transform.rotation;
        _effect.Play();
        if (!actor.GetPhotonView().isMine) return;
        SkillCollider col = ColliderManager.Instance.GetCollider();
        col.Initialize(actor, SkillCollider.HitTarget.Monster, 1.0f, 1.0f, (defActor, atkActor) =>
        {
            defActor.AddCondition(ActorCondition.KIND.GROUND_FROST, 2.0f, 0.0f);
            defActor.TakeDamage(60.0f);
        });
        col.SetFanCollider(actor.transform.position, 40.0f, actor.transform.forward, 30.0f);
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
