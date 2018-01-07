using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffIcon : MonoBehaviour {
    [SerializeField]
    Text _stackText;
    [SerializeField]
    Text _timeText;
    Condition _subject;

    public void MyUpdate()
    {
        if (_subject == null) return;
        if(_subject.GetStack() == 0)
        {
            gameObject.SetActive(false);
            return;
        }
        else
        {
            gameObject.SetActive(true);
        }
        _stackText.text = _subject.GetStack().ToString();
        _timeText.text = Mathf.Floor(_subject.GetTime()).ToString();
    }

    public void SetCondition(Condition condition)
    {
        _subject = condition;
    }
}
