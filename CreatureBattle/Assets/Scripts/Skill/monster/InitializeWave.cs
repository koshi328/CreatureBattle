using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeWave : SkillBase {
    
    public InitializeWave()
    {
        CAST_TIME = 0.5f;
        RECAST_TIME = 30.0f;
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
        if (!actor.GetPhotonView().isMine) return;
        actor.AddCondition(ActorCondition.KIND.INITIALIZE_WAVE, 10.0f, 0.0f);
        actor.GetCondition().ClearDebuff();
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
