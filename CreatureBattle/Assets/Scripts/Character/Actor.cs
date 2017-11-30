using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Actor : MonoBehaviour {

    protected NavMeshAgent _navMesh;
    protected int _currentSkill;
    protected Animator _myAnimator;
    protected PhotonView _myPhotonView;
    // 自分の使うスキルを全て持っておく
    protected SkillBase[] _skillList;

    // 仮
    public int _maxHP { get; set; }
    public int _currentHP { get; set; }
    


    protected void Initialize()
    {
        _maxHP = 100;_currentHP = _maxHP;
        _currentSkill = -1;
        _navMesh = GetComponent<NavMeshAgent>();
        _myAnimator = GetComponent<Animator>();
        _myPhotonView = GetComponent<PhotonView>();
    }

    public virtual void Movement(float x, float z, float speed)
    {
        Vector3 vel = new Vector3(x, 0, z).normalized;
        transform.rotation = Quaternion.LookRotation(Vector3.Lerp(vel, transform.forward, 0.4f));
        _navMesh.Move(vel * speed * Time.deltaTime);
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
        if (_skillList == null) return;
        for (int i = 0; i < 4; i++)
        {
            _skillList[i].MyUpdate();
        }
    }
    // ----------------------------------------------------------------------

    //public virtual SkillBase Action()
    //{
    //    if (_currentSkill == null) return null;
    //    SkillBase nextSkill = _currentSkill.Execute(this);
    //    if(_currentSkill != nextSkill)
    //    {
    //        _currentSkill = nextSkill;
    //        if(_currentSkill != null)
    //            _currentSkill.Initialize();
    //    }
    //    return _currentSkill;
    //}

    public void CancelAction()
    {
        if (_currentSkill < 0) return;
        _skillList[_currentSkill].Dispose();
    }

    //public void ExecuteSkill(SkillBase skill)
    //{
    //    if (_currentSkill != null && !_currentSkill.CanDiscard()) return;
    //    _currentSkill = skill;
    //    _currentSkill.Initialize();
    //}
    [PunRPC]
    public void TakeDamage(int damage)
    {
        _currentHP = Mathf.Max(_currentHP - damage, 0);
        AnimationSetTrigger("Damage");
        Debug.Log("TakeDamage=>currentHP:" + _currentHP);
    }
    public void CallTakeDamage(int damage)
    {
        _myPhotonView.RPC("TakeDamage", PhotonTargets.AllViaServer, damage);
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
        }
    }

    public SKILL_STATE GetSkillState(int n)
    {
        if (_skillList == null) return SKILL_STATE.USABLE;
        return _skillList[n].GetState();
    }

    public float GetRecastPer(int n)
    {
        if (_skillList == null) return 1.0f;
        return _skillList[n].GetRecastPer();
    }
}
