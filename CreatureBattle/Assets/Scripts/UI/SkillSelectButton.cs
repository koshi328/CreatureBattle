using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillSelectButton : MonoBehaviour {

    string infomation;
    Text textObj;
    int _id;
    SkillSelectWindow ssWindow;
    Button _myButton;
    Image _myImage;

    private void Start()
    {
        _myButton = GetComponent<Button>();
        _myImage = GetComponent<Image>();
    }
    public void SetInfomation(Text textObj, string str,int id,SkillSelectWindow ssWin)
    {
        this.textObj = textObj;
        infomation = str;
        _id = id;
        ssWindow = ssWin;
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

    public void OnClickButton()
    {
        if (_id == -1) return;
        if (!ssWindow.SetSkill(_id)) return;
        _myButton.enabled = false;
        _myImage.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
    }

    public void OnButtonEnable()
    {
        _myImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }
}
