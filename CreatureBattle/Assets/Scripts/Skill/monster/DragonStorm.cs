using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonStorm : SkillBase {
    GameObject _rangeObj;
    ParticleSystem _effect;
    public DragonStorm()
    {
        CAST_TIME = 2.0f;
        RECAST_TIME = 7.0f;
        ACTION_TIME = 1.0f;
        GameObject prefab = Resources.Load("Effect/KY_effects/AMFX02/DragonStorm") as GameObject;
        _effect = GameObject.Instantiate(prefab).GetComponent<ParticleSystem>();
        _effect.Stop();
    }

    protected override void EntryCast(Actor actor)
    {
        _rangeObj = EffectManager.Instance.SphereRange(actor.transform.position, 30.0f, _myColor);
    }

    protected override void Casting(Actor actor)
    {

    }

    protected override void EndCast(Actor actor)
    {
        actor.GetAnimator().SetTrigger("Scream");
        _effect.transform.position = actor.transform.position;
        _effect.Play();
    }

    protected override void Action(Actor actor)
    {

    }

    protected override void EndAction(Actor actor)
    {
        GameObject.Destroy(_rangeObj);
        if (!actor.GetPhotonView().isMine) return;
        SkillCollider col = ColliderManager.Instance.GetCollider();
        col.Initialize(actor, SkillCollider.HitTarget.Player, 0.1f, 99.9f, (defActor, atkActor) =>
        {
            defActor.AddCondition(ActorCondition.KIND.SILENCE, 5.0f, 0.0f);
            defActor.TakeDamage(141.0f);
        });
        col.SetSphereCollider(actor.transform.position, 30.0f);
    }

    protected override void Cancel(Actor actor)
    {
        GameObject.Destroy(_rangeObj);
        _effect.Stop();
    }
}
