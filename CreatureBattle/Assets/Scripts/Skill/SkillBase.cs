using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase {
    // 通常攻撃など実行したら中断できないアクション中はTrue
    protected bool _canDiscard = false;
    protected float _castTime;
    protected float _recastTime;

    public virtual void Initialize()
    {

    }

    public virtual SkillBase Execute(Actor owner)
    {
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
