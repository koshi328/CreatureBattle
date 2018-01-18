using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageRenderer : MonoBehaviour {

    [SerializeField]
    GameObject _damageTextPrefab;

    [SerializeField]
    Canvas _canvas;

	void Start () {
		
	}
	
	void Update () {
        
    }

    public void Render(Vector3 pos, int damage, Color color)
    {
        pos = new Vector3(pos.x + Random.Range(-0.1f, 0.1f), pos.y + Random.Range(-0.1f, 0.1f), pos.z + Random.Range(-0.1f, 0.1f));

        Vector3 labelPos = RectTransformUtility.WorldToScreenPoint(Camera.main, pos);
        labelPos.z = 0;

        // テキストを生成
        GameObject damageTextObject = Instantiate<GameObject>(_damageTextPrefab, _canvas.transform);
        damageTextObject.GetComponent<DamageText>()._pos = pos;
        Text damageText = damageTextObject.GetComponent<Text>();
        damageText.transform.position = labelPos;
        damageText.text = damage.ToString();
        damageText.color = color;
    }
}
