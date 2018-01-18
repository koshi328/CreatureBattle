using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour {

    [SerializeField]
    float _timer;

    [SerializeField]
    float EXIST_TIME;

    Text _text;

    public Vector3 _pos { get; set; }


	void Start ()
    {
        _timer = EXIST_TIME;
        _text = GetComponent<Text>();
    }
	
	void Update ()
    {
        // 徐々に上昇する
        _pos = new Vector3(_pos.x, _pos.y + 0.01f, _pos.z);

        Vector3 labelPos = RectTransformUtility.WorldToScreenPoint(Camera.main, _pos);
        labelPos.z = 0;
        this._text.rectTransform.position = labelPos;


        _timer -= Time.deltaTime;
        float rate = _timer / EXIST_TIME;

        // テキストを徐々に透明にする
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, rate);

        if (_timer < 0.0f)
        {
            Destroy(this.gameObject);
        }
	}
}
