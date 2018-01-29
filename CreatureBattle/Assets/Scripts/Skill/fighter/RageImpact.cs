using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageImpact : SkillBase {
    GameObject _rangeObj;
    ParticleSystem _effect;
    public RageImpact()
    {
        CAST_TIME = 1.0f;
        RECAST_TIME = 7.0f;
        ACTION_TIME = 1.0f;
        GameObject prefab = Resources.Load("Effect/KY_effects/AMFX02/P_AMFX02_claw") as GameObject;
        _effect = GameObject.Instantiate(prefab).GetComponent<ParticleSystem>();
        _effect.transform.localScale = new Vector3(3.0f, 1.0f, 1.0f);
        _effect.Stop();
    }

    protected override void EntryCast(Actor actor)
    {
        _rangeObj = EffectManager.Instance.QuadRange(actor.transform.position + actor.transform.forward * 7.5f, actor.transform.eulerAngles.y, new Vector3(4, 15, 1), new Color(1, 0.5f, 0, 1));
    }

    protected override void Casting(Actor actor)
    {

    }

    protected override void EndCast(Actor actor)
    {
        actor.GetAnimator().SetTrigger("NormalAttack");
        Vector3 pos = actor.transform.position + actor.transform.forward * 9.0f;
        _effect.transform.position = pos;
        _effect.transform.eulerAngles = new Vector3(0, actor.transform.eulerAngles.y + 90, 0);
        _effect.Play();
        GameObject.Destroy(_rangeObj);
        if (!actor.GetPhotonView().isMine)return;
        SkillCollider col = ColliderManager.Instance.GetCollider();
        col.Initialize(actor, SkillCollider.HitTarget.Monster, 10.0f, 10.0f, (defActor, atkActor) =>
        {
            defActor.TakeDamage(35.0f);
        });
        col.SetQubeCollider(pos, actor.transform.rotation, new Vector3(4, 1, 15));
    }

    protected override void Action(Actor actor)
    {

    }

    protected override void EndAction(Actor actor)
    {

    }

    protected override void Cancel(Actor actor)
    {
        GameObject.Destroy(_rangeObj);
        _effect.Stop();
    }
}
