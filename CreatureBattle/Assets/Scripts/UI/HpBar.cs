using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour {

    [SerializeField]
    Image _currentHp;
    [SerializeField]
    Text test;
    [SerializeField]
    Actor _actor;

	void Start () {

    }

    void LateUpdate(){
        test.text = ((int)_actor.GetStatus().GetHP()).ToString();
    }
}
