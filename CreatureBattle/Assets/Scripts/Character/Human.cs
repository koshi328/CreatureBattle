using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Actor {

    public override void Movement(Vector3 dir, float speed)
    {
        // アニメーションの再生など

        // 移動処理　方向転換
        base.Movement(dir, speed);
    }
}
