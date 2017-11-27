using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusCanvas : MonoBehaviour {
    [SerializeField]
    Gauge _hpGauge;
    [SerializeField]
    Gauge _mpGauge;

    public void SetHPGauge(float value, float maxValue)
    {
        _hpGauge.SetValue(value,maxValue);
    }
    public void SetMPGauge(float value, float maxValue)
    {
        _mpGauge.SetValue(value, maxValue);
    }
}
