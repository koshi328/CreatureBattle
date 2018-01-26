using System.Collections;
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
        if (!actor.GetPhotonView().isMine) return;
        SkillCollider col = ColliderManager.Instance.GetCollider();
        col.Initialize(actor, SkillCollider.HitTarget.Monster, 1.0f, 1.0f, (argActor) =>
        {
            argActor.AddCondition(ActorCondition.KIND.ANGRY_SHOUT, 4.0f, 25.0f);
        });
        col.SetSphereCollider(actor.transform.position, 10.0f);
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
