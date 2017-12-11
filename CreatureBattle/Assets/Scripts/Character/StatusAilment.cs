using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatusAilment
{
    public enum KIND
    {
        // 状態異常
        STAN,
        SILENCE,
        BAN_REC,
        COUNTER_STAN,

        // スリップダメージ
        BURN,

        // バフ
        ATK_UP,
        DEF_UP,
        MOV_UP,
        REC_UP,
        ATK_SPD_UP,
        //BAN_ 行動妨害スキルを無効化する効果

        // デバフ
        ATK_DOWN,
        DEF_DOWN,
        MOV_DOWN,
        REC_DOWN,

        // 累積デバフ
        MOV_DOWN_DUP,
    }

    /// <summary>
    /// シングルトンの状態異常クリエイター
    /// </summary>
    public class StatusAilmentCreator
    {
        static StatusAilmentCreator _instance;

        public static StatusAilmentCreator GetInstance()
        {
            if (_instance == null)
            {
                _instance = new StatusAilmentCreator();
            }
            return _instance;
        }

        public StatusAilmentBase GetStatusAilment(Actor owner, int kind, float time)
        {
            switch (kind)
            {
                case (int)KIND.STAN: return new StatusStan(owner, (StatusAilment.KIND)kind, time);
                case (int)KIND.SILENCE: return new StatusSilence(owner, (StatusAilment.KIND)kind, time);
                default: return new StatusStan(owner, (StatusAilment.KIND)kind, time);
            }
        }

        public StatusAilmentBase GetStatusAilment2(Actor owner, int kind, float time, int damage, float damageInterval)
        {
            switch (kind)
            {
                case (int)KIND.BURN: return new StatusBurn(owner, (StatusAilment.KIND)kind, time, damage, damageInterval);
                default: return null;
            }
        }

        public StatusAilmentBase GetStatusAilment3(Actor owner, int kind, float time, float rate)
        {
            return new StatusBuff(owner, (StatusAilment.KIND)kind, time, rate);
        }
    }
    
    /// <summary>
    /// 基底クラス
    /// </summary>
    public class StatusAilmentBase
    {
        public KIND _kind { get; set; }
        protected Actor _owner;
        public float _limitTime { get; set; }
        protected float _limitTimer;
        public bool _isFinished { get; set; }

        /// <summary>
        /// 付与された時の初期化
        /// </summary>
        /// <param name="time"></param>
        public StatusAilmentBase(Actor owner, KIND kind, float time)
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
    }

    /// <summary>
    /// スタン
    /// </summary>
    public class StatusStan : StatusAilmentBase
    {
        public StatusStan(Actor owner, KIND kind, float time)
            :base(owner, kind, time)
        {
        }
    }

    /// <summary>
    /// サイレス
    /// </summary>
    public class StatusSilence : StatusAilmentBase
    {
        public StatusSilence(Actor owner, KIND kind, float time)
            :base(owner, kind, time)
        {
        }
    }

    /// <summary>
    /// カウンタースタン
    /// </summary>
    public class StatusCounterStan : StatusAilmentBase
    {
        public StatusCounterStan(Actor owner, KIND kind, float time)
            : base(owner, kind, time)
        {
        }
    }

    /// <summary>
    /// 炎上
    /// </summary>
    public class StatusBurn : StatusAilmentBase
    {
        // ダメージ
        public int _damage { get; set; }
        // ダメージの間隔
        public float _damageInterval { get; set; }
        public float _damageTimer { get; set; }

        public StatusBurn(Actor owner, KIND kind, float time, int damage, float damageInterval)
            :base(owner, kind, time)
        {
            _damage = damage;
            _damageInterval = damageInterval;
            _damageTimer = damageInterval;
        }

        public override void TakeEffect()
        {
            _damageTimer -= Time.deltaTime;

            // 一定時間経過する度にダメージを与える
            if (_damageTimer <= 0.0f)
            {
                Debug.Log(_damage);
                _damageTimer = _damageInterval;
                if(_owner.GetPhotonView().isMine)
                    _owner.CallTakeDamage(_damage);
            }
        }
    }

    public class StatusBuff : StatusAilmentBase
    {
        // 倍率
        public float _rate { get; set; }

        public StatusBuff(Actor owner, KIND kind, float time, float rate)
            :base(owner, kind, time)
        {
            _rate = rate;
        }

        public override void TakeEffect()
        {
            // 種類によって処理を変える
            switch(_kind)
            {
                case KIND.ATK_UP:

                    break;
                case KIND.DEF_UP:

                    break;
                case KIND.MOV_UP:

                    break;
                case KIND.REC_UP:

                    break;
                case KIND.ATK_SPD_UP:

                    break;

                case KIND.ATK_DOWN:

                    break;
                case KIND.DEF_DOWN:

                    break;
                case KIND.MOV_DOWN:
                    _owner._maxSpeed = _owner._defaultSpeed * (1.0f - _rate);
                    break;
                case KIND.REC_DOWN:

                    break;
                case KIND.MOV_DOWN_DUP:
                    _owner._maxSpeed = _owner._maxSpeed * (1.0f - _rate);
                    break;
            }
        }
    }
}
