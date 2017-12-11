using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Example/Create ScriptableSkill")]
public class ScriptableSkill : ScriptableObject {

    [System.Serializable]
    public struct SkillObject
    {
        public int skillID;
    }

    public SkillObject[] data;
}

