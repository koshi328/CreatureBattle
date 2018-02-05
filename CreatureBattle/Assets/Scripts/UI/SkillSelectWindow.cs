using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelectWindow : MonoBehaviour {
    [SerializeField]
    SelectController selectCon;
    [SerializeField]
    ScriptableAllSkills skillsData;
    [SerializeField]
    GameObject buttonPrefab;
    [SerializeField]
    Text skillInfomationText;
    ScriptableSkill currentSkills;

    Button[] buttonList;
    int skillNum = 12;

    int currentSelectNum;
	// Use this for initialization
	void Start () {
        currentSelectNum = 0;
        buttonList = new Button[skillNum];
        for (int i = 0; i < skillNum; i++)
        {
            buttonList[i] = Instantiate(buttonPrefab, transform).GetComponent<Button>();
            buttonList[i].transform.localPosition = new Vector3((i % 4) * 150, (i / 4) * -150, 0) + new Vector3(-220, 150);
        }
	}

    public void ChangeActor(ScriptableActor actorData, int actorID)
    {
        SkillReset();
        ScriptableSkill skills = actorData.data[actorID].skillData;
        for (int i = 0; i < skills.data.Length; i++)
        {
            Button button = buttonList[i];
            button.image.sprite = skillsData.data[skills.data[i].skillID].sprite;
            button.GetComponent<SkillSelectButton>().SetInfomation(skillInfomationText, 
                skillsData.data[skills.data[i].skillID].info,
                actorData.data[actorID].skillData.data[i].skillID, this);
        }
        for (int i = skills.data.Length; i < skillNum; i++)
        {
            Button button = buttonList[i];
            button.image.sprite = null;
            button.GetComponent<SkillSelectButton>().SetInfomation(skillInfomationText, "", -1, this);
        }
    }


    public void SkillReset()
    {
        currentSelectNum = 0;
        for (int i = 0; i < 3; i++)
        {
            selectCon.SetSkill(i, -1);
        }

        for (int i = 0; i < skillNum; i++)
        {
            buttonList[i].GetComponent<SkillSelectButton>()._selected = false;
            buttonList[i].GetComponent<SkillSelectButton>().OnButtonEnable();
        }
    }

    public bool SetSkill(int id)
    {
        if (currentSelectNum >= 3) return false;
        selectCon.SetSkill(currentSelectNum, id);
        currentSelectNum++;
        return true;
    }

    public Text InfomationText()
    {
        return skillInfomationText;
    }
}
