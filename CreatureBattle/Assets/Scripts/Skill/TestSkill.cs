using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class TestSkill : SkillBase {


    public override void Initialize(Actor owner)
    {
        base.Initialize(owner);
        REQUIREMENT_CAST_TIME = 0.0f;
        REQUIREMENT_RECAST_TIME = 5.0f;
        _castTime = REQUIREMENT_CAST_TIME;
        _recastTime = REQUIREMENT_RECAST_TIME;
    }
    public override SkillBase Execute(Actor owner)
    {

        if (_castTime >= REQUIREMENT_CAST_TIME)
        {
            
        }

        base.Execute(owner);

        if (_castTime <= 0)
        {
            owner.AnimationSetTrigger("NormalAttack");

            ColliderManager cm = ColliderManager.GetInstance();
            cm.ActiveSphereCollider(owner, 5.0f, owner.transform.position, 2.0f);
            
            owner.CancelAction();

            OnFinished();

            return null;
        }
        return this;
    }

    public override void Dispose()
    {

    }

    private void OnFinished()
    {
        _owner.AnimationSetTrigger("Idle");

    }
}

public class Shield : SkillBase
{
    GameObject ShieldObj;
    public override void Initialize(Actor owner)
    {
        base.Initialize(owner);
        _castTime = 1.5f;
        _recastTime = 0.0f;
    }
    public override SkillBase Execute(Actor owner)
    {
        if(_castTime >= 1.5f)
        {
            ShieldObj = GameObject.Instantiate(Resources.Load("Prefabs/Shield")) as GameObject;
        }
        ShieldObj.transform.position = owner.transform.position;
        _castTime -= Time.deltaTime;
        if (_castTime <= 0)
        {
            GameObject.Destroy(ShieldObj);
            return null;
        }
        return this;
    }

    public override void Dispose()
    {
        if (ShieldObj == null) return;
        GameObject.Destroy(ShieldObj);
    }
}