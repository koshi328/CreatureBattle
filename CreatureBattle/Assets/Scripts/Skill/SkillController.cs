using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SKILL_ID
{
    SWORD_ATTACK_NORMAL,
    SHIELD,
    ALL_NUM
}

public class SkillController {

    static SkillBase[] skill_clone = new SkillBase[(int)SKILL_ID.ALL_NUM];

    static public void Initialize()
    {
        skill_clone[(int)SKILL_ID.SWORD_ATTACK_NORMAL] = new TestSkill();
        skill_clone[(int)SKILL_ID.SHIELD] = new Shield();
    }

	static public SkillBase GetSkill(SKILL_ID skillID)
    {
        return skill_clone[(int)skillID].Clone();
    }
}
