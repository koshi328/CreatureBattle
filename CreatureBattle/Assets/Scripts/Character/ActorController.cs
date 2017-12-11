using System.Collections;
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

        // 使うスキルを予め伝える
        object value = null;
        int[] skills = new int[4];
        for (int i = 0; i < 3; i++)
        {
            PhotonNetwork.player.CustomProperties.TryGetValue("skill" + (i + 1).ToString(), out value);
            skills[i] = (int)value;
        }
        _myActor.CallSetSkills(skills);

        _commandController.SetCommand(0, () => { _myActor.CallExecuteSkill(0); });
        _commandController.SetCommand(1, () => { _myActor.CallExecuteSkill(1); });
        _commandController.SetCommand(2, () => { _myActor.CallExecuteSkill(2); });
        _commandController.SetCommand(3, () => { _myActor.CallExecuteSkill(3); });
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.connected && !_myActor.GetPhotonView().isMine) return;

        // UIの更新
        _statusCanvas.SetHPGauge((float)_myActor._currentHP, (float)_myActor._maxHP);
        _statusCanvas.SetMPGauge(_speed, _maxSpeed);
        UpdateRecastTime();
        
        // 使用中のスキルがあり、そのスキルが中断不可能なら処理しない
        if (!_myActor.IsMovable()) return;

        // 移動
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

    /// <summary>
    /// スキルのUIを更新
    /// </summary>
    void UpdateRecastTime()
    {
        // リキャストタイムをUIに反映する
        for (int i = 0; i < 4; i++)
        {
            if (_myActor.GetSkillState(i) == SKILL_STATE.RECASTING)
            {
                _commandController.SetFillAmount(i, _myActor.GetRecastPer(i));
            }
            else
            {
                _commandController.SetFillAmount(i, 1.0f);
            }
        }

        for (int i = 0; i < 4; i++)
        {
            _commandController.SetInteractable(i, _myActor.IsUsableSkill());
        }
    }
}
