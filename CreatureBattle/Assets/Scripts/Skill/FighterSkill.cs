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
                cm.EntrySphereCollider(VariableCollider.COLLISION_PLAYER_ATTACK, _owner, 1.0f, _damage, 0, _owner.transform.position, 2.0f, null);
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
            cm.EntryFanCollider(VariableCollider.COLLISION_PLAYER_ATTACK, _owner, 1.0f, _damage, 5, _owner.transform.position, 2.0f, _owner.transform.eulerAngles, 30.0f, new StatusAilmentBase[] { stan });
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// テンペストブロー
    /// </summary>
    public class TempestBlow : SkillBase
    {
        int _damage;

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 10.0f;
            _damage = 8;
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

            // 被ダメージ上昇の状態異常
            StatusAilmentBase damageUp = new StatusBuff(null, KIND.DAMAGE_UP, 10.0f, 0.02f);
            ColliderManager cm = ColliderManager.GetInstance();
            for (int i = 0; i < 10; i++)
            {
                cm.EntrySphereCollider(VariableCollider.COLLISION_PLAYER_ATTACK, _owner, 1.0f, _damage, 5, _owner.transform.position, 1.0f, new StatusAilmentBase[] { damageUp });
            }
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// アースデストラクション
    /// </summary>
    public class EarthDestruction : SkillBase
    {
        int _damage;

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 2.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 15.0f;
            _damage = 75;
        }

        public override SkillBase MyUpdate()
        {
            base.MyUpdate();
            return this;
        }

        public override void Cast()
        {
            _owner.AnimationSetTrigger("Idle");
            _owner.AddStatusAilment((int)KIND.NO_DAMAGE, 2.0f);
        }

        public override void Activate()
        {
            _owner.AnimationSetTrigger("NormalAttack");

            if (_owner.GetPhotonView().isMine == false) return;

            // スタン
            StatusAilmentBase stan = new StatusAilmentBase(null, KIND.STAN, 2.0f);
            ColliderManager cm = ColliderManager.GetInstance();
            cm.EntrySphereCollider(VariableCollider.COLLISION_PLAYER_ATTACK, _owner, 1.0f, _damage, 5, _owner.transform.position, 3.0f, new StatusAilmentBase[] { stan });
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// クロスカウンター
    /// </summary>
    public class CrossCounter : SkillBase
    {
        int _damage;

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 3.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 12.0f;
            _damage = 60;
        }

        public override SkillBase MyUpdate()
        {
            base.MyUpdate();
            return this;
        }

        public override void Cast()
        {
            _owner.AnimationSetTrigger("Idle");
            _owner.AddStatusAilment((int)KIND.NO_DAMAGE, 3.0f);
        }

        public override void Activate()
        {
            _owner.AnimationSetTrigger("NormalAttack");

            if (_owner.GetPhotonView().isMine == false) return;

            // スタン
            StatusAilmentBase stan = new StatusAilmentBase(null, KIND.STAN, 3.0f);
            ColliderManager cm = ColliderManager.GetInstance();
            cm.EntrySphereCollider(VariableCollider.COLLISION_PLAYER_ATTACK, _owner, 1.0f, _damage, 5, _owner.transform.position, 2.0f, new StatusAilmentBase[] { stan });
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// デッドリーインパクト
    /// </summary>
    public class DeadlyImpact : SkillBase
    {
        int _damage;

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 15.0f;
            _damage = 50;
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
            ColliderManager cm = ColliderManager.GetInstance();
            cm.EntrySphereCollider(VariableCollider.COLLISION_PLAYER_ATTACK, _owner, 1.0f, _damage, 5, _owner.transform.position, 2.0f, null);
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// フラッシュアサルト
    /// </summary>
    public class FlashAssault : SkillBase
    {
        int _damage;

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 12.0f;
            _damage = 39;
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
            StatusAilmentBase stan = new StatusAilmentBase(null, KIND.STAN, 1.0f);
            ColliderManager cm = ColliderManager.GetInstance();
            cm.EntrySphereCollider(VariableCollider.COLLISION_PLAYER_ATTACK, _owner, 1.0f, _damage, 5, _owner.transform.position, 2.0f, new StatusAilmentBase[] { stan });
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// ラストアテンプト
    /// </summary>
    public class LastAttempt : SkillBase
    {
        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 5.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 10.0f;
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

            _owner.CallAddStatusAilment((int)KIND.LAST_ATTEMPT, 10.0f);
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// レイジインパクト
    /// </summary>
    public class RageImpact : SkillBase
    {
        int _damage;
        CapsuleCollider _capsuleCollider;
        Vector3 _vec;

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 1.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 7.0f;
            _damage = 35;
        }

        public override SkillBase MyUpdate()
        {
            base.MyUpdate();

            if (_capsuleCollider != null &&
                _capsuleCollider.gameObject.GetActive() != false)
            {
                _capsuleCollider.transform.position = _capsuleCollider.transform.position + _vec * Time.deltaTime;
            }
            else
            {
                _capsuleCollider = null;
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
            _vec *= 10.0f;

            if (_owner.GetPhotonView().isMine == false) return;

            ColliderManager cm = ColliderManager.GetInstance();
            _capsuleCollider = cm.EntryCapsuleCollider(VariableCollider.COLLISION_PLAYER_ATTACK, _owner, 3.0f, _damage, 5,
                _owner.transform.position, 0, 3.0f, 0.2f, null);
            Vector3 f = _owner.transform.forward.normalized;
            Vector3 axis = f;
            Quaternion q = Quaternion.identity;
            q = Quaternion.Euler(_owner.transform.rotation.x, _owner.transform.rotation.y, _owner.transform.rotation.z);
            q *= Quaternion.AngleAxis(45.0f, axis);
            _capsuleCollider.transform.rotation = q;
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }


    /// <summary>
    /// インビジブルドッジ
    /// </summary>
    public class InvisibleDodge : SkillBase
    {
        float _timer;

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 0.0f;
            _timer = 0.0f;
        }

        public override SkillBase MyUpdate()
        {
            base.MyUpdate();

            // リキャストをスキップできるかの判定に使うタイマー
            if (GetState() != SKILL_STATE.ACTIVATING)
            {
                if(0.0f < _timer)
                    _timer -= Time.deltaTime;

                if (_timer <= 0.0f)
                    REQUIREMENT_RECAST_TIME = 0.0f;
            }

            return this;
        }

        public override void Cast()
        {
            // 前回の発動から5秒以内だったらリキャストが発生する
            if (0.0f < _timer)
            {
                REQUIREMENT_RECAST_TIME = 7.0f;
                _timer = 5.0f;
            }
            else
            {
                REQUIREMENT_RECAST_TIME = 0.0f;
                _timer = 5.0f;
            }

            _owner.AnimationSetTrigger("Idle");
        }

        public override void Activate()
        {
            _owner.AnimationSetTrigger("NormalAttack");
            
            if (_owner.GetPhotonView().isMine == false) return;

            _owner.CallAddStatusAilment((int)KIND.NO_DAMAGE, 2.0f);
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// サマーソルトレイヴ
    /// </summary>
    public class SummerSaltRave : SkillBase
    {
        int _damage;

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 12.0f;
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

            // サイレス
            StatusAilmentBase silence = new StatusAilmentBase(null, KIND.SILENCE, 3.0f);
            ColliderManager cm = ColliderManager.GetInstance();
            cm.EntryCapsuleCollider(VariableCollider.COLLISION_PLAYER_ATTACK, _owner, 1.0f, _damage, 5,
                _owner.transform.position, 0, 2.0f, 1.0f, new StatusAilmentBase[]{ silence });
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }
}
