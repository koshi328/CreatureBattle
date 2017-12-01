using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Example/Create ScriptableSkill")]
public class ScriptableSkill : ScriptableObject {

    [System.Serializable]
    public struct SkillObject
    {
        public string skillName;
        public int skillID;
    }

    public SkillObject[] data;
}

