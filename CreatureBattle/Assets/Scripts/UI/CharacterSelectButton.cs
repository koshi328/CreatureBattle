using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectButton : MonoBehaviour {

    string infomation;
    Text textObj;
    int _id;
    Image _myImage;

    private void Start()
    {
        _myImage = GetComponent<Image>();
    }
    public void SetInfomation(Text textObj, string str)
    {
        this.textObj = textObj;
        infomation = str;
    }

    public void OnPointerEnter()
    {
        if (!textObj) return;
        textObj.text = infomation;
    }

    public void OnPointerExit()
    {
        if (!textObj) return;
        textObj.text = "";
    }
}
