using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatusAilment;
using System;

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

            StatusAilmentBase movDown = new StatusBuff(null, KIND.MOV_DOWN, 3.0f, 0.2f);
            ColliderManager cm = ColliderManager.GetInstance();
            _sphereCollider = cm.EntrySphereCollider(VariableCollider.COLLISION_PLAYER_ATTACK, _owner, 5.0f, _damage, 5, _owner.transform.position, 1.0f, new StatusAilmentBase[] { movDown });
            _sphereCollider.transform.position = _owner.transform.position;
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// マジックインパルス
    /// </summary>
    public class MagicImpulse : SkillBase
    {
        int _damage;
        Vector3 _vec;
        SphereCollider _sphereCollider;

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 2.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 12.0f;
            _damage = 60;
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

            ColliderManager cm = ColliderManager.GetInstance();
            _sphereCollider = cm.EntrySphereCollider(VariableCollider.COLLISION_PLAYER_ATTACK, _owner, 5.0f, _damage, 5, _owner.transform.position, 3.0f, null);
            _sphereCollider.transform.position = _owner.transform.position;
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// グラウンドフロスト
    /// </summary>
    public class GroundFrost : SkillBase
    {
        int _damage;
        Vector3 _vec;
        SphereCollider _fanCollider;

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 2.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 10.0f;
            _damage = 60;
            _fanCollider = null;
        }

        public override SkillBase MyUpdate()
        {
            base.MyUpdate();

            if (_fanCollider != null &&
                _fanCollider.gameObject.GetActive() == false)
            {
                _fanCollider = null;
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
            EffectManager.Instance.IceBlessEffect(_owner.transform.position + Vector3.up,_owner.transform.rotation,REQUIREMENT_CAST_TIME);

            if (_owner.GetPhotonView().isMine == false) return;

            StatusAilmentBase movDown = new StatusBuff(null, KIND.MOV_DOWN, 2.0f, 0.5f);
            ColliderManager cm = ColliderManager.GetInstance();
            _fanCollider = cm.EntryFanCollider(VariableCollider.COLLISION_PLAYER_ATTACK, _owner, 5.0f, _damage, 5,  _owner.transform.position, 3.0f, _owner.transform.eulerAngles, 30.0f, new StatusAilmentBase[] { movDown });
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// メテオインパクト
    /// </summary>
    public class MeteorImpact : SkillBase
    {
        int _damage;
        Vector3 _vec;
        SphereCollider _sphereCollider;
        Vector3 _targetPos;

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 1.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 12.0f;
            _damage = 54;
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
            _vec *= 5.0f;
            _vec.y = -5.0f;

            if (_owner.GetPhotonView().isMine == false) return;

            StatusAilmentBase burn = new StatusBurn(null, KIND.BURN, 5.0f, 6, 1.0f);
            ColliderManager cm = ColliderManager.GetInstance();
            _sphereCollider = cm.EntrySphereCollider(VariableCollider.COLLISION_PLAYER_ATTACK, _owner, 5.0f, _damage, 5, _owner.transform.position + new Vector3(0, 10, 0), 3.0f, new StatusAilmentBase[] { burn });
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// アイスブラスト
    /// </summary>
    public class IceBrust : SkillBase
    {
        Vector3 _generatePos;
        int _damage;
        float _damageInterval;
        float _damageTimer;
        int _brustNum;
        

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 2.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 20.0f;

            _damage = 10;
            _damageInterval = 1.0f;
            _brustNum = 0;
        }

        public override SkillBase MyUpdate()
        {
            base.MyUpdate();

            // 氷の嵐を生成
            if(0 < _brustNum)
            {
                _damageTimer -= Time.deltaTime;
                if(_damageTimer < 0.0f)
                {
                    GenerateBrust();
                    _damageTimer = 1.0f;
                    _brustNum--;
                }
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
            _generatePos = _owner.transform.forward;
            _generatePos.y = 0.0f;
            _generatePos *= 4.0f;
            _generatePos += _owner.transform.position;
            EffectManager.Instance.IceTornadoEffect(_generatePos, 8.0f);
            if (_owner.GetPhotonView().isMine == false) return;
            _brustNum = 8;
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }

        private void GenerateBrust()
        {
            StatusAilmentBase movDown = new StatusBuff(null, KIND.MOV_DOWN_DUP, 8.0f, 0.1f);
            ColliderManager cm = ColliderManager.GetInstance();
            cm.EntryCapsuleCollider(VariableCollider.COLLISION_PLAYER_ATTACK, _owner, 1.0f, _damage, 0, _generatePos, 0, 10.0f, 2.0f, new StatusAilmentBase[] { movDown });
        }
    }

    /// <summary>
    /// サンダーフォール
    /// </summary>
    public class ThunderFall : SkillBase
    {
        int _damage;
        Vector3 _targetPos;

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 1.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 6.0f;
            _damage = 42;
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

            // プレイヤーの向いてる方向へのベクトルを取る
            Vector3 vec = _owner.transform.forward;
            vec *= 5.0f;
            vec.y = 0.0f;

            if (_owner.GetPhotonView().isMine == false) return;

            Debug.Log("ThunderFall");

            // 細長いカプセルコライダーを生成
            ColliderManager cm = ColliderManager.GetInstance();
            cm.EntryCapsuleCollider(VariableCollider.COLLISION_PLAYER_ATTACK, _owner, 1.0f, _damage,  5,
                _owner.transform.position + vec, 0, 5.0f, 0.2f, null);
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// フレイムピラー
    /// </summary>
    public class FlamePillar : SkillBase
    {
        int _damage;

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 1.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 7.0f;
            _damage = 34;
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

            // 細長いカプセルコライダーを生成
            for(int i = 0; i < 8; i++)
            {
                // 角度
                float deg = 360.0f / 8.0f * i;
                float rad = deg * Mathf.Deg2Rad;

                // 円状に炎上柱を発生する
                Vector3 pos = _owner.transform.position;
                pos += new Vector3(3.0f * Mathf.Sin(rad), 0.0f, 3.0f * Mathf.Cos(rad));
                StatusAilmentBase burn = new StatusBurn(null, KIND.BURN, 3.0f, 5, 1.0f);
                ColliderManager cm = ColliderManager.GetInstance();
                cm.EntryCapsuleCollider(VariableCollider.COLLISION_PLAYER_ATTACK, _owner, 1.0f, _damage, 5,
                    pos, 0, 5.0f, 0.2f, new StatusAilmentBase[] { burn });
            }
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// チェイスフレイム
    /// </summary>
    public class ChaseFlame : SkillBase
    {
        int _damage;
        Vector3 _vec;
        SphereCollider _sphereCollider;
        static int _usedNum = 0;

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 5.0f;
            _damage = 5 + 1 * _usedNum;
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

            _usedNum++;

            // プレイヤーの向いてる方向へのベクトルを取る
            _vec = _owner.transform.forward;
            _vec.y = 0.0f;
            _vec *= 6.0f;

            if (_owner.GetPhotonView().isMine == false) return;

            ColliderManager cm = ColliderManager.GetInstance();
            _sphereCollider = cm.EntrySphereCollider(VariableCollider.COLLISION_PLAYER_ATTACK, _owner, 5.0f, _damage, 5,  _owner.transform.position, 1.0f, null);
            _sphereCollider.transform.position = _owner.transform.position;
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }
}
