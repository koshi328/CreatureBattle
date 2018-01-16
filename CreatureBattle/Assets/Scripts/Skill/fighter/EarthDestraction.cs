using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthDestraction : SkillBase {

    GameObject _rangeObj;
    ParticleSystem _effect;
    public EarthDestraction()
    {
        CAST_TIME = 1.0f;
        RECAST_TIME = 15.0f;
        ACTION_TIME = 1.0f;
        
        GameObject prefab = Resources.Load("Effect/KY_effects/AMFX02/P_AMFX02_thunder6") as GameObject;
        _effect = GameObject.Instantiate(prefab).GetComponent<ParticleSystem>();
        _effect.Stop();
    }

    protected override void EntryCast(Actor actor)
    {
        _rangeObj = EffectManager.Instance.SphereRange(actor.transform.position, 10.0f, new Color(1, 0.5f, 0, 1));
    }

    protected override void Casting(Actor actor)
    {

    }

    protected override void EndCast(Actor actor)
    {
        _effect.transform.position = actor.transform.position;
        _effect.Play();
        GameObject.Destroy(_rangeObj.gameObject);
        if (!actor.GetPhotonView().isMine) return;
        SkillCollider col = ColliderManager.Instance.GetCollider();
        col.Initialize(actor, SkillCollider.HitTarget.Monster, 0.1f, 0.1f, (argActor) =>
        {
            argActor.AddCondition(ActorCondition.KIND.EARTH_DESTRACTION, 10.0f, 0.0f);
            argActor.TakeDamage(75.0f);
        });
        col.SetSphereCollider(actor.transform.position, 6.0f);
    }

    protected override void Action(Actor actor)
    {

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
