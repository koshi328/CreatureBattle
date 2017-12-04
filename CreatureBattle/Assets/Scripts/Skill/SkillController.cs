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

    ALL_NUM
}

public class SkillController
{
	static public SkillBase GetSkill(SKILL_ID skillID)
    {
        switch (skillID)
        {
            case SKILL_ID.SWORD_ATTACK_NORMAL:  return new Skill.NormalAttack();
            case SKILL_ID.CONTINUOUS_ATTACK:    return new Skill.ContinuousAttack();
            case SKILL_ID.EMERGENCY_AVOID:      return new Skill.EmergencyAvoid();

            default:                            return new Skill.NormalAttack();
        }
    }
}
