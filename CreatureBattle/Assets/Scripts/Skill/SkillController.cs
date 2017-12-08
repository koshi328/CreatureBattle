using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SKILL_ID
{
    // 通常攻撃
    SWORD_ATTACK_NORMAL,

    // ファイターのスキル
    FIGHTER_CONTINUOUS_ATTACK,  // 連撃
    FIGHTER_EMERGENCY_AVOID,    // 緊急回避
    FIGHTER_TRIP_STAN,          // トリップスタン

    // タンクのスキル
    TANK_CONTINUOUS_ATTACK,  // 連撃
    TANK_EMERGENCY_AVOID,    // 緊急回避
    TANK_SHIELD_BASH,

    // メイジのスキル
    MAGE_FROST_ENERGY,

    // モンスターのスキル
    MONSTER_CAPACITY_RISE,
    MONSTER_DRAGON_STORM,
    MONSTER_ABNORMAL_COUNTER,
    MONSTER_STAN_BREATH,

    ALL_NUM
}

public class SkillController
{
	static public SkillBase GetSkill(SKILL_ID skillID)
    {
        switch (skillID)
        {
            case SKILL_ID.SWORD_ATTACK_NORMAL:  return new Skill.NormalAttack();

            case SKILL_ID.FIGHTER_CONTINUOUS_ATTACK:    return new FighterSkill.ContinuousAttack();
            case SKILL_ID.FIGHTER_EMERGENCY_AVOID:      return new FighterSkill.EmergencyAvoid();
            case SKILL_ID.FIGHTER_TRIP_STAN:            return new FighterSkill.TripStan();

            case SKILL_ID.TANK_CONTINUOUS_ATTACK: return new TankSkill.ContinuousAttack();
            case SKILL_ID.TANK_EMERGENCY_AVOID: return new TankSkill.EmergencyAvoid();
            case SKILL_ID.TANK_SHIELD_BASH: return new TankSkill.ShieldBash();

            case SKILL_ID.MAGE_FROST_ENERGY: return new MageSkill.FrostEnergy();

            case SKILL_ID.MONSTER_CAPACITY_RISE: return new MonsterSkill.CapacityRise();
            case SKILL_ID.MONSTER_DRAGON_STORM: return new MonsterSkill.DragonStorm();
            case SKILL_ID.MONSTER_ABNORMAL_COUNTER: return new MonsterSkill.AbnormalCounter();
            case SKILL_ID.MONSTER_STAN_BREATH: return new MonsterSkill.StanBreath();

            default:                            return new Skill.NormalAttack();
        }
    }
}
