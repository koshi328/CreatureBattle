using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonStorm : SkillBase {
    GameObject _rangeObj;
    ParticleSystem _effect;
    public DragonStorm()
    {
        CAST_TIME = 0.5f;
        RECAST_TIME = 7.0f;
        ACTION_TIME = 1.0f;
        GameObject prefab = Resources.Load("Effect/KY_effects/AMFX02/DragonStorm") as GameObject;
        _effect = GameObject.Instantiate(prefab).GetComponent<ParticleSystem>();
        _effect.Stop();
    }

    protected override void EntryCast(Actor actor)
    {
        _rangeObj = EffectManager.Instance.SphereRange(actor.transform.position, 30.0f, new Color(1, 0.5f, 0, 1));
    }

    protected override void Casting(Actor actor)
    {

    }

    protected override void EndCast(Actor actor)
    {
        _effect.transform.position = actor.transform.position;
        _effect.Play();
        if (!actor.GetPhotonView().isMine) return;
        SkillCollider col = ColliderManager.Instance.GetCollider();
        col.Initialize(actor, SkillCollider.HitTarget.Player, 0.5f, 0.5f, (argActor) =>
        {
            argActor.AddCondition(ActorCondition.KIND.SILENCE, 5.0f, 0.0f);
            argActor.TakeDamage(141.0f);
        });
        col.SetSphereCollider(actor.transform.position, 30.0f);
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
        _effect.Stop();
    }
}
