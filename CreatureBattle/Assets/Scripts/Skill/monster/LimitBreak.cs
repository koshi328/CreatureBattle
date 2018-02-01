using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using antilunchbox;

public class LimitBreak : SkillBase
{
    public LimitBreak()
    {
        CAST_TIME = 0.0f;
        RECAST_TIME = 0.0f;
        ACTION_TIME = 0.0f;
    }

    protected override void Update(Actor actor)
    {
        float maxHp = actor.GetStatus().GetMaxHP();
        float hp = actor.GetStatus().GetHP();
        if (hp / maxHp <= 0.25f)
        {
            if (actor.GetCondition().GetCondition(ActorCondition.KIND.LIMIT_BREAK).GetStack() != 0) return;
            actor.AddCondition(ActorCondition.KIND.LIMIT_BREAK, 1.0f, 0.0f);
		    SoundManager.PlaySFX("se_002");
        }
        else
        {
            actor.GetCondition().GetCondition(ActorCondition.KIND.LIMIT_BREAK).SetTime(-0.1f);
        }
    }
}
