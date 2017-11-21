using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 火の球を飛ばす
/// </summary>
public class SkillFireball : SkillBase
{
    // 基礎ダメージ
    protected int _baseDamage;

    // ダメージの振れ幅
    protected int _damageRange;


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

        Debug.Log("ファイアボール発動");
    }
}
