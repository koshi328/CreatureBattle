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

    // プレイヤーごとに異なる
    public int _playerID { get; set; }

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

        // プレイヤー毎に異なる
        _playerID = PhotonNetwork.player.ID;

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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            EffectManager.Instance.SlashEffect(transform.position + Vector3.up, Quaternion.identity);
        }
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
        // ダメージ無効状態なら処理しない
        if (HaveStatusAilment(KIND.NO_DAMAGE)) return;

        // 被ダメージアップの合計割合
        float upRateTotal = 1.0f;

        // 被ダメージアップの状態異常になっていたら計算に加える
        if (HaveStatusAilment(KIND.DAMAGE_UP))
        {
            // 被ダメージアップの状態異常
            StatusBuff[] damageUp = (StatusBuff[])GetStatusAilment(KIND.DAMAGE_UP);

            for (int i = 0; i < damageUp.Length; i++)
            {
                upRateTotal += damageUp[i]._rate;
            }
        }

        // 被ダメージカットの合計割合
        float cutRateTotal = 1.0f;

        // 被ダメージカットの状態異常になっていたら計算に加える
        if (HaveStatusAilment(KIND.DAMAGE_CUT))
        {
            // 被ダメージカットの状態異常
            StatusBuff[] damageCut = (StatusBuff[])GetStatusAilment(KIND.DAMAGE_CUT);

            for (int i = 0; i < damageCut.Length; i++)
            {
                cutRateTotal -= damageCut[i]._rate;
            }
        }

        // ダメージを計算
        damage = (int)((float)damage * upRateTotal * cutRateTotal);
        damage = Mathf.Max(0, damage);

        // カバーされている状態の場合
        if (HaveStatusAilment(KIND.COVERED))
        {
            // 一番最初に付与してきたキャラクターに肩代わりしてもらう
            StatusAilmentBase[] skill = GetStatusAilment(KIND.COVERED);
            StatusCovered covered = (StatusCovered)skill[0];
            PhotonPlayer[] playerList = PhotonNetwork.playerList;
            for (int i = 0; i < playerList.Length; i++)
            {
                if(playerList[i].ID == covered._playerID)
                {
                    //playerList[i].Get
                }
            }
        }

        if (0 <= damage)
        {
            _currentHP = Mathf.Max(_currentHP - damage, 0);
            AnimationSetTrigger("Damage");
            Debug.Log("TakeDamage=>currentHP:" + _currentHP);
        }

        EffectManager.Instance.HitEffect(transform.position + Vector3.up * 0.5f);

        // 死んだとき
        if (_currentHP <= 0)
        {
            AnimationSetBool("Death", true);
            if (!_myPhotonView.isMine) return;
            StartCoroutine(DeathMotion());
        }
    }

    public void CallTakeDamage(int damage)
    {
        _myPhotonView.RPC("TakeDamage", PhotonTargets.AllViaServer, damage);
    }

    [PunRPC]
    public void TakeRecover(int rec)
    {
        // 回復無効の場合は0
        if(HaveStatusAilment(KIND.BAN_REC))
        {
            rec = 0;
        }

        // 被回復アップの合計割合
        float upRateTotal = 1.0f;

        if (HaveStatusAilment(KIND.REC_UP))
        {
            StatusBuff[] recUp = (StatusBuff[])GetStatusAilment(KIND.REC_UP);

            for (int i = 0; i < recUp.Length; i++)
            {
                upRateTotal += recUp[i]._rate;
            }
        }

        // 被回復減少の合計割合
        float downRateTotal = 1.0f;

        if (HaveStatusAilment(KIND.DAMAGE_CUT))
        {
            StatusBuff[] recDown = (StatusBuff[])GetStatusAilment(KIND.REC_DOWN);

            for (int i = 0; i < recDown.Length; i++)
            {
                downRateTotal -= recDown[i]._rate;
            }
        }

        // 回復量を計算
        rec = (int)((float)rec * upRateTotal * downRateTotal);
        rec = Mathf.Max(0, rec);

        // HPを回復
        _currentHP = Mathf.Min(_currentHP + rec, _maxHP);
        Debug.Log("TakeRecover=>currentHP:" + _currentHP);
    }

    public void CallTakeRecover(int rec)
    {
        _myPhotonView.RPC("TakeRecover", PhotonTargets.AllViaServer, rec);
    }

    IEnumerator DeathMotion()
    {
        yield return new WaitForSeconds(5.0f);
        Camera.main.transform.parent.GetComponent<TrackCamera>().SetTarget(GameObject.Find("DeathView").transform);
        ActorController con = gameObject.GetComponent<ActorController>();
        // ActorControllerの終了処理を入れる

        if (con) con.enabled = false;
    }


    // 状態異常追加のクラス //////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// スタン等の時間と種類だけで足りる状態異常に掛かる
    /// </summary>
    /// <param name="kind"></param>
    /// <param name="time"></param>
    [PunRPC]
    public void AddStatusAilment(int kind, float time)
    {
        // デバフの場合
        if (StatusAilmentBase.IsDebuff((KIND)kind))
        {
            // 妨害スキルのみを無効化する時
            if (HaveStatusAilment(KIND.BAN_DIS) &&
                !HaveStatusAilment(KIND.BAN_DEBUFF))
            {
                if (!((KIND)kind == KIND.BURN))
                    return;
            }

            // 全状態異常を無効化
            if (HaveStatusAilment(KIND.BAN_DEBUFF)) return;
        }

        var creator = StatusAilmentCreator.GetInstance();
        var sa = creator.GetStatusAilment(this, kind, time);
        if (sa == null) return;
        _statusAilments.Add(sa);
    }
    public void CallAddStatusAilment(int kind, float time)
    {
        _myPhotonView.RPC("AddStatusAilment", PhotonTargets.AllViaServer, kind, time);
    }


    /// <summary>
    /// スリップダメージの状態異常になる
    /// </summary>
    /// <param name="kind"></param>
    /// <param name="time"></param>
    /// <param name="damage"></param>
    /// <param name="damageInterval"></param>
    [PunRPC]
    public void AddStatusAilment2(int kind, float time, int damage, float damageInterval)
    {
        // デバフの場合
        if (StatusAilmentBase.IsDebuff((KIND)kind))
        {
            // 全状態異常を無効化
            if (HaveStatusAilment(KIND.BAN_DEBUFF)) return;
        }

        var creator = StatusAilmentCreator.GetInstance();
        var sa = creator.GetStatusAilment2(this, kind, time, damage, damageInterval);
        if (sa == null) return;
        _statusAilments.Add(sa);
    }
    public void CallAddStatusAilment2(int kind, float time, int damage, float damageInterval)
    {
        _myPhotonView.RPC("AddStatusAilment2", PhotonTargets.AllViaServer, kind, time, damage, damageInterval);
    }


    /// <summary>
    /// バフかデバフに掛かる
    /// </summary>
    /// <param name="kind"></param>
    /// <param name="time"></param>
    /// <param name="rate"></param>
    [PunRPC]
    public void AddStatusAilment3(int kind, float time, float rate)
    {
        // デバフの場合
        if (StatusAilmentBase.IsDebuff((KIND)kind))
        {
            // 妨害スキルのみを無効化する時
            if (HaveStatusAilment(KIND.BAN_DIS) &&
                !HaveStatusAilment(KIND.BAN_DEBUFF))
            {
                if (!((KIND)kind == KIND.BURN))
                    return;
            }

            // 全状態異常を無効化
            if (HaveStatusAilment(KIND.BAN_DEBUFF)) return;
        }

        var creator = StatusAilmentCreator.GetInstance();
        var sa = creator.GetStatusAilment3(this, kind, time, rate);
        if (sa == null) return;
        _statusAilments.Add(sa);
    }
    public void CallAddStatusAilment3(int kind, float time, float rate)
    {
        _myPhotonView.RPC("AddStatusAilment3", PhotonTargets.AllViaServer, kind, time, rate);
    }


    /// <summary>
    /// カバーされる状態にする
    /// </summary>
    /// <param name="kind"></param>
    /// <param name="time"></param>
    /// <param name="instanceID"></param>
    [PunRPC]
    public void AddStatusAilment4(int kind, float time, int instanceID)
    {
        var creator = StatusAilmentCreator.GetInstance();
        var sa = creator.GetStatusAilment4(this, kind, time, instanceID);
        if (sa == null) return;
        _statusAilments.Add(sa);
    }

    public void CallAddStatusAilment4(int kind, float time, int instanceID)
    {
        _myPhotonView.RPC("AddStatusAilment4", PhotonTargets.AllViaServer, kind, time, instanceID);
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 状態異常を解除
    /// </summary>
    /// <param name="kind"></param>
    [PunRPC]
    public void RefreshStatusAilment(int kind)
    {
        if (kind == (int)KIND.ALL)
        {
            for (int i = 0; i < _statusAilments.Count; i++)
            {
                _statusAilments.RemoveAt(i);
            }
            return;
        }

        for (int i = 0; i < _statusAilments.Count; i++)
        {
            // バフ全消去
            if (kind == (int)KIND.BUFF)
            {
                if (KIND.BUFF < _statusAilments[i]._kind &&
                   _statusAilments[i]._kind < KIND.DEBUFF)
                {
                    _statusAilments.RemoveAt(i);
                }
            }

            // デバフ全消去
            else if (kind == (int)KIND.DEBUFF)
            {
                if (KIND.DEBUFF < _statusAilments[i]._kind)
                {
                    _statusAilments.RemoveAt(i);
                }
            }
            // 各状態異常につき消去
            else
            {
                if((int)_statusAilments[i]._kind == kind)
                {
                    _statusAilments.RemoveAt(i);
                }
            }
        }
    }
    public void CallRefreshStatusAilment(int kind)
    {
        _myPhotonView.RPC("RefreshStatusAilment", PhotonTargets.AllViaServer, kind);
    }


    [PunRPC]
    public void SkipSkillRecast(float time)
    {
        for (int i = 0; i < _skillList.Length; i++)
        {
            _skillList[i].SkipRecast(time);
        }
    }
    public void CallSkipSkillRecast(float time)
    {
        _myPhotonView.RPC("SkipSkillRecast", PhotonTargets.AllViaServer, time);
    }


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
            _skillList[i].SetCommandNum(i);
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

    /// <summary>
    /// 指定された状態異常になっているか？
    /// </summary>
    /// <param name="kind"></param>
    /// <returns></returns>
    public bool HaveStatusAilment(StatusAilment.KIND kind)
    {
        for(int i = 0;i < _statusAilments.Count; i++)
        {
            if (_statusAilments[i]._kind == kind) return true;
        }

        return false;
    }

    /// <summary>
    /// 指定された種類の状態異常をまとめて返す
    /// </summary>
    /// <param name="kind"></param>
    /// <returns></returns>
    public StatusAilmentBase[] GetStatusAilment(StatusAilment.KIND kind)
    {
        int num = 0;
        StatusAilmentBase[] returnData;

        // 指定された状態異常がいくつあるか
        for (int i = 0; i < _statusAilments.Count; i++)
        {
            if (_statusAilments[i]._kind == kind) num++;
        }

        if (num <= 0) return null;
        returnData = new StatusAilmentBase[num];
        num = 0;

        // 同じ状態異常をまとめて送る
        for (int i = 0; i < _statusAilments.Count; i++)
        {
            if (_statusAilments[i]._kind == kind)
            {
                returnData[num] = _statusAilments[i];
                num++;
            }
        }

        return returnData;
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
