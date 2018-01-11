﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoImpact : SkillBase {

    public MeteoImpact()
    {
        CAST_TIME = 1.0f;
        RECAST_TIME = 12.0f;
        ACTION_TIME = 2.0f;
    }

    protected override void EntryCast(Actor actor)
    {

    }

    protected override void Casting(Actor actor)
    {

    }

    protected override void EndCast(Actor actor)
    {
        //if (!actor.GetPhotonView().isMine) return;
        SkillCollider col = ColliderManager.Instance.GetCollider();
        col.Initialize(actor, SkillCollider.HitTarget.Monster, 2.0f, 2.0f, (argActor) =>
        {
            argActor.AddCondition(ActorCondition.KIND.METEO_IMPACT, 5.0f, 0.0f);
            argActor.TakeDamage(54.0f);
        });
        col.SetFanCollider(actor.transform.position, 8.0f, actor.transform.forward, 45.0f);
    }

    protected override void Action(Actor actor)
    {

    }

    protected override void EndAction(Actor actor)
    {

    }

    protected override void Cancel(Actor actor)
    {

    }
}
