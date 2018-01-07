using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffIconController : MonoBehaviour {

    BuffIcon[] _iconList;
    int _maxNum;
	void Start () {
        _maxNum = (int)ActorCondition.KIND.MAX_NUM;
        _iconList = new BuffIcon[_maxNum];
        for (int i = 0; i < _maxNum; i++)
        {
            _iconList[i] = transform.Find(i.ToString()).GetComponent<BuffIcon>();
            _iconList[i].gameObject.SetActive(false);
        }
	}
	
	void Update () {
        for (int i = 0; i < _maxNum; i++)
        {
            _iconList[i].MyUpdate();
        }
    }

    public void SetCondition(ActorCondition condition)
    {
        for (int i = 0; i < _maxNum; i++)
        {
            _iconList[i].SetCondition(condition.GetCondition((ActorCondition.KIND)i));
        }
    }
}
