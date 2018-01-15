using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitBreak : SkillBase
{
    public LimitBreak()
    {
        CAST_TIME = 0.0f;
        RECAST_TIME = 10.0f;
        ACTION_TIME = 1.0f;
    }

    protected override void EntryCast(Actor actor)
    {

    }

    protected override void Casting(Actor actor)
    {

    }

    protected override void EndCast(Actor actor)
    {
        actor.AddCondition(ActorCondition.KIND.ANGRY_SHOUT, 20.0f, 35.0f);
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
