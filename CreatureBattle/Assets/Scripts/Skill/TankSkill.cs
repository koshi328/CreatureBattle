using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skill
{
    public class FourAttack : SkillBase
    {
        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 1.0f;
            REQUIREMENT_ACTIVATION_TIME = 5.0f;
            REQUIREMENT_RECAST_TIME = 1.0f;
        }

        public override SkillBase MyUpdate()
        {
            base.MyUpdate();
            return this;
        }

        public override void Cast()
        {

        }

        public override void Activate()
        {
            _owner.AnimationSetTrigger("NormalAttack");

            ColliderManager cm = ColliderManager.GetInstance();
            cm.ActiveSphereCollider(VariableCollider.COLLISION_PLAYER_ATTACK, _owner, 5.0f, 10, _owner.transform.position, 2.0f);
        }

        public override void Dispose()
        {
            _owner.AnimationSetTrigger("Idle");
        }
    }
}