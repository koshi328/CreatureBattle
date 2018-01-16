﻿using System.Collections;
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
        transform.position += ((_refTransform.position + _offset) - transform.position) * 0.7f;

        if (Input.GetMouseButtonDown(1))
        {
            _preClickPos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(1))
        {
            Vector2 mouseDir = (new Vector2(Input.mousePosition.x, Input.mousePosition.y) - _preClickPos) * 0.5f;
            mouseDir.y *= -1;
            _cameraAngle += mouseDir;
            _preClickPos = Input.mousePosition;
            _cameraAngle.y = Mathf.Clamp(_cameraAngle.y, -20, 80);
            transform.rotation = Quaternion.Euler(new Vector2(_cameraAngle.y, _cameraAngle.x));
        }

        float horizontal = Input.GetAxis("RightHorizontal");
        float vertical = Input.GetAxis("RightVertical");

        if (horizontal == 0.0f)
            if (vertical == 0.0f) return;

        Vector2 dir = new Vector2(horizontal, vertical) * _sensitivity;
        dir.y *= -1;
        _cameraAngle += dir;
        _preClickPos = Input.mousePosition;
        _cameraAngle.y = Mathf.Clamp(_cameraAngle.y, -20, 80);
        transform.rotation = Quaternion.Euler(new Vector2(_cameraAngle.y, _cameraAngle.x));
    }

    public void SetTarget(Transform target)
    {
        _refTransform = target;
    }

    public void SetOffset(Vector3 vec)
    {
        _offset = vec;
    }
}
