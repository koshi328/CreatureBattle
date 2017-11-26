﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    [SerializeField]
    float _maxSpeed;
    float _speed;
    Actor _myActor;
    Camera _mainCamera;
    CommandCanvas _commandController;
    StatusCanvas _statusCanvas;

    // Use this for initialization
    void Start()
    {
       
        _myActor = GetComponent<Actor>();
        _mainCamera = Camera.main;
        _maxSpeed = 5.0f;
        _speed = 0.0f;
        // 仮処理
        if (PhotonNetwork.connected && !_myActor.GetPhotonView().isMine) return;
        _statusCanvas = GameObject.Find("StatusCanvas").GetComponent<StatusCanvas>();
        _commandController = GameObject.Find("CommandCanvas").GetComponent<CommandCanvas>();
        _commandController.SetCommand(0, () => { _myActor.CallExecuteSkill((int)SKILL_ID.SWORD_ATTACK_NORMAL_1); });
        _commandController.SetCommand(1, () => { _myActor.CallExecuteSkill((int)SKILL_ID.SWORD_ATTACK_NORMAL_1); });
        _commandController.SetCommand(2, () => { _myActor.CallExecuteSkill((int)SKILL_ID.SWORD_ATTACK_NORMAL_1); });
        _commandController.SetCommand(3, () => { _myActor.CallExecuteSkill((int)SKILL_ID.SHIELD); });
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.connected && !_myActor.GetPhotonView().isMine) return;
        _statusCanvas.SetHPGauge(_myActor._currentHP, _myActor._maxHP);
        _statusCanvas.SetMPGauge(_speed, _maxSpeed);
        // 方向指定
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 dirZ = (transform.position - _mainCamera.transform.position).normalized * z;
        Vector3 dirX = _mainCamera.transform.right * x;
        Vector3 dir = (dirZ + dirX).normalized;
        // 徐々に加速する処理
        if ((x * x) + (z * z) >= 0.1 && _myActor.CanDiscardSkill())
        {
            _speed = Mathf.Clamp(_speed + Time.deltaTime * _maxSpeed * 2, 0.0f, _maxSpeed);
            // 移動関数
            _myActor.CancelAction();
            _myActor.Movement(dir.x, dir.z, _speed);
        }
        else
        {
            _speed = 0.0f;
        }

        _myActor.AnimationSetFloat("Run", _speed);
    }
}
