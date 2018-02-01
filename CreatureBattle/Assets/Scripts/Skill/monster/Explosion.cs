using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using antilunchbox;

public class Explosion : SkillBase {

    GameObject _rangeObj;
    ParticleSystem _effect;
    public Explosion()
    {
        CAST_TIME = 2.2f;
        RECAST_TIME = 6.0f;
        ACTION_TIME = 1.0f;
        GameObject prefab = Resources.Load("Effect/KY_effects/AMFX02/Explosion") as GameObject;
        _effect = GameObject.Instantiate(prefab).GetComponent<ParticleSystem>();
        _effect.Stop();
    }

    protected override void EntryCast(Actor actor)
    {
        _rangeObj = EffectManager.Instance.SphereRange(actor.transform.position + actor.transform.forward * 15, 28.0f, _myColor);
    }

    protected override void Casting(Actor actor)
    {

    }

    protected override void EndCast(Actor actor)
    {
        actor.GetAnimator().SetTrigger("Scream");
        _effect.transform.position = actor.transform.position + actor.transform.forward * 15 + Vector3.up * 5;
        _effect.Play();
        if (!actor.GetPhotonView().isMine) return;
        SkillCollider col = ColliderManager.Instance.GetCollider();
        col.Initialize(actor, SkillCollider.HitTarget.Player, 0.5f, 99.0f, (defActor, atkActor) =>
        {
            defActor.TakeDamage(60.0f);
            defActor.AddCondition(ActorCondition.KIND.EXPLOSION, 6.0f, 0.0f);
        });
        col.SetSphereCollider(actor.transform.position + actor.transform.forward * 15, 28.0f);
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
        _effect.Stop();
    }
}
