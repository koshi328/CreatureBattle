using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatusAilment;

namespace TankSkill
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
            for (int i = 0; i < 3; i++)
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
            if (GetState() == SKILL_STATE.ACTIVATING)
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
    /// シールドバッシュ
    /// </summary>
    public class ShieldBash : SkillBase
    {
        int _damage;

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = 10.0f;
            _damage = 9;
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

            StatusAilmentBase stan = new StatusAilmentBase(null, KIND.STAN, 1.0f);
            cm.EntrySphereCollider(VariableCollider.COLLISION_PLAYER_ATTACK, _owner, 1.0f, _damage, 5, _owner.transform.position, 2.0f, new StatusAilmentBase[] { stan });
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// リザレクション
    /// </summary>
    public class Resurrection : SkillBase
    {
        int _healNum;

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 0.5f;
            REQUIREMENT_RECAST_TIME = 25.0f;
            _healNum = 173;
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
            _owner.CallTakeRecover(_healNum + Random.Range(-5, 6));
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// バトルインスパイア
    /// </summary>
    public class BattleInspire : SkillBase
    {
        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 0.5f;
            REQUIREMENT_RECAST_TIME = 22.0f;
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

            // 一定範囲内の味方のスキルクールダウンを減少
            GameObject[] go = GameObject.FindGameObjectsWithTag("Player");
            Actor[] actors = new Player[go.Length];
            for (int i = 0; i < go.Length; i++)
            {
                if(go[i].GetComponent<Player>() != null)
                actors[i] = go[i].GetComponent<Player>();
            }
            for (int i = 0; i < actors.Length; i++)
            {
                if(actors[i] != null)
                actors[i].CallSkipSkillRecast(-4.0f);
            }
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// アングリーシャウト
    /// </summary>
    public class AngryShout : SkillBase
    {
        int _healNum;

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 0.5f;
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
            _owner.CallAddStatusAilment3((int)KIND.DAMAGE_CUT, 15.0f, 0.35f);
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// アグレッシブシャウト
    /// </summary>
    public class AggressiveShout : SkillBase
    {
        int _healNum;

        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 0.5f;
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

            // 球形の当たり判定に当たった味方に攻撃バフ
            StatusAilmentBase atkUp = new StatusBuff(null, KIND.ATK_UP, 8.0f, 0.25f);
            ColliderManager cm = ColliderManager.GetInstance();
            cm.EntrySphereCollider(VariableCollider.COLLISION_MONSTER_ATTACK, _owner, 1.0f, 0, 0, _owner.transform.position, 5.0f, new StatusAilmentBase[] { atkUp });
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// アーマーブレイク
    /// </summary>
    public class ArmorBreak : SkillBase
    {
        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 0.5f;
            REQUIREMENT_RECAST_TIME = 8.0f;
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

            // 攻撃が当たった敵に被ダメ上昇と回復力ダウン効果
            StatusAilmentBase damageUp = new StatusBuff(null, KIND.DAMAGE_UP, 8.0f, 0.25f);
            StatusAilmentBase recDown = new StatusBuff(null, KIND.REC_DOWN, 8.0f, 1.0f);
            ColliderManager cm = ColliderManager.GetInstance();
            cm.EntrySphereCollider(VariableCollider.COLLISION_PLAYER_ATTACK, _owner, 1.0f, 0, 0, _owner.transform.position, 2.0f, new StatusAilmentBase[] { damageUp, recDown });
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>W
    /// ディレイウェーブ
    /// </summary>
    public class DelayWave : SkillBase
    {
        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 0.5f;
            REQUIREMENT_RECAST_TIME = 9.0f;
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

            // 球形の当たり判定に当たった敵に移動スピードダウン
            StatusAilmentBase movDown = new StatusBuff(null, KIND.MOV_DOWN, 3.0f, 0.9f);
            ColliderManager cm = ColliderManager.GetInstance();
            cm.EntrySphereCollider(VariableCollider.COLLISION_PLAYER_ATTACK, _owner, 1.0f, 0, 0, _owner.transform.position, 5.0f, new StatusAilmentBase[] { movDown });
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }

    /// <summary>
    /// ガードアグリーメント
    /// </summary>
    public class GuardAgreement : SkillBase
    {
        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 0.5f;
            REQUIREMENT_RECAST_TIME = 15.0f;
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

            // 自分の被ダメ軽減
            _owner.CallAddStatusAilment3((int)KIND.DAMAGE_CUT, 10.0f, 0.1f);

            // パーティ全体の受けたダメージを肩代わりする
            // プレイヤー勢力のキャラクターを全員取得
            GameObject[] go = GameObject.FindGameObjectsWithTag("Player");
            Player[] players = new Player[go.Length];
            for(int i = 0; i < go.Length; i++)
            {
                players[i] = go[i].GetComponent<Player>();
            }

            // プレイヤー全員にこのスキルの発動者のインスタンスIDを教える
            // 各プレイヤーの被ダメ判定の時に、代わりに発動者の被ダメージ関数を呼ぶ
            int instanceID = _owner.GetInstanceID();
            _owner.name = instanceID.ToString();
            for (int i = 0; i < go.Length; i++)
            {
                players[i].CallAddStatusAilment4((int)KIND.COVERED, 10.0f, instanceID);
            }
        }

        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }
    }
}