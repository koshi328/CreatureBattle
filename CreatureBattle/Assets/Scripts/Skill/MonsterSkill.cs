using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterSkill
{
    /// <summary>
    /// キャパシティら伊豆
    /// </summary>
    public class CapacityRise : SkillBase
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
}
