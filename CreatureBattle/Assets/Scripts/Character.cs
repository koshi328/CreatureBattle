using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 仮でキャラクターのクラス
/// </summary>
public class Character : MonoBehaviour
{
    public SkillBase[] _skill;


    private void Awake()
    {
        for (int i = 0; i < _skill.Length; i++)
        {
            _skill[i].Init();
        }
    } 

    void Update()
    {
        for (int i = 0; i < _skill.Length; i++)
        {
            _skill[i].MyUpdate();
        }
    }

    public void UseSkill(int n)
    {
        // 詠唱中のものがあれば新しく詠唱しない
        for (int i = 0; i < _skill.Length; i++)
        {
            SkillBase skill = _skill[i];

            if (_skill[i].GetState() == SkillBase.STATE.CASTING)
            {
                Debug.Log("詠唱中のスキルがあるので使えません");
                return;
            }
        }

        _skill[n].CastStart();
    }
}
