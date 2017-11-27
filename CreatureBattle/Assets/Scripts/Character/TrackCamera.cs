using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCamera : MonoBehaviour {
    [SerializeField]
    Vector3 _offset;
    [SerializeField]
    float _sensitivity;

    Transform _refTransform = null;
    Vector2 _cameraAngle = new Vector2(0, 0);
    Vector2 _preClickPos;
	// Update is called once per frame
	void LateUpdate () {
        if (!_refTransform) return;
        transform.position = _refTransform.position + _offset;

        if (Input.GetMouseButtonDown(1))
        {
            _preClickPos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(1))
        {
            Vector2 dir = (new Vector2(Input.mousePosition.x, Input.mousePosition.y) - _preClickPos) * _sensitivity;
            dir.y *= -1;
            _cameraAngle += dir;
            _preClickPos = Input.mousePosition;
            _cameraAngle.y = Mathf.Clamp(_cameraAngle.y, -20, 80);
            transform.rotation = Quaternion.Euler(new Vector2(_cameraAngle.y, _cameraAngle.x));
        }
	}

    public void SetTarget(Transform target)
    {
        _refTransform = target;
    }
}
