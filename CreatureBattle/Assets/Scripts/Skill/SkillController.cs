using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SKILL_ID
{
    // 通常攻撃
    SWORD_ATTACK_NORMAL,

    // ファイターのスキル
    CONTINUOUS_ATTACK,  // 連撃
    EMERGENCY_AVOID,    // 緊急回避
    TRIP_STAN,          // トリップスタン

    ALL_NUM
}

public class SkillController
{
	static public SkillBase GetSkill(SKILL_ID skillID)
    {
        switch (skillID)
        {
            case SKILL_ID.SWORD_ATTACK_NORMAL:  return new Skill.NormalAttack();
            case SKILL_ID.CONTINUOUS_ATTACK:    return new FighterSkill.ContinuousAttack();
            case SKILL_ID.EMERGENCY_AVOID:      return new FighterSkill.EmergencyAvoid();
            case SKILL_ID.TRIP_STAN:            return new FighterSkill.TripStan();

            default:                            return new Skill.NormalAttack();
        }
    }
}
