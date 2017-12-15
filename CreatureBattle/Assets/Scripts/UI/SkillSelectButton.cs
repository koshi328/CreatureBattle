using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillSelectButton : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler {

    string infomation;
    Text textObj;
    public void SetInfomation(Text textObj, string str)
    {
        this.textObj = textObj;
        infomation = str;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!textObj) return;
        textObj.text = infomation;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!textObj) return;
        textObj.text = "";
    }
}
