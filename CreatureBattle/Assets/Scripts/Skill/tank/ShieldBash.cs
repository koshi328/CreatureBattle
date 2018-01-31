using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using antilunchbox;

public class ShieldBash : SkillBase {

    ParticleSystem _effect;
    public ShieldBash()
    {
        CAST_TIME = 1.2f;
        RECAST_TIME = 10.0f;
        ACTION_TIME = 1.0f;
        GameObject prefab = Resources.Load("Effect/KY_effects/AMFX02/P_AMFX02_slash1") as GameObject;
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
        actor.GetAnimator().SetTrigger("Attack03");
        _effect.transform.position = actor.transform.position + actor.transform.forward * 5 + new Vector3(0, 2, 0);
        _effect.transform.eulerAngles = new Vector3(0, actor.transform.eulerAngles.y + 90, 0);
        _effect.Play();
        if (!actor.GetPhotonView().isMine) return;
        SkillCollider col = ColliderManager.Instance.GetCollider();
        col.Initialize(actor, SkillCollider.HitTarget.Monster, 2.0f, 2.0f, (defActor, atkActor) =>
        {
            defActor.TakeDamage(90);
            defActor.AddCondition(ActorCondition.KIND.STAN, 1.0f, 0.0f, false);
        });
        col.SetFanCollider(actor.transform.position, 7.0f, actor.transform.forward, 90.0f);
		SoundManager.PlaySFX("se_040");
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
