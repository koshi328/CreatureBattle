using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour {
    Actor _myActor;
    CommandCanvas _commandCanvas;
    StatusCanvas _statusCanavas;
    CastingCanvas _castingCanavas;
    Transform _cameraTrans;
    BattleManager _battleManager;
	void Start () {
        _myActor = GetComponent<Actor>();
        _cameraTrans = Camera.main.transform;
        _cameraTrans.parent.GetComponent<TrackCamera>().SetTarget(transform);
        _commandCanvas = GameObject.Find("CommandCanvas").GetComponent<CommandCanvas>();
        _statusCanavas = GameObject.Find("StatusCanvas").GetComponent<StatusCanvas>();
        _castingCanavas = GameObject.Find("CastingCanvas").GetComponent<CastingCanvas>();
        _commandCanvas.SetCommand(0, () => { _myActor.SkillExecute(0); });
        _commandCanvas.SetCommand(1, () => { _myActor.SkillExecute(1); });
        _commandCanvas.SetCommand(2, () => { _myActor.SkillExecute(2); });
        _commandCanvas.SetCommand(3, () => { _myActor.SkillExecute(3); });

        GameObject.Find("BuffList").GetComponent<BuffIconController>().SetCondition(_myActor.GetCondition());
        _battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        // スキルの設定
        int[] skills_id = new int[4];
        for (int i = 0; i < 4; i++)
        {
            object value;
            PhotonNetwork.player.CustomProperties.TryGetValue("skill" + (i + 1).ToString(), out value);
            if (value == null) value = 0;
            skills_id[i] = (int)value;
        }
        _myActor.SetSkills(skills_id);
        for (int i = 0; i < 4; i++)
        {
            _commandCanvas.SetImage(i,skills_id[i]);
        }
    }
	
	void Update () {
        _statusCanavas.SetHPGauge(_myActor.GetStatus().GetHP(), _myActor.GetStatus().GetMaxHP());
        if (!_myActor.GetPhotonView().isMine) return;
        if (!_battleManager.IsBattling()) return;
        if (_myActor.GetStatus().GetHP() <= 0) return;
        // 移動
        float x = Input.GetAxis("LeftHorizontal");
        float z = Input.GetAxis("LeftVertical");
        Vector3 dirZ = (transform.position - _cameraTrans.transform.position).normalized * z;
        Vector3 dirX = _cameraTrans.transform.right * x;
        Vector3 dir = (dirZ + dirX).normalized;
        _myActor.SetMoveDirection(new Vector3(dir.x, 0, dir.z));
        // スキルの実行テスト
        //if(Input.GetKeyDown(KeyCode.Alpha1))
        if(Input.GetButtonDown("Skill1"))
        {
                _myActor.SkillExecute(0);
        }
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        if(Input.GetButtonDown("Skill2"))
        {
                _myActor.SkillExecute(1);
        }
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        if(Input.GetButtonDown("Skill3"))
        {
            _myActor.SkillExecute(2);
        }
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        if(Input.GetButtonDown("Skill4"))
        {
            _myActor.SkillExecute(3);
        }
            for (int i = 0; i < 4; i++)
        {
            SkillBase skill = _myActor.GetSkillController().GetSkill(i);
            if (skill == null) continue;
            if (!skill.NowReCasting())
            {
                _commandCanvas.SetFillAmount(i, 1.0f);
                continue;
            }
            float amount = 1.0f - skill.GetTimer() / skill.GetReCastTime();
            _commandCanvas.SetFillAmount(i, amount);
        }

        // 死んだとき
        DeathProcess();
    }

    void DeathProcess()
    {
        if (_myActor.GetStatus().GetHP() > 0) return;
        StartCoroutine(DeathWait());
    }

    IEnumerator DeathWait()
    {
        yield return new WaitForSeconds(4.0f);
        GameObject deathView = GameObject.Find("DeathView");
        _cameraTrans.parent.GetComponent<TrackCamera>().SetTarget(deathView.transform);
    }
}
