using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using antilunchbox;

public class AggressiveShout : SkillBase
{
    GameObject _rangeObj;
    ParticleSystem _effect;
    public AggressiveShout()
    {
        CAST_TIME = 0.0f;
        RECAST_TIME = 20.0f;
        ACTION_TIME = 1.0f;

        GameObject prefab = Resources.Load("Effect/KY_effects/AMFX02/AgressiveShout") as GameObject;
        _effect = GameObject.Instantiate(prefab).GetComponent<ParticleSystem>();
        _effect.Stop();
    }

    protected override void EntryCast(Actor actor)
    {
        _rangeObj = EffectManager.Instance.SphereRange(actor.transform.position, 20.0f, _myColor);
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
        col.Initialize(actor, SkillCollider.HitTarget.Player, 2.0f, 2.0f, (defActor, atkActor) =>
        {
            defActor.AddCondition(ActorCondition.KIND.AGGRESSIVE_SHOUT, 10.0f, 0.0f);
        });
        col.SetSphereCollider(actor.transform.position, 20.0f);
		SoundManager.PlaySFX("se_001");
    }

    protected override void Action(Actor actor)
    {

    }

    protected override void EndAction(Actor actor)
    {
        GameObject.Destroy(_rangeObj.gameObject);
        _effect.Stop();
    }

    protected override void Cancel(Actor actor)
    {
        GameObject.Destroy(_rangeObj.gameObject);
        _effect.Stop();
    }
}
