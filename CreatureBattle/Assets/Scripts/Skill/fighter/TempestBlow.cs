using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using antilunchbox;

public class TempestBlow : SkillBase {

    GameObject _rangeObj;
    ParticleSystem _effect;
    float time = 0.0f;
    public TempestBlow()
    {
        CAST_TIME = 0.5f;
        RECAST_TIME = 10.0f;
        ACTION_TIME = 1.2f;
        GameObject prefab = Resources.Load("Effect/KY_effects/AMFX02/P_AMFX02_slash2") as GameObject;
        _effect = GameObject.Instantiate(prefab).GetComponent<ParticleSystem>();
        _effect.Stop();
    }
    
    protected override void EntryCast(Actor actor)
    {
        time = 0.0f;
        _rangeObj = EffectManager.Instance.FanRange(actor.transform.position, actor.transform.eulerAngles.y, 10, 45, _myColor);
    }

    protected override void Casting(Actor actor)
    {

    }

    protected override void EndCast(Actor actor)
    {
        _effect.transform.position = actor.transform.position + actor.transform.forward * 2.0f;
        _effect.transform.rotation = actor.transform.rotation;

        GameObject.Destroy(_rangeObj.gameObject);
        actor.GetAnimator().SetTrigger("Slash2");
        if (!actor.GetPhotonView().isMine) return;
        SkillCollider col = ColliderManager.Instance.GetCollider();
        col.Initialize(actor, SkillCollider.HitTarget.Monster, 1.2f, 0.2f, (defActor, atkActor) =>
        {
            defActor.AddCondition(ActorCondition.KIND.TEMPEST_BLOW, 10.0f, 0.0f);
            defActor.TakeDamage(16.0f, atkActor);
        });
        col.SetFanCollider(actor.transform.position, 10.0f, actor.transform.forward, 45.0f);
		SoundManager.PlaySFX("se_028");
    }

    protected override void Action(Actor actor)
    {
        time += Time.deltaTime;
        if (time >= 0.4f)
        {
            _effect.Stop();
            time = 0.0f;
            _effect.Play();
        }
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
