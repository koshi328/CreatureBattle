using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController {

    SkillBase[] _haveSkills;
    int _currentElem;

    public SkillController()
    {
        _haveSkills = new SkillBase[4];
        _currentElem = -1;
    }

    public void Initialize(int[] elements)
    {
        for (int i = 0; i < _haveSkills.Length; i++)
        {
            _haveSkills[i] = SkillGenerator.GetSkill((SKILL_ID)elements[i]);
        }
    }

    public void Execute(Actor actor)
    {
        for (int i = 0; i < _haveSkills.Length; i++)
        {
            if (_haveSkills[i] == null) continue;
            _haveSkills[i].Execute(actor);
        }

        // スキルの中断と終了処理
        if (_currentElem != -1)
        {
            if (_haveSkills[_currentElem].NowReCasting())
            {
                _currentElem = -1;
            }
            if (actor.GetCondition().DontMove())
            {
                actor.CancelSkill();
            }
        }
    }

    public void Select(int elem, Actor actor)
    {
        if (elem >= _haveSkills.Length) return;
        if (_haveSkills[elem].NowReCasting()) return;
        _haveSkills[elem].Select(actor);
        _currentElem = elem;
    }
    public bool NowCasting()
    {
        if(_currentElem == -1) return false;
        return _haveSkills[_currentElem].NowCasting();
    }
    public bool NowAction()
    {
        if (_currentElem == -1) return false;
        return _haveSkills[_currentElem].NowAction();
    }
    public bool NowReCasting()
    {
        if (_currentElem == -1) return false;
        return _haveSkills[_currentElem].NowReCasting();
    }
    public bool NowWaiting()
    {
        return _currentElem == -1;
    }

    public SkillBase GetSkill(int elem)
    {
        return _haveSkills[elem];
    }

    public void CancelSkill(Actor actor)
    {
        if (_currentElem == -1) return;
        _haveSkills[_currentElem].Dispose(actor);
        _currentElem = -1;
    }
}
