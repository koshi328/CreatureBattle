﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase {

    public enum STATE
    {
        WAIT,
        CASTING,
        ACTION,
        RECASTING
    }

    protected float CAST_TIME = 1.0f;
    protected float ACTION_TIME = 1.0f;
    protected float RECAST_TIME = 1.0f;
    protected float _timer;
    protected STATE _state;

    public float GetCastTime()
    {
        return CAST_TIME;
    }
    public float GetActionTime()
    {
        return ACTION_TIME;
    }
    public float GetReCastTime()
    {
        return RECAST_TIME;
    }
    public float GetTimer()
    {
        return _timer;
    }
    public void Select(Actor actor)
    {
        _timer = CAST_TIME;
        _state = STATE.CASTING;
        EntryCast(actor);
    }
    public bool NowCasting()
    {
        return _state == STATE.CASTING;
    }
    public bool NowAction()
    {
        return _state == STATE.ACTION;
    }
    public bool NowReCasting()
    {
        return _state == STATE.RECASTING;
    }
    public bool NowWaiting()
    {
        return _state == STATE.WAIT;
    }

    public void Execute(Actor actor)
    {
        switch(_state)
        {
            case STATE.CASTING:
                Casting(actor);
                _timer -= Time.deltaTime;
                if (_timer <= 0.0f)
                {
                    _timer = ACTION_TIME;
                    _state = STATE.ACTION;
                    EndCast(actor);
                }
                break;
            case STATE.ACTION:
                Action(actor);
                _timer -= Time.deltaTime;
                if (_timer <= 0.0f)
                {
                    _timer = RECAST_TIME;
                    _state = STATE.RECASTING;
                    EndAction(actor);
                }
                break;
            case STATE.RECASTING:
                Recasting();
                break;
        }
    }
    public void Dispose(Actor actor)
    {
        _state = STATE.WAIT;
        _timer = CAST_TIME;
        Cancel(actor);
    }
    // virtual ---------------------
    // キャスト開始
    protected virtual void EntryCast(Actor actor)
    {
        Debug.Log("EntryCast");
    }
    // キャスト中
    protected virtual void Casting(Actor actor)
    {
        Debug.Log("Casting");
    }
    // キャストが終わったとき
    protected virtual void EndCast(Actor actor)
    {
        Debug.Log("EndCast");
    }
    // アクション中
    protected virtual void Action(Actor actor)
    {
        Debug.Log("Action");
    }
    // アクションが終わったとき
    protected virtual void EndAction(Actor actor)
    {
        Debug.Log("EndAction");
    }
    // アクションがキャンセルされた
    protected virtual void Cancel(Actor actor)
    {
        Debug.Log("Cancel");
    }
    // -----------------------------
    protected void Recasting()
    {
        _timer -= Time.deltaTime;
        if(_timer <= 0.0f)
        {
            _timer = CAST_TIME;
            _state = STATE.WAIT;
        }
    }
}
