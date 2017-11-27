using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase {
    protected Actor _owner;
    // 通常攻撃など実行したら中断できないアクション中はTrue
    protected bool _canDiscard = false;
    protected float REQUIREMENT_CAST_TIME;
    protected float REQUIREMENT_RECAST_TIME;
    protected float _castTime;
    protected float _recastTime;

    public virtual void Initialize(Actor owner)
    {
        _owner = owner;
    }

    public virtual SkillBase Execute(Actor owner)
    {
        _castTime -= Time.deltaTime;
        return null;
    }

    public virtual void Dispose()
    {

    }

    public bool CanDiscard()
    {
        return _canDiscard;
    }

    public SkillBase Clone()
    {
        SkillBase send = new SkillBase();
        send = this;
        return send;
    }
    
}
