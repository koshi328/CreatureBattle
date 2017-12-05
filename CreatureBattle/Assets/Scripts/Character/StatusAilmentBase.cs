using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusAilmentBase
{
    public enum KIND
    {
        STAN,   // スタン
        SILENCE,// サイレス
        BURN,   // 炎上
    }

    private KIND _kind;

    private float _limitTime;



    public void Initialize(KIND kind, float time)
    {
        _kind = kind;
        _limitTime = time;
    }

    public KIND GetKind()
    {
        return _kind;
    }
}
