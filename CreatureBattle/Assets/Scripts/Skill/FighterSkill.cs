using System.Collections;
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
            REQUIREMENT_RECAST_TIME = 0.0f;
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
            // effect -----------------------------------
            float angle = Mathf.Acos(Vector3.Dot(new Vector3(-1, 0, 0), _owner.transform.forward)) * 180 / 3.14f;
            if (_owner.transform.forward.z > 0) angle *= -1;
            Quaternion rot = Quaternion.Euler(new Vector3(90, 0, angle));
            GameObject obj = EffectManager.Instance.SlashEffect(_owner.transform.position + Vector3.up, rot);
            //-------------------------------------------
            _owner.AnimationSetTrigger("NormalAttack");

            if (_owner.GetPhotonView().isMine == false) return;
            ColliderManager cm = ColliderManager.GetInstance();

            StatusAilmentBase stan = new StatusAilmentBase(null, KIND.STAN, 2.0f);
            cm.EntryFanCollider(VariableCollider.COLLISION_PLAYER_ATTACK, _owner, 1.0f, _damage, _owner.transform.position, 2.0f, _owner.transform.eulerAngles, 30.0f, new StatusAilmentBase[] { stan });
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }
}
