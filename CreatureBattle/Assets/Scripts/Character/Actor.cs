using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StatusAilment;

public class Actor : MonoBehaviour
{
    protected NavMeshAgent _navMesh;
    protected int _currentSkill;
    protected Animator _myAnimator;
    protected PhotonView _myPhotonView;

    // 自分の使うスキルを全て持っておく
    protected SkillBase[] _skillList;

    // 自分の番号
    public int _actorNumber { get; set; }

    // 仮HP
    public int _maxHP { get; set; }
    public int _currentHP { get; set; }
    
    // 与ダメージ
    public int _attackDamage { get; set; }
    public int _defaultDamage { get; set; }

    // 攻撃のインターバル
    public float _attackInterval { get; set; }
    public float _defaultInterval { get; set; }

    // 速度
    public float _maxSpeed { get; set; }
    public float _defaultSpeed { get; set; }

    // 人間かモンスターか？
    protected ACTOR_TYPE _actorType;
    
    [SerializeField]
    protected ScriptableActor _actorData;

    // 状態異常
    private List<StatusAilmentBase> _statusAilments = new List<StatusAilmentBase>();
    


    protected void Initialize()
    {
        // 自分のキャラの番号もらう
        object value;
        PhotonNetwork.player.CustomProperties.TryGetValue("ActorID", out value);

        // もらった番号で基本ステータスを拾う
        _actorNumber = (int)value;

        _maxHP = _actorData.data[(int)value].hp;
        _currentHP = _maxHP;

        _attackDamage = _actorData.data[(int)value].attackDamage;
        _defaultDamage = _actorData.data[(int)value].attackDamage;

        _attackInterval = _actorData.data[(int)value].attackInterval;
        _defaultInterval = _actorData.data[(int)value].attackInterval;

        _maxSpeed = _actorData.data[(int)value].maxSpeed;
        _defaultSpeed = _actorData.data[(int)value].maxSpeed;
        _actorType = _actorData.data[(int)value].actorType;

        // 使用中スキル
        _currentSkill = -1;

        // その他
        _navMesh = GetComponent<NavMeshAgent>();
        _myAnimator = GetComponent<Animator>();
        _myPhotonView = GetComponent<PhotonView>();
    }

    public virtual void Movement(float x, float z, float speed)
    {
        Vector3 vel = new Vector3(x, 0, z).normalized;
        transform.rotation = Quaternion.LookRotation(Vector3.Lerp(vel, transform.forward, 0.4f));
        _navMesh.Move(vel * _maxSpeed * Time.deltaTime);
    }


    // Photonでスキルを連動する実験関数--------------------------------------
    [PunRPC]
    public void ExecuteSkillPhoton(int commandNum)
    {
        _currentSkill = commandNum;
        _skillList[commandNum].Initialize(this);
        _skillList[commandNum].Execute();
    }

    // 外部からの呼び出し関数
    public void CallExecuteSkill(int commandNum)
    {
        Debug.Log(commandNum);

        // 使用中のスキルがあれば処理しない
        for(int i = 0; i < 4; i++)
        {
            if (_skillList[i].GetState() == SKILL_STATE.ACTIVATING) return;
            if (_skillList[i].GetState() == SKILL_STATE.CASTING) return;
        }
        // 使用しようとしているスキルが使用可能でなければ処理しない
        if (_skillList[commandNum].GetState() != SKILL_STATE.USABLE) return;

        _myPhotonView.RPC("ExecuteSkillPhoton", PhotonTargets.AllViaServer, commandNum);
    }

    public virtual void MyUpdate()
    {
        _attackDamage = _actorData.data[_actorNumber].attackDamage;
        _attackInterval = _actorData.data[_actorNumber].attackInterval;
        _maxSpeed = _actorData.data[_actorNumber].maxSpeed;

        if (_statusAilments != null)
        {
            // 状態異常の更新
            for (int i = 0; i < _statusAilments.Count; i++)
            {
                _statusAilments[i].Update();

                // 時間が終わっていたら
                if (_statusAilments[i]._isFinished)
                {
                    _statusAilments.RemoveAt(i);
                }
            }
        }

        // スキルの更新
        if (_skillList != null)
        {
            for (int i = 0; i < 4; i++)
            {
                _skillList[i].MyUpdate();

                if (0 <= _currentSkill)
                {
                    if (_skillList[_currentSkill].GetState() == SKILL_STATE.RECASTING)
                    {
                        _currentSkill = -1;
                    }
                }
            }
        }
    }
    // ----------------------------------------------------------------------

    /// <summary>
    /// 移動可能か？
    /// </summary>
    /// <returns></returns>
    public bool IsMovable()
    {
        if (_statusAilments != null)
        {
            for (int i = 0; i < _statusAilments.Count; i++)
            {
                if (_statusAilments[i]._kind == KIND.STAN) return false;
            }
        }

        if (!CanDiscardSkill()) return false;

        return true;
    }

    /// <summary>
    /// スキルが使えるか？
    /// </summary>
    /// <returns></returns>
    public bool IsUsableSkill()
    {
        if (_statusAilments != null)
        {
            for (int i = 0; i < _statusAilments.Count; i++)
            {
                if (_statusAilments[i]._kind == KIND.STAN) return false;
                if (_statusAilments[i]._kind == KIND.SILENCE) return false;
            }
        }

        // ボタンを押せないようにする
        for (int i = 0; i < 4; i++)
        {
            // 詠唱または発動中のスキルがある
            if (GetSkillState(i) == SKILL_STATE.CASTING) return false;
            else if (GetSkillState(i) == SKILL_STATE.ACTIVATING) return false;
        }

        return true;
    }

    public void CancelAction()
    {
        if (_currentSkill < 0) return;
        _skillList[_currentSkill].Dispose();
        _currentSkill = -1;
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        _currentHP = Mathf.Max(_currentHP - damage, 0);
        AnimationSetTrigger("Damage");
        Debug.Log("TakeDamage=>currentHP:" + _currentHP);
        // 死んだとき
        if(_currentHP <= 0)
        {
            AnimationSetBool("Death", true);
            if (!_myPhotonView.isMine) return;
            StartCoroutine(DeathMotion());
        }
    }

    IEnumerator DeathMotion()
    {
        yield return new WaitForSeconds(5.0f);
        Camera.main.transform.parent.GetComponent<TrackCamera>().SetTarget(GameObject.Find("DeathView").transform);
        ActorController con = gameObject.GetComponent<ActorController>();
        // ActorControllerの終了処理を入れる

        if (con) con.enabled = false;
    }

    public void CallTakeDamage(int damage)
    {
        _myPhotonView.RPC("TakeDamage", PhotonTargets.AllViaServer, damage);
    }


    // 状態異常追加のクラス //////////////////////////////////////////////////////////////////////////////////
    [PunRPC]
    public void AddStatusAilment(int kind, float time)
    {
        var creator = StatusAilmentCreator.GetInstance();
        var sa = creator.GetStatusAilment(this, kind, time);
        if (sa == null) return;
        _statusAilments.Add(sa);
    }

    public void CallAddStatusAilment(int kind, float time)
    {
        _myPhotonView.RPC("AddStatusAilment", PhotonTargets.AllViaServer, kind, time);
    }

    [PunRPC]
    public void AddStatusAilment2(int kind, float time, int damage, float damageInterval)
    {
        var creator = StatusAilmentCreator.GetInstance();
        var sa = creator.GetStatusAilment2(this, kind, time, damage, damageInterval);
        if (sa == null) return;
        _statusAilments.Add(sa);
    }

    public void CallAddStatusAilment2(int kind, float time, int damage, float damageInterval)
    {
        _myPhotonView.RPC("AddStatusAilment2", PhotonTargets.AllViaServer, kind, time, damage, damageInterval);
    }

    [PunRPC]
    public void AddStatusAilment3(int kind, float time, float rate)
    {
        var creator = StatusAilmentCreator.GetInstance();
        var sa = creator.GetStatusAilment3(this, kind, time, rate);
        if (sa == null) return;
        _statusAilments.Add(sa);
    }

    public void CallAddStatusAilment3(int kind, float time, float rate)
    {
        _myPhotonView.RPC("AddStatusAilment3", PhotonTargets.AllViaServer, kind, time, rate);
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////


    public bool CanDiscardSkill()
    {
        if (_currentSkill < 0) return true;
        return _skillList[_currentSkill].CanDiscard();
    }

    public PhotonView GetPhotonView()
    {
        return _myPhotonView;
    }

    public void AnimationSetFloat(string name, float value)
    {
        if (_myAnimator == null) return;
        _myAnimator.SetFloat(name, value);
    }

    public void AnimationSetTrigger(string name)
    {
        if (_myAnimator == null) return;
        _myAnimator.SetTrigger(name);
    }
    public void AnimationSetBool(string name, bool flag)
    {
        if (_myAnimator == null) return;
        _myAnimator.SetBool(name, flag);
    }

    public void CallSetSkills(int[] idList)
    {
        _myPhotonView.RPC("SetSkills", PhotonTargets.AllBufferedViaServer, idList);
    }

    [PunRPC]
    public void SetSkills(int[] idList)
    {
        _skillList = new SkillBase[idList.Length];

        for (int i = 0; i < idList.Length; i++)
        {
            _skillList[i] = SkillController.GetSkill((SKILL_ID)idList[i]);
            _skillList[i].Initialize(this);
        }
    }

    public SKILL_STATE GetSkillState(int n)
    {
        if (_skillList == null) return SKILL_STATE.RECASTING;
        return _skillList[n].GetState();
    }
    
    public bool IsPlayer()
    {
        if(_actorType == ACTOR_TYPE.PLAYER)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool HaveStatusAilment(StatusAilment.KIND kind)
    {
        for(int i = 0;i < _statusAilments.Count; i++)
        {
            if (_statusAilments[i]._kind == kind) return true;
        }

        return false;
    }

    public float GetRecastPer(int n)
    {
        if (_skillList == null) return 1.0f;
        return _skillList[n].GetRecastPer();
    }

    public int GetAttackDamage()
    {
        return _attackDamage;
    }

    public float GetAttackInterval()
    {
        return _attackInterval;
    }
}
