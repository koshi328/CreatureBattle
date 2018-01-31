using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuddiProtect : SkillBase {
    float _time;
    public StuddiProtect()
    {
        CAST_TIME = 0.0f;
        RECAST_TIME = 0.0f;
        ACTION_TIME = 0.0f;
    }
    protected override void Update(Actor actor)
    {
        if (!actor.GetPhotonView().isMine) return;
        if (actor.GetCondition().GetCondition(ActorCondition.KIND.STUDII_PROTECT).GetStack() == 0)
        {
            _time += Time.deltaTime;
            if (_time >= 20.0f)
            {
                _time = 0.0f;
                actor.AddCondition(ActorCondition.KIND.STUDII_PROTECT, 1.0f, 0.0f);
            }
        }
        else
        {
            _time = 0.0f;
        }
    }
}
