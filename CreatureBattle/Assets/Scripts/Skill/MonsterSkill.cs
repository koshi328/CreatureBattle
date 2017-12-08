using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatusAilment;

namespace MonsterSkill
{
    /// <summary>
    /// キャパシティら伊豆
    /// </summary>
    public class CapacityRise : SkillBase
    {
        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 20.0f;
        }

        public override SkillBase MyUpdate()
        {
            base.MyUpdate();
            return this;
        }

        public override void Cast()
        {
            _owner.AnimationSetTrigger("Idle");
        }

        public override void Activate()
        {
            _owner.AnimationSetTrigger("NormalAttack");

            // 通常攻撃速度2倍
            _owner.CallAddStatusAilment3((int)KIND.ATK_SPD_UP, 10.0f, 2.0f);
            // 通常攻撃ダメージ3.1倍
            _owner.CallAddStatusAilment3((int)KIND.ATK_UP, 10.0f, 3.1f);
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// ドラゴンストーム
    /// </summary>
    public class DragonStorm : SkillBase
    {
        int _damage;

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 7.0f;
            _damage = 141;
        }

        public override SkillBase MyUpdate()
        {
            base.MyUpdate();
            return this;
        }

        public override void Cast()
        {
            _owner.AnimationSetTrigger("Idle");
        }

        public override void Activate()
        {
            _owner.AnimationSetTrigger("NormalAttack");

            if (_owner.GetPhotonView().isMine == false) return;

            // サイレンス
            StatusAilmentBase silence = new StatusStan(null, KIND.SILENCE, 5.0f);

            // 周囲にダメージ&当たった対象にサイレンスを付与
            ColliderManager cm = ColliderManager.GetInstance();
            cm.EntrySphereCollider(VariableCollider.COLLISION_MONSTER_ATTACK, _owner, 1.0f, _damage, _owner.transform.position, 3.0f, new StatusAilmentBase[]{ silence });
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// アブノーマルカウンター
    /// </summary>
    public class AbnormalCounter : SkillBase
    {
        int _damage;

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 7.0f;
            _damage = 227;
        }

        public override SkillBase MyUpdate()
        {
            base.MyUpdate();
            return this;
        }

        public override void Cast()
        {
            _owner.AnimationSetTrigger("Idle");
        }

        public override void Activate()
        {
            _owner.AnimationSetTrigger("NormalAttack");

            // カウンタースタンを自分に付与
            _owner.CallAddStatusAilment((int)KIND.COUNTER_STAN, 3.0f);
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// スタンブレス
    /// </summary>
    public class StanBreath : SkillBase
    {
        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 30.0f;
        }

        public override SkillBase MyUpdate()
        {
            base.MyUpdate();
            return this;
        }

        public override void Cast()
        {
            _owner.AnimationSetTrigger("Idle");
        }

        public override void Activate()
        {
            _owner.AnimationSetTrigger("NormalAttack");


            if (_owner.GetPhotonView().isMine == false) return;

            // スタン
            StatusAilmentBase stan = new StatusStan(null, KIND.STAN, 8.0f);

            // 炎上
            StatusAilmentBase burn = new StatusBurn(null, KIND.BURN, 8.0f, 60, 1.0f);

            // 前方扇形の当たり判定
            ColliderManager cm = ColliderManager.GetInstance();
            cm.EntryFanCollider(VariableCollider.COLLISION_MONSTER_ATTACK, _owner, 1.0f, 0, _owner.transform.position, 5.0f, _owner.transform.eulerAngles, 30.0f, new StatusAilmentBase[] { stan, burn });
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }
}
