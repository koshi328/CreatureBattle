using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Actor : MonoBehaviour {

    protected NavMeshAgent _navMesh;
    protected SkillBase _currentSkill;
    protected Animator _myAnimator;
    protected PhotonView _myPhotonView;

    // 仮
    public int _maxHP { get; set; }
    public int _currentHP { get; set; }
    
    protected void Initialize()
    {
        _maxHP = 100;_currentHP = _maxHP;
        _currentSkill = null;
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
    public void ExecuteSkillPhoton(int skillID)
    {
        if (_currentSkill != null && !_currentSkill.CanDiscard()) return;
        _currentSkill = SkillController.GetSkill((SKILL_ID)skillID); // IDでスキルを取得（生成？）する関数を入れる
        if (_currentSkill == null) return;
        _currentSkill.Initialize();
    }
    // 外部からの呼び出し関数
    public void CallExecuteSkill(int skillID)
    {
        _myPhotonView.RPC("ExecuteSkillPhoton", PhotonTargets.AllViaServer, skillID);
    }
    public virtual SkillBase ActionTest()
    {
        if (_currentSkill == null) return null;
        return _currentSkill = _currentSkill.Execute(this);
    }
    // ----------------------------------------------------------------------

    public virtual SkillBase Action()
    {
        if (_currentSkill == null) return null;
        SkillBase nextSkill = _currentSkill.Execute(this);
        if(_currentSkill != nextSkill)
        {
            _currentSkill = nextSkill;
            if(_currentSkill != null)
                _currentSkill.Initialize();
        }
        return _currentSkill;
    }

    public void CancelAction()
    {
        if (_currentSkill == null) return;
        _currentSkill.Dispose();
        _currentSkill = null;
    }

    public void ExecuteSkill(SkillBase skill)
    {
        if (_currentSkill != null && !_currentSkill.CanDiscard()) return;
        _currentSkill = skill;
        _currentSkill.Initialize();
    }
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
        if (_currentSkill == null) return true;
        return _currentSkill.CanDiscard();
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
}
