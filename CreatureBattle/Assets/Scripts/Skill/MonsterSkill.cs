using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatusAilment;

namespace MonsterSkill
{
    /// <summary>
    /// ドラゴンストーム
    /// </summary>
    public class DragonStorm : SkillBase
    {
        int _damage;

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 7.0f;
            _damage = 141;
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

            // サイレンス
            StatusAilmentBase silence = new StatusAilmentBase(null, KIND.SILENCE, 5.0f);

            // 周囲にダメージ&当たった対象にサイレンスを付与
            ColliderManager cm = ColliderManager.GetInstance();
            cm.EntrySphereCollider(VariableCollider.COLLISION_MONSTER_ATTACK, _owner, 1.0f, _damage, 5,  _owner.transform.position, 5.0f, new StatusAilmentBase[]{ silence });
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// キャパシティら伊豆
    /// </summary>
    public class CapacityRise : SkillBase
    {
        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 20.0f;
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

            // 通常攻撃速度2倍
            _owner.CallAddStatusAilment3((int)KIND.ATK_SPD_UP, 10.0f, 2.0f);
            // 通常攻撃ダメージ3.1倍
            _owner.CallAddStatusAilment3((int)KIND.ATK_UP, 10.0f, 3.1f);
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// アブノーマルカウンター
    /// </summary>
    public class AbnormalCounter : SkillBase
    {
        int _damage;

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 7.0f;
            _damage = 227;
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

            // カウンタースタンを自分に付与
            _owner.CallAddStatusAilment((int)KIND.COUNTER_STAN, 3.0f);
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// スタンブレス
    /// </summary>
    public class StanBreath : SkillBase
    {
        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 30.0f;
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
            StatusAilmentBase stan = new StatusAilmentBase(null, KIND.STAN, 8.0f);

            // 炎上
            StatusAilmentBase burn = new StatusBurn(null, KIND.BURN, 8.0f, 60, 1.0f);

            // 前方扇形の当たり判定
            ColliderManager cm = ColliderManager.GetInstance();
            cm.EntryFanCollider(VariableCollider.COLLISION_MONSTER_ATTACK, _owner, 1.0f, 0, 0, _owner.transform.position, 5.0f, _owner.transform.eulerAngles, 30.0f, new StatusAilmentBase[] { stan, burn });
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// イニシャライズウェーブ
    /// </summary>
    public class InitializeWave : SkillBase
    {
        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 30.0f;
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

            // デバフ無効
            _owner.CallAddStatusAilment((int)KIND.BAN_DEBUFF, 10.0f);
            _owner.CallAddStatusAilment((int)KIND.DAMAGE_CUT, 0.65f);
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// ラティスサンダー
    /// </summary>
    public class LatisThunder : SkillBase
    {
        int _damage;
        // 格子の数
        readonly int LINE_NUM = 5;
        // 格子の隙間の幅
        readonly float LINE_INTERVAL = 10.0f;

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 18.0f;
            _damage = 374;
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

            // 格子状当たり判定
            for (int i = 0; i < LINE_NUM; i++)
            {
                // サイレス
                StatusAilmentBase silence1 = new StatusAilmentBase(null, KIND.SILENCE, 3.0f);

                // Z軸に沿った当たり判定
                CapsuleCollider c1 = cm.EntryCapsuleCollider(VariableCollider.COLLISION_MONSTER_ATTACK, _owner, 10.0f, _damage, 5,
                    new Vector3(-LINE_INTERVAL * (float)LINE_NUM / 2.0f + LINE_INTERVAL * i, 0.0f, 0.0f),
                    0, 50.0f, 0.2f, new StatusAilmentBase[] { silence1 });

                c1.transform.eulerAngles = new Vector3(90.0f, 0.0f, 0.0f);

                for (int j = 0; j < LINE_NUM; j++)
                {
                    // サイレス
                    StatusAilmentBase silence2 = new StatusAilmentBase(null, KIND.SILENCE, 3.0f);

                    // X軸に沿った当たり判定
                    CapsuleCollider c2 = cm.EntryCapsuleCollider(VariableCollider.COLLISION_MONSTER_ATTACK, _owner, 10.0f, _damage, 5,
                        new Vector3(0.0f, 0.0f, -LINE_INTERVAL * (float)LINE_NUM / 2.0f + LINE_INTERVAL * i),
                        0, 50.0f, 0.2f, new StatusAilmentBase[] { silence2 });

                    c2.transform.eulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
                }
            }

        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// エクスプロージョン
    /// </summary>
    public class Explosion : SkillBase
    {
        int _damage;
        Vector3 _vec;
        SphereCollider _sphereCollider;

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 6.0f;
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

            // 炎上
            StatusAilmentBase burn = new StatusBurn(null, KIND.BURN, 6.0f, 11, 1.0f);
            // 回復無効
            StatusAilmentBase banRec = new StatusAilmentBase(null, KIND.BAN_REC, 6.0f);

            // 玉
            ColliderManager cm = ColliderManager.GetInstance();
            _sphereCollider = cm.EntrySphereCollider(VariableCollider.COLLISION_MONSTER_ATTACK, _owner, 2.0f, _damage, 5, _owner.transform.position, 2.5f, new StatusAilmentBase[] { burn, banRec });
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// ダブルエッジレート
    /// </summary>
    public class DoubleEdgeRate : SkillBase
    {
        int _damage;
        Vector3 _vec;
        CapsuleCollider[] _capsuleCollider = new CapsuleCollider[2];
        static int _usedNum = 0;

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 10.0f;
            _damage = 60 + 10 * _usedNum;
        }

        public override SkillBase MyUpdate()
        {
            base.MyUpdate();

            for (int i = 0; i < 2; i++)
            {
                if (_capsuleCollider[i] != null &&
                    _capsuleCollider[i].gameObject.GetActive() != false)
                {
                    _capsuleCollider[i].transform.position = _capsuleCollider[i].transform.position + _vec * Time.deltaTime;
                }
                else
                {
                    _capsuleCollider[i] = null;
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

            _usedNum++;

            // プレイヤーの向いてる方向へのベクトルを取る
            _vec = _owner.transform.forward;
            _vec.y = 0.0f;
            _vec *= 10.0f;

            if (_owner.GetPhotonView().isMine == false) return;

            // 細長い当たり判定
            for (int i = 0; i < 2; i++)
            {
                ColliderManager cm = ColliderManager.GetInstance();
                _capsuleCollider[i] = cm.EntryCapsuleCollider(VariableCollider.COLLISION_MONSTER_ATTACK, _owner, 3.0f, _damage, 5,
                    _owner.transform.position, 0, 3.0f, 0.2f, null);
                Vector3 f = _owner.transform.forward.normalized;
                Vector3 axis = f;
                Quaternion q = Quaternion.identity;
                q = Quaternion.Euler(_owner.transform.rotation.x, _owner.transform.rotation.y, _owner.transform.rotation.z);
                q *= Quaternion.AngleAxis(45.0f + 270.0f * i, axis);
                _capsuleCollider[i].transform.rotation = q;
            }
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// クレンズシステム
    /// </summary>
    public class CleanseSystem : SkillBase
    {
        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
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

            // クレンズシステムを自分に付与
            _owner.CallAddStatusAilment((int)KIND.CLEANSE_SYSTEM, 5.0f);

            // 移動速度アップ
            _owner.CallAddStatusAilment3((int)KIND.MOV_UP, 5.0f, 2.0f);
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// ダークインパクト
    /// </summary>
    public class DarkImpact : SkillBase
    {
        int _damage;
        int _spearNum;
        Vector3 _vec;
        CapsuleCollider[] _capsuleCollider;

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 9.0f;
            _spearNum = 10;
            _damage = 189 / _spearNum;
            _capsuleCollider = new CapsuleCollider[_spearNum];
        }

        public override SkillBase MyUpdate()
        {
            base.MyUpdate();

            for (int i = 0; i < _spearNum; i++)
            {
                if (_capsuleCollider[i] != null &&
                    _capsuleCollider[i].gameObject.GetActive() != false)
                {
                    _capsuleCollider[i].transform.position = _capsuleCollider[i].transform.position + _vec * Time.deltaTime;
                }
                else
                {
                    _capsuleCollider[i] = null;
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
            _vec = _owner.transform.forward;
            _vec.y = 0.0f;
            _vec *= 10.0f;

            if (_owner.GetPhotonView().isMine == false) return;

            // 細長い当たり判定
            for (int i = 0; i < _spearNum; i++)
            {
                // 打ち出す場所を決める
                Vector3 pos = _owner.transform.position;
                pos += new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-0.5f, 0.5f), Random.Range(-1.0f, 1.0f));

                ColliderManager cm = ColliderManager.GetInstance();
                _capsuleCollider[i] = cm.EntryCapsuleCollider(VariableCollider.COLLISION_MONSTER_ATTACK, _owner, 3.0f, _damage, 5,
                    pos, 0, 3.0f, 0.2f, null);

                Vector3 worldForward = Vector3.forward;
                Vector3 objForward = _owner.transform.forward;
                float dot = Vector3.Dot(worldForward, objForward);
                float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
                if (objForward.x < 0) angle *= -1;
                Quaternion q = Quaternion.Euler(90, angle, 0);
                _capsuleCollider[i].transform.rotation = q;
            }
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// リミットブレイク
    /// </summary>
    public class LimitBreak : SkillBase
    {
        // 発動済みか？
        bool _isActivated;

        // 自分のコマンド番号
        int _commandNum;


        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 0.0f;
            _isActivated = false;
            _commandNum = 0;
        }

        public override SkillBase MyUpdate()
        {
            base.MyUpdate();

            if (_isActivated) return this;

            // HP2割以下で自動発動
            if(_owner._currentHP <= _owner._maxHP * 0.2f)
            {
                if(_owner.IsUsableSkill())
                {
                    _owner.CallExecuteSkill(_commandNum);
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
            Debug.Log("LimitBreak");

            _owner.AnimationSetTrigger("NormalAttack");

            // 発動済みにする
            _isActivated = true;

            // 攻撃力アップを自分に付与
            _owner.CallAddStatusAilment3((int)KIND.ATK_UP, 9999.0f, 0.55f);

            // 防御力アップを自分に付与
            _owner.CallAddStatusAilment3((int)KIND.DAMAGE_CUT, 9999.0f, 0.25f);
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }

        public override void SetCommandNum(int num)
        {
            _commandNum = num;
        }
    }

    /// <summary>
    /// スタッディプロテクト
    /// </summary>
    public class StadyProtect : SkillBase
    {
        // 自分のコマンド番号
        int _commandNum;

        // 経過時間
        float _timer;

        // 付与間隔
        readonly float USE_INTERVAL = 3.0f;


        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 0.0f;
            _commandNum = 0;
            _timer = USE_INTERVAL;
        }

        public override SkillBase MyUpdate()
        {
            base.MyUpdate();

            if (0.0f < _timer)
                _timer -= Time.deltaTime;
            if (_timer <= 0.0f)
            {
                if (!_owner.HaveStatusAilment(KIND.STADY_PROTECT))
                {
                    if (_owner.IsUsableSkill())
                    {
                        _owner.CallExecuteSkill(_commandNum);
                        _timer = USE_INTERVAL;
                    }
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
            Debug.Log("StadyProtect");

            _owner.AnimationSetTrigger("NormalAttack");

            // プロテクトを自分に付与
            _owner.CallAddStatusAilment((int)KIND.STADY_PROTECT, 9999.0f);
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }

        public override void SetCommandNum(int num)
        {
            _commandNum = num;
        }
    }
}
