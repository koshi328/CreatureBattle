using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusCanvas : MonoBehaviour {
    [SerializeField]
    Gauge _hpGauge;

    public void SetHPGauge(float value, float maxValue)
    {
        _hpGauge.SetValue(value,maxValue);
    }
}
