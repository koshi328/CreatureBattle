using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gauge : MonoBehaviour {
    [SerializeField]
    Image _fill;
    [SerializeField]
    Image _backFill;
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetValue(float value, float maxValue)
    {
        float ratio = value / maxValue;
        float current = _fill.fillAmount;
        current = current - (current - ratio) * 0.3f;
        _fill.fillAmount = current;

        current = _backFill.fillAmount;
        current = current - (current - ratio) * 0.1f;
        _backFill.fillAmount = current;
    }
}
