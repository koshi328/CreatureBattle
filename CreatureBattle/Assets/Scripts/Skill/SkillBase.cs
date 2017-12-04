using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// スキルの状態
public enum SKILL_STATE
{
    USABLE,
    CASTING,
    ACTIVATING,
    RECASTING,
}

public class SkillBase
{
    // スキルがどんな状態か？
    private SKILL_STATE _state;

    // このスキルを使うキャラクター
    protected Actor _owner;

    // 通常攻撃など実行したら中断できないアクション中はFalse
    protected bool _canDiscard = true;

    // キャストにかかる時間
    protected float REQUIREMENT_CAST_TIME;

    // キャストが終了してから効果が発動し切るまでの時間
    protected float REQUIREMENT_ACTIVATION_TIME;

    // リキャストにかかる時間
    protected float REQUIREMENT_RECAST_TIME;

    // 時間関係の処理のためのタイマー
    private float _timer;

    /// <summary>
    /// キャラクターから呼ばれる初期化
    /// </summary>
    /// <param name="owner"></param>
    public virtual void Initialize(Actor owner)
    {
        _owner = owner;
        _state = SKILL_STATE.USABLE;
        _timer = REQUIREMENT_CAST_TIME;
        _canDiscard = true;
    }


    /// <summary>
    /// キャストを開始する
    /// </summary>
    public void Execute()
    {
        _state = SKILL_STATE.CASTING;
        Cast();
    }


    /// <summary>
    /// 毎フレーム呼ばれる更新
    /// </summary>
    /// <returns></returns>
    public virtual SkillBase MyUpdate()
    {
        switch(_state)
        {
                // キャスト開始
            case SKILL_STATE.USABLE:
                
                break;

                // キャスト中
            case SKILL_STATE.CASTING:

                _timer -= Time.deltaTime;
                if (_timer <= 0.0f)
                {
                    _timer = REQUIREMENT_ACTIVATION_TIME;
                    _state = SKILL_STATE.ACTIVATING;
                    _canDiscard = false;
                    Activate();
                }
                break;

                // 発動
            case SKILL_STATE.ACTIVATING:

                _timer -= Time.deltaTime;
                if (_timer <= 0.0f)
                {
                    _timer = REQUIREMENT_RECAST_TIME;
                    _state = SKILL_STATE.RECASTING;
                    //Dispose();
                }
                break;

                // リキャスト中
            case SKILL_STATE.RECASTING:

                _timer -= Time.deltaTime;
                if(_timer <= 0.0f)
                {
                    _timer = REQUIREMENT_CAST_TIME;
                    _state = SKILL_STATE.USABLE;
                }
                break;
        }

        return null;
    }

    /// <summary>
    /// 詠唱開始した時に1回呼ばれる
    /// </summary>
    public virtual void Cast()
    {

    }

    /// <summary>
    /// 発動する時に1回呼ばれる関数
    /// </summary>
    public virtual void Activate()
    {

    }

    /// <summary>
    /// 終了した時の処理
    /// 予備動作とか予備エフェクトを削除する
    /// </summary>
    public virtual void Dispose()
    {

    }

    /// <summary>
    /// キャンセル可能か？
    /// </summary>
    /// <returns></returns>
    public bool CanDiscard()
    {
        return _canDiscard;
    }


    public SKILL_STATE GetState()
    {
        return _state;
    }

    public float GetRecastPer()
    {
        float per = 1.0f - _timer / REQUIREMENT_RECAST_TIME;
        return per;
    }
}
