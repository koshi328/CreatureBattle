using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SKILL_ID
{
    SWORD_ATTACK_NORMAL_1,
    SWORD_ATTACK_NORMAL_2,
    SWORD_ATTACK_NORMAL_3,
    SHIELD,
    ALL_NUM
}

public class SkillController {

    static SkillBase[] skill_clone = new SkillBase[(int)SKILL_ID.ALL_NUM];

    static public void Initialize()
    {
        skill_clone[0] = new TestSkill();
        skill_clone[1] = new TestSkill2();
        skill_clone[2] = new TestSkill3();
        skill_clone[3] = new Shield();
    }

	static public SkillBase GetSkill(SKILL_ID skillID)
    {
        return skill_clone[(int)skillID].Clone();
    }
}
