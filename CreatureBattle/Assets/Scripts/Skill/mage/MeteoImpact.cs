﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoImpact : SkillBase {
    GameObject _rangeObj;
    ParticleSystem _effect;
    public MeteoImpact()
    {
        CAST_TIME = 1.2f;
        RECAST_TIME = 12.0f;
        ACTION_TIME = 1.5f;

        GameObject prefab = Resources.Load("Effect/KY_effects/AMFX02/P_AMFX02_fire6") as GameObject;
        _effect = GameObject.Instantiate(prefab).GetComponent<ParticleSystem>();
        _effect.Stop();
    }

    protected override void EntryCast(Actor actor)
    {
        _rangeObj = EffectManager.Instance.SphereRange(actor.transform.position + actor.transform.forward * 16.0f, 12.0f, _myColor);

        _effect.transform.position = actor.transform.position + actor.transform.forward * 16.0f;
        _effect.Play();
    }

    protected override void Casting(Actor actor)
    {

    }

    protected override void EndCast(Actor actor)
    {
        actor.GetAnimator().SetTrigger("Cast02");
        GameObject.Destroy(_rangeObj.gameObject);
        if (!actor.GetPhotonView().isMine) return;
        SkillCollider col = ColliderManager.Instance.GetCollider();
        col.Initialize(actor, SkillCollider.HitTarget.Monster, 0.1f, 99.0f, (defActor, atkActor) =>
        {
            defActor.AddCondition(ActorCondition.KIND.METEO_IMPACT, 5.0f, 0.0f);
            defActor.TakeDamage(90.0f);
        });
        col.SetSphereCollider(actor.transform.position + actor.transform.forward * 16.0f, 12.0f);
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
        GameObject.Destroy(_rangeObj.gameObject);
        _effect.Stop();
    }
}
