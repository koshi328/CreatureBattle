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
        _commandCanvas.SetCommand(0, () => { _myActor.SkillExecute(0); _castingCanavas.SetSkillName(_myActor.GetSkillController().GetSkillName(0)); });
        _commandCanvas.SetCommand(1, () => { _myActor.SkillExecute(1); _castingCanavas.SetSkillName(_myActor.GetSkillController().GetSkillName(1)); });
        _commandCanvas.SetCommand(2, () => { _myActor.SkillExecute(2); _castingCanavas.SetSkillName(_myActor.GetSkillController().GetSkillName(2)); });

        GameObject.Find("BuffList").GetComponent<BuffIconController>().SetCondition(_myActor.GetCondition());
        _battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        // スキルの設定
        int[] skills_id = new int[3];
        for (int i = 0; i < 3; i++)
        {
            object value;
            PhotonNetwork.player.CustomProperties.TryGetValue("skill" + (i + 1).ToString(), out value);
            if (value == null) value = 0;
            skills_id[i] = (int)value;
        }
        _myActor.SetSkills(skills_id);
        for (int i = 0; i < 3; i++)
        {
            _commandCanvas.SetImage(i,skills_id[i]);
        }
    }
	
	void Update () {
        _statusCanavas.SetHPGauge(_myActor.GetStatus().GetHP(), _myActor.GetStatus().GetMaxHP());
        if (!_myActor.GetPhotonView().isMine) return;
        // 死んだとき
        DeathProcess();
        _battleManager.IsBattling();
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
            _castingCanavas.SetSkillName(_myActor.GetSkillController().GetSkillName(0));
        }
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        if(Input.GetButtonDown("Skill2"))
        {
            _myActor.SkillExecute(1);
            _castingCanavas.SetSkillName(_myActor.GetSkillController().GetSkillName(1));
        }
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        if (Input.GetButtonDown("Skill3"))
        {
            _myActor.SkillExecute(2);
            _castingCanavas.SetSkillName(_myActor.GetSkillController().GetSkillName(2));
        }
        bool visibleCasting = false;
        for (int i = 0; i < 3; i++)
        {
            SkillBase skill = _myActor.GetSkillController().GetSkill(i);
            if (skill == null) continue;

            if (skill.NowCasting())
            {
                float castedAmount = 1.0f - (skill.GetTimer() / skill.GetCastTime());
                _castingCanavas.SetFillAmount(castedAmount);
                visibleCasting = true;
            }
            if (!skill.NowReCasting())
            {
                _commandCanvas.SetFillAmount(i, 1.0f);
                continue;
            }
            float amount = 1.0f - skill.GetTimer() / skill.GetReCastTime();
            _commandCanvas.SetFillAmount(i, amount);
        }
        _castingCanavas.gameObject.SetActive(visibleCasting);
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
