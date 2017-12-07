using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatusAilment
{
    public enum KIND
    {
        STAN,
        SILENCE,
        BURN,
        ATK_UP,
        DEF_UP,
        MOV_UP,
        REC_UP,
        ATK_DOWN,
        DEF_DOWN,
        MOV_DOWN,
        REC_DOWN,
    }


    /// <summary>
    /// 基底クラス
    /// </summary>
    public class StatusAilmentBase
    {
        public KIND _kind { get; set; }
        protected Actor _owner;
        protected float _limitTime;
        protected float _limitTimer;
        public bool _isFinished { get; set; }

        /// <summary>
        /// 付与された時の初期化
        /// </summary>
        /// <param name="time"></param>
        public virtual void Initialize(KIND kind, Actor owner, float time)
        {
            // 種類
            _kind = kind;

            // キャラ
            _owner = owner;

            // 効果時間
            _limitTime = time;
            _limitTimer = time;

            // 効果時間が終了したか？
            _isFinished = false;
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            TakeEffect();

            _limitTimer -= Time.deltaTime;
            if (_limitTimer <= 0.0f)
            {
                _isFinished = true;
                Debug.Log("状態異常が解けた");
            }
        }

        /// <summary>
        /// 効果
        /// </summary>
        public virtual void TakeEffect()
        {
        }

        /// <summary>
        /// 付与対象を教えてもらう
        /// </summary>
        /// <param name="actor"></param>
        public void SetActor(Actor actor)
        {
            _owner = actor;
        }
    }

    /// <summary>
    /// スタン
    /// </summary>
    public class StatusStan : StatusAilmentBase
    {
        public override void Initialize(KIND kind, Actor owner, float time)
        {
            base.Initialize(kind, owner, time);
        }
    }

    /// <summary>
    /// サイレス
    /// </summary>
    public class StatusSilence : StatusAilmentBase
    {
        public override void Initialize(KIND kind, Actor owner, float time)
        {
            base.Initialize(kind, owner, time);
        }
    }

    /// <summary>
    /// 炎上
    /// </summary>
    public class StatusBurn : StatusAilmentBase
    {
        // ダメージ
        protected int _damage;
        // ダメージの間隔
        protected float _damageInterval;
        protected float _damageTimer;

        public void Initialize(KIND kind, Actor owner, float time, int damage, float damageInterval)
        {
            base.Initialize(kind, owner, time);
            _damage = damage;
            _damageInterval = damageInterval;
            _damageTimer = damageInterval;
        }

        public override void TakeEffect()
        {
            // 一定時間経過する度にダメージを与える
            if(_damageTimer <= 0.0f)
            {
                _damageTimer = _damageInterval;
                _owner.CallTakeDamage(_damage);
            }
        }
    }

    public class StatusAtkUp : StatusAilmentBase
    {
        // 倍率
        protected float _rate;
        
        public void Initialize(KIND kind, Actor owner, float time, float rate)
        {
            base.Initialize(kind, _owner, time);
            _rate = rate;
        }

        public override void TakeEffect()
        {
        }
    }
}
