using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 共通スキル
/// キャラクター毎にエフェクトとか違うかもなのであとで区別するかも
/// </summary>
namespace Skill
{
    /// <summary>
    /// 通常攻撃
    /// </summary>
    public class NormalAttack : SkillBase
    {
        // ダメージ
        int _damage;


        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="owner"></param>
        public override void Initialize(Actor owner)
        {
            base.Initialize(owner);
            REQUIREMENT_CAST_TIME = 0.0f;
            REQUIREMENT_ACTIVATION_TIME = 1.0f;
            REQUIREMENT_RECAST_TIME = _owner.GetAttackInterval();
            _damage = _owner.GetAttackDamage();
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        public override SkillBase MyUpdate()
        {
            base.MyUpdate();
            return this;
        }


        /// <summary>
        /// 詠唱開始の瞬間に一度だけ呼ばれる
        /// </summary>
        public override void Cast()
        {
            _owner.AnimationSetTrigger("Idle");
        }


        /// <summary>
        /// 発動の瞬間に一度だけ呼ばれる
        /// </summary>
        public override void Activate()
        {
            // 攻撃モーションを取る
            _owner.AnimationSetTrigger("NormalAttack");

            // 以下は自分のキャラでなければ行わない
            if (_owner.GetPhotonView().isMine == false) return;

            // コライダー管理クラスを取得
            ColliderManager cm = ColliderManager.GetInstance();

            // PhysicsLayerNameを決める
            int collisionLayer = 0;
            if(_owner.IsPlayer())
            {
                collisionLayer = VariableCollider.COLLISION_PLAYER_ATTACK;
            }
            else
            {
                collisionLayer = VariableCollider.COLLISION_MONSTER_ATTACK;
            }

            // コライダーをエントリー
            VariableCollider vc = cm.Entry(0.5f, 1.0f, 1, _owner.transform.position, _owner, collisionLayer, 1.0f);
            vc.SetDelegate(Callback);
        }


        /// <summary>
        /// スキルの発動が終了した瞬間に一度だけ呼ばれる
        /// </summary>
        public override void Dispose()
        {
            Initialize(_owner);
            _owner.AnimationSetTrigger("Idle");
        }


        /// <summary>
        /// 衝突した時のコールバック
        /// </summary>
        /// <param name="actor"></param>
        public void Callback(Actor actor)
        {
            // ダメージを与える
            actor.CallTakeDamage(_damage);
        }
    }
}