using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeWave : SkillBase {
    ParticleSystem _effect;
    public InitializeWave()
    {
        CAST_TIME = 1.0f;
        RECAST_TIME = 20.0f;
        ACTION_TIME = 1.0f;

        GameObject prefab = Resources.Load("Effect/ItoEffects/InitializeWave") as GameObject;
        _effect = GameObject.Instantiate(prefab).GetComponent<ParticleSystem>();
        _effect.Stop();
    }

    protected override void EntryCast(Actor actor)
    {
        actor.GetAnimator().SetTrigger("Scream");
    }

    protected override void Casting(Actor actor)
    {

    }

    protected override void EndCast(Actor actor)
    {
        _effect.transform.position = actor.transform.position + Vector3.up * 4;
        _effect.Play();
        if (!actor.GetPhotonView().isMine) return;
        actor.AddCondition(ActorCondition.KIND.INITIALIZE_WAVE, 10.0f, 0.0f);
        actor.GetCondition().ClearDebuff();
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
