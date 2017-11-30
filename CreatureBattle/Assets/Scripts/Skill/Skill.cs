using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skill
{
    public class SkillSlash : SkillBase
    {
        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 1.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
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
            cm.ActiveSphereCollider(_owner, 5.0f, _owner.transform.position, 2.0f);
        }

        public override void Dispose()
        {
            _owner.AnimationSetTrigger("Idle");
        }
    }

    public class SkillShield : SkillBase
    {
        GameObject ShieldObj;

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 2.0f;
            REQUIREMENT_RECAST_TIME = 5.0f;
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
            ShieldObj = GameObject.Instantiate(Resources.Load("Prefabs/Shield")) as GameObject;
            ShieldObj.transform.position = _owner.transform.position;
        }

        public override void Dispose()
        {
            if (ShieldObj == null) return;
            GameObject.Destroy(ShieldObj);
        }
    }

}