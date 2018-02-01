﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using antilunchbox;

public class ChaseFlame : SkillBase {
    GameObject _rangeObj;
    ParticleSystem[] _effect = new ParticleSystem[4];
    int _stack;
    public ChaseFlame()
    {
        CAST_TIME = 1.2f;
        RECAST_TIME = 5.0f;
        ACTION_TIME = 1.0f;
        _stack = 0;
        GameObject prefab = Resources.Load("Effect/KY_effects/AMFX02/P_AMFX02_fire2") as GameObject;
        for (int i = 0; i < 4; i++)
        {
            _effect[i] = GameObject.Instantiate(prefab).GetComponent<ParticleSystem>();
            _effect[i].Stop();
        }
    }

    protected override void EntryCast(Actor actor)
    {
        _rangeObj = EffectManager.Instance.QuadRange(actor.transform.position + actor.transform.forward * 7.5f, actor.transform.eulerAngles.y, new Vector3(5, 18, 1), _myColor);
		SoundManager.PlaySFX("se_002");
    }

    protected override void Casting(Actor actor)
    {

    }

    protected override void EndCast(Actor actor)
    {
        actor.GetAnimator().SetTrigger("Cast02");
        for (int i = 0; i < 4; i++)
        {
            _effect[i].transform.position = actor.transform.position + actor.transform.forward * (i + 1) * 3.0f;
            _effect[i].Play();
        }
        if (!actor.GetPhotonView().isMine) return;
        SkillCollider col = ColliderManager.Instance.GetCollider();
        col.Initialize(actor,SkillCollider.HitTarget.Monster, 0.5f, 2.0f, (defActor, atkActor) =>
        {
            defActor.TakeDamage(5.0f + (_stack * 6.0f));
        });
        col.SetQubeCollider(actor.transform.position + actor.transform.forward * 7.5f, actor.transform.rotation, new Vector3(5, 1, 18));
        _stack++;
		SoundManager.PlaySFX("se_017");
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
