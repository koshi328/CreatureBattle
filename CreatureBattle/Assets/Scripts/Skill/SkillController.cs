using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SKILL_ID
{
    SWORD_ATTACK_NORMAL,
    SHIELD,
    FIREBOLT,
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
            case SKILL_ID.FIREBOLT:             return new Skill.SkillFirebolt();

            default:                            return new Skill.SkillSlash();
        }
    }
}
