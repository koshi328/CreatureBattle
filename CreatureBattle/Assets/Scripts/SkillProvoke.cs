using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillProvoke : SkillBase
{
    /// <summary>
    /// 初期化
    /// クールタイムやキャストタイムなどを設定する
    /// </summary>
    public override void Init()
    {
        base.Init();

        _coolTime = 10.0f;
        _castTime = 10.0f;
    }


    /// <summary>
    /// 効果を発動する
    /// </summary>
    override protected void Activate()
    {
        base.Activate();

        Debug.Log("挑発発動");
    }
}
