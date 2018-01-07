using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour {
    Actor _myActor;
    CommandCanvas _commandCanvas;
    Transform _cameraTrans;
	void Start () {
        _myActor = GetComponent<Actor>();
        _cameraTrans = Camera.main.transform;
        _cameraTrans.parent.GetComponent<TrackCamera>().SetTarget(transform);
        _commandCanvas = GameObject.Find("CommandCanvas").GetComponent<CommandCanvas>();
        _commandCanvas.SetCommand(0, () => { _myActor.SkillExecute(0); });
        _commandCanvas.SetCommand(1, () => { _myActor.SkillExecute(1); });
        _commandCanvas.SetCommand(2, () => { _myActor.SkillExecute(2); });
        _commandCanvas.SetCommand(3, () => { _myActor.SkillExecute(3); });

        GameObject.Find("BuffList").GetComponent<BuffIconController>().SetCondition(_myActor.GetCondition());

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
	}
	
	void Update () {
        _myActor.MyUpdate();
        if (!_myActor.GetPhotonView().isMine) return;
        // 移動
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 dirZ = (transform.position - _cameraTrans.transform.position).normalized * z;
        Vector3 dirX = _cameraTrans.transform.right * x;
        Vector3 dir = (dirZ + dirX).normalized;
        _myActor.SetMoveDirection(new Vector3(dir.x, 0, dir.z));
        // スキルの実行テスト
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            _myActor.SkillExecute(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _myActor.SkillExecute(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _myActor.SkillExecute(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _myActor.SkillExecute(3);
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _myActor.GetCondition().GetCondition((ActorCondition.KIND)0).AddStack(5.0f, 1.0f, _myActor);
        }
    }
}
