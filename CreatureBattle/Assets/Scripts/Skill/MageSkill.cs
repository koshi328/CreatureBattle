using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatusAilment;

namespace MageSkill
{
    /// <summary>
    /// フロストエナジー
    /// </summary>
    public class FrostEnergy : SkillBase
    {
        int _damage;
        Vector3 _vec;
        SphereCollider _sphereCollider;

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 8.0f;
            _damage = 50;
            _sphereCollider = null;
        }

        public override SkillBase MyUpdate()
        {
            base.MyUpdate();

            if (_sphereCollider != null &&
                _sphereCollider.gameObject.GetActive() != false)
            {
                _sphereCollider.transform.position = _sphereCollider.transform.position + _vec * Time.deltaTime;
            }
            else
            {
                _sphereCollider = null;
            }

            return this;
        }

        public override void Cast()
        {
            _owner.AnimationSetTrigger("Idle");
        }

        public override void Activate()
        {
            _owner.AnimationSetTrigger("NormalAttack");

            // プレイヤーの向いてる方向へのベクトルを取る
            _vec = _owner.transform.forward;
            _vec.y = 0.0f;
            _vec *= 6.0f;

            if (_owner.GetPhotonView().isMine == false) return;

            StatusAilmentBase stan = new StatusStan(null, KIND.STAN, 2.0f);
            ColliderManager cm = ColliderManager.GetInstance();
            _sphereCollider = cm.EntrySphereCollider(VariableCollider.COLLISION_PLAYER_ATTACK, _owner, 5.0f, _damage, _owner.transform.position, 1.0f, null);
            _sphereCollider.transform.position = _owner.transform.position;
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }
}
