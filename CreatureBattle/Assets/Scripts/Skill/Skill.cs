using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skill
{
    /// <summary>
    /// 通常攻撃
    /// </summary>
    public class NormalAttack : SkillBase
    {
        int _damage;

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = _owner.GetAttackInterval() * 0.5f;
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
            int collisionLayer = 0;
            if(_owner.IsPlayer())
            {
                collisionLayer = VariableCollider.COLLISION_PLAYER_ATTACK;
            }
            else
            {
                collisionLayer = VariableCollider.COLLISION_MONSTER_ATTACK;
            }
            cm.EntrySphereCollider(collisionLayer, _owner, 1.0f, _damage, 0, _owner.transform.position, 2.0f, null);
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }
}