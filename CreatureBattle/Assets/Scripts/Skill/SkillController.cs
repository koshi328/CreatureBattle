using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SKILL_ID
{
    SWORD_ATTACK_NORMAL,
    SHIELD,
    ALL_NUM
}

public class SkillController
{
	static public SkillBase GetSkill(SKILL_ID skillID)
    {
        switch (skillID)
        {
            case SKILL_ID.SWORD_ATTACK_NORMAL:  return new Skill.SkillSlash();
            case SKILL_ID.SHIELD:               return new Skill.SkillShield();

            default:                            return new Skill.SkillSlash();
        }
    }
}
