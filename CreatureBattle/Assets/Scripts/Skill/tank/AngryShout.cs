﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryShout : SkillBase
{

    ParticleSystem _effect;
    
    public AngryShout()
    {
        CAST_TIME = 0.0f;
        RECAST_TIME = 10.0f;
        ACTION_TIME = 1.0f;
        GameObject prefab = Resources.Load("Effect/KY_effects/AMFX02/P_AMFX02_erekiDoom") as GameObject;
        _effect = GameObject.Instantiate(prefab).GetComponent<ParticleSystem>();
        _effect.Stop();
    }

    protected override void EntryCast(Actor actor)
    {

    }

    protected override void Casting(Actor actor)
    {

    }

    protected override void EndCast(Actor actor)
    {
        actor.GetAnimator().SetTrigger("Cry");
        _effect.transform.position = actor.transform.position;
        _effect.Play();
        actor.AddCondition(ActorCondition.KIND.ANGRY_SHOUT, 20.0f, 35.0f);
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
        _effect.Stop();
    }
}
