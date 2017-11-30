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
            cm.ActiveSphereCollider(_owner, 5.0f, 10, _owner.transform.position, 2.0f);
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


    public class SkillFirebolt : SkillBase
    {
        GameObject _firebolt;

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 0.5f;
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
            // エフェクト生成
            _firebolt = GameObject.Instantiate(Resources.Load("Prefabs/Firebolt")) as GameObject;

            // 座標をちょっとプレイヤーの座標より上に
            Vector3 pos = _owner.transform.position;
            pos.y += 1.0f;

            // 前に
            float yRot = _owner.transform.rotation.eulerAngles.y;
            Vector3 forwardY = Quaternion.Euler(0.0f, yRot, 0.0f) * Vector3.forward;
            pos += forwardY;

            _firebolt.transform.position = pos;
            _firebolt.transform.rotation = _owner.transform.rotation;
        }

        public override void Dispose()
        {
            // アセットの力で勝手に消えるのでOK
        }
    }
}