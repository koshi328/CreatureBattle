using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbnormalCounter : SkillBase {
    GameObject _rangeObj;
    public AbnormalCounter()
    {
        CAST_TIME = 3.0f;
        RECAST_TIME = 10.0f;
        ACTION_TIME = 0.0f;
    }

    protected override void EntryCast(Actor actor)
    {
        _rangeObj = EffectManager.Instance.SphereRange(actor.transform.position, 34.0f, _myColor);
        actor.AddCondition(ActorCondition.KIND.ABNORMAL_COUNTER, CAST_TIME, 0.0f);
    }

    protected override void Casting(Actor actor)
    {

    }

    protected override void EndCast(Actor actor)
    {

    }

    protected override void Action(Actor actor)
    {

    }

    protected override void EndAction(Actor actor)
    {
        GameObject.Destroy(_rangeObj);
    }

    protected override void Cancel(Actor actor)
    {
        actor.AddCondition(ActorCondition.KIND.ABNORMAL_COUNTER, -0.1f, 0.0f);
        GameObject.Destroy(_rangeObj);
    }
}
