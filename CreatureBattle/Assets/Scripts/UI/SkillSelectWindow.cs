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
            buttonList[i].transform.localPosition = new Vector3((i % 4) * 150, (i / 4) * -150, 0) + new Vector3(-270, 150);
            buttonList[i].name = i.ToString();
            SkillSelectButton buttonScript = buttonList[i].gameObject.AddComponent<SkillSelectButton>();
        }
	}

    public void ChangeActor(ScriptableActor actorData, int actorID)
    {
        currentSelectNum = 0;
        selectCon.SetSkill(0, -1);
        selectCon.SetSkill(1, -1);
        selectCon.SetSkill(2, -1);
        ScriptableSkill skills = actorData.data[actorID].skillData;
        for (int i = 0; i < skills.data.Length; i++)
        {
            Button button = buttonList[i];
            button.image.sprite = skillsData.data[skills.data[i].skillID].sprite;
            button.onClick.RemoveAllListeners();
            button.name = actorData.data[actorID].skillData.data[i].skillID.ToString();
            button.onClick.AddListener(()=> 
            {
                if (currentSelectNum >= 3) return;
                selectCon.SetSkill(currentSelectNum, int.Parse(button.gameObject.name));
                currentSelectNum++;
            });
            button.GetComponent<SkillSelectButton>().SetInfomation(skillInfomationText, skillsData.data[skills.data[i].skillID].info);
        }
        for (int i = skills.data.Length; i < skillNum; i++)
        {
            Button button = buttonList[i];
            button.onClick.RemoveAllListeners();
            button.image.sprite = null;
            button.GetComponent<SkillSelectButton>().SetInfomation(skillInfomationText, "");
        }
    }


    public void SkillReset()
    {
        currentSelectNum = 0;
        for (int i = 0; i < 3; i++)
        {
            selectCon.SetSkill(i, -1);
        }
    }
}
