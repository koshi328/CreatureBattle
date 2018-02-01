using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using antilunchbox;

public class DragonStorm : SkillBase {
    GameObject _rangeObj;
    ParticleSystem _effect;
    public DragonStorm()
    {
        CAST_TIME = 1.7f;
        RECAST_TIME = 7.0f;
        ACTION_TIME = 1.0f;
        GameObject prefab = Resources.Load("Effect/KY_effects/AMFX02/DragonStorm") as GameObject;
        _effect = GameObject.Instantiate(prefab).GetComponent<ParticleSystem>();
        _effect.Stop();
    }

    protected override void EntryCast(Actor actor)
    {
        _rangeObj = EffectManager.Instance.SphereRange(actor.transform.position, 34.0f, _myColor);
    }

    protected override void Casting(Actor actor)
    {

    }

    protected override void EndCast(Actor actor)
    {
        actor.GetAnimator().SetTrigger("Scream");
        _effect.transform.position = actor.transform.position;
        _effect.Play();
		SoundManager.PlaySFX("se_000");
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
            defActor.TakeDamage(210.0f);
        });
        col.SetSphereCollider(actor.transform.position, 34.0f);
    }

    protected override void Cancel(Actor actor)
    {
        GameObject.Destroy(_rangeObj);
        _effect.Stop();
    }
}
