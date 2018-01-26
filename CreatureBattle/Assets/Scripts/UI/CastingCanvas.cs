using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastingCanvas : MonoBehaviour {

    [SerializeField]
    Text _skillName;

    [SerializeField]
    Image _castedImage;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetSkillName(string skillName)
    {
        _skillName.text = skillName;
    }

    public void SetFillAmount(float amount)
    {
        _castedImage.fillAmount = amount;
    }
}
