﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using antilunchbox;

public class EarthDestraction : SkillBase {

    GameObject _rangeObj;
    ParticleSystem _effect;
    public EarthDestraction()
    {
        CAST_TIME = 1.7f;
        RECAST_TIME = 15.0f;
        ACTION_TIME = 1.0f;
        
        GameObject prefab = Resources.Load("Effect/KY_effects/AMFX02/P_AMFX02_thunder6") as GameObject;
        _effect = GameObject.Instantiate(prefab).GetComponent<ParticleSystem>();
        _effect.Stop();
    }

    protected override void EntryCast(Actor actor)
    {
        _rangeObj = EffectManager.Instance.SphereRange(actor.transform.position, 20.0f, _myColor);
		SoundManager.PlaySFX("se_002");
    }

    protected override void Casting(Actor actor)
    {

    }

    protected override void EndCast(Actor actor)
    {

        actor.GetAnimator().SetTrigger("Power");
        _effect.transform.position = actor.transform.position;
        _effect.Play();
        GameObject.Destroy(_rangeObj.gameObject);
        if (!actor.GetPhotonView().isMine) return;
        SkillCollider col = ColliderManager.Instance.GetCollider();
        col.Initialize(actor, SkillCollider.HitTarget.Monster, 0.1f, 0.1f, (defActor, atkActor) =>
        {
            defActor.AddCondition(ActorCondition.KIND.EARTH_DESTRACTION, 3.0f, 0.0f);
            defActor.TakeDamage(150.0f, atkActor);
        });
        col.SetSphereCollider(actor.transform.position, 20.0f);
		SoundManager.PlaySFX("se_000");
    }

    protected override void Action(Actor actor)
    {

    }

    protected override void EndAction(Actor actor)
    {

    }

    protected override void Cancel(Actor actor)
    {
        GameObject.Destroy(_rangeObj.gameObject);
        _effect.Stop();
    }
}
