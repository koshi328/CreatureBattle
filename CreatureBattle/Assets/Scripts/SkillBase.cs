using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// スキルの基底クラス
/// </summary>
public class SkillBase : MonoBehaviour
{
    // 状態
    public enum STATE
    {
        USABLE,     // 使用可能
        CASTING,    // キャスト中
        COOLING,    // クール中
    }


    // 状態
    protected STATE _currentState;
    
    // スキルの対象
    protected List<GameObject> _targets;

    // クールタイム
    protected float _coolTime;

    // キャストタイム
    protected float _castTime;

    // 現在の経過時間
    protected float _timeCounter;


    /// <summary>
    /// 初期化
    /// </summary>
    virtual public void Init()
    {
        _currentState = STATE.USABLE;
    }


    /// <summary>
    /// 詠唱開始
    /// </summary>
    virtual public void CastStart()
    {
        switch (_currentState)
        {
            // 使用可能
            case STATE.USABLE:

                // 詠唱を始める
                _timeCounter = 0.0f;
                ChangeState(STATE.CASTING);
                Debug.Log("詠唱開始");

                break;

            // 詠唱中
            case STATE.CASTING:

                Debug.Log("詠唱中のため使用できません");

                break;

            // クールタイム中
            case STATE.COOLING:

                Debug.Log("クールタイム中のため使用できません");

                break;
        }
    }


    /// <summary>
    /// 更新
    /// </summary>
    public void MyUpdate()
    {
        switch(_currentState)
        {
            // 使用可能
            case STATE.USABLE:

                break;

            // 詠唱中
            case STATE.CASTING:

                _timeCounter += Time.deltaTime;
                if(_castTime < _timeCounter)
                {
                    // 発動
                    Activate();

                    // カウンターを初期化
                    _timeCounter = 0.0f;

                    // 発動したのでクールタイムに
                    ChangeState(STATE.COOLING);
                }

                break;

            // クールタイム中
            case STATE.COOLING:

                _timeCounter += Time.deltaTime;
                if (_coolTime < _timeCounter)
                {
                    // カウンターを初期化
                    _timeCounter = 0.0f;

                    // クールタイムが終了したので使用可能に
                    ChangeState(STATE.USABLE);
                }

                break;
        }
    }


    /// <summary>
    /// 効果を発動する
    /// 継承先で定義する
    /// </summary>
    virtual protected void Activate()
    {
    }


    /// <summary>
    /// 詠唱をキャンセル
    /// </summary>
    public void CastCancel()
    {
        ChangeState(STATE.USABLE);
    }


    /// <summary>
    /// 状態を変更
    /// </summary>
    /// <param name="state"></param>
    private void ChangeState(STATE state)
    {
        _currentState = state;
    }


    /// <summary>
    /// 状態を返す
    /// </summary>
    /// <returns></returns>
    public STATE GetState()
    {
        return _currentState;
    }
}
