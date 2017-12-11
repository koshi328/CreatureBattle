using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Example/Create AllSkills")]
public class ScriptableAllSkills : ScriptableObject
{
    [System.Serializable]
    public struct Skills
    {
        public string skillName;
        public int id;
        public Sprite sprite;
    }

    public Skills[] data;
}
