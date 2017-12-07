﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatusAilment;

namespace FighterSkill
{
    /// <summary>
    /// 連続攻撃
    /// </summary>
    public class ContinuousAttack : SkillBase
    {
        int _damage;

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 8.0f;
            _damage = _owner.GetAttackDamage();
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
            ColliderManager cm = ColliderManager.GetInstance();
            for (int i = 0; i < 10; i++)
            {
                cm.EntrySphereCollider(VariableCollider.COLLISION_PLAYER_ATTACK, _owner, 1.0f, _damage, _owner.transform.position, 2.0f, null);
            }
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// 緊急回避
    /// </summary>
    public class EmergencyAvoid : SkillBase
    {
        Vector3 _vec;

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 0.5f;
            REQUIREMENT_RECAST_TIME = 4.0f;
        }

        public override SkillBase MyUpdate()
        {
            base.MyUpdate();
            if(GetState() == SKILL_STATE.ACTIVATING)
            {
                _owner.transform.position = _owner.transform.position + _vec * Time.deltaTime;
            }
            return this;
        }

        public override void Cast()
        {
            _owner.AnimationSetTrigger("Idle");
        }

        public override void Activate()
        {
            // プレイヤーの向いてる方向の反対へのベクトルを取る
            _vec = _owner.transform.forward;
            _vec.y = 0.0f;
            _vec = -_vec;
            _vec *= 6.0f;
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// トリップスタン
    /// </summary>
    public class TripStan : SkillBase
    {
        int _damage;

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 15.0f;
            _damage = 33;
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
            ColliderManager cm = ColliderManager.GetInstance();

            StatusAilmentBase stan = new StatusStan();
            stan.Initialize(KIND.STAN, null, 2.0f);
            List<StatusAilment.StatusAilmentBase> statusAilments = new List<StatusAilment.StatusAilmentBase>();
            statusAilments.Add(stan);
            cm.EntryFanCollider(VariableCollider.COLLISION_PLAYER_ATTACK, _owner, 1.0f, _damage, _owner.transform.position, 2.0f, _owner.transform.eulerAngles, 30.0f, statusAilments);
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }
}
