using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Actor : MonoBehaviour {

    SkillController _skillController;
    ActorCondition _condition;
    ActorStatus _status;
    NavMeshAgent _navMesh;
    PhotonView _myPhotonView;
    Animator _myAnimator;
    Vector3 _moveDirection;
    private void Awake()
    {
        _skillController = new SkillController();
        _condition = gameObject.AddComponent<ActorCondition>();
        _navMesh = GetComponent<NavMeshAgent>();
        _myPhotonView = GetComponent<PhotonView>();
        _myAnimator = GetComponent<Animator>();
        _status = new ActorStatus();
        _condition.Initialize();

        // ステータスの設定
        ScriptableActor actorData;
        if (PhotonNetwork.playerName == "monster")
            actorData = Resources.Load("MonsterData") as ScriptableActor;
        else
            actorData = Resources.Load("ActorData") as ScriptableActor;
        object value;
        PhotonNetwork.player.CustomProperties.TryGetValue("ActorID", out value);
        int actorID = (int)value;
        _status.Initialize(
            _condition, 
            actorData.data[actorID].attackDamage,
            actorData.data[actorID].attackInterval,
            actorData.data[actorID].hp);

        _moveDirection = Vector3.zero;
    }

    public void MyUpdate()
    {
        _myAnimator.SetFloat("Run", Vector3.Distance(_moveDirection, Vector3.zero));
        _condition.Execute(this);
        _skillController.Execute(this);
        if (_condition.DontMove()) return;
        MoveMent();
    }

    public void SetMoveDirection(Vector3 dir)
    {
        _moveDirection = Vector3.Normalize(dir);
    }
    public bool CheckMovement()
    {
        float length = Vector3.Distance(_moveDirection, Vector3.zero);
        return length > 0.0f;
    }
    public SkillController GetSkillController()
    {
        return _skillController;
    }

    public ActorCondition GetCondition()
    {
        return _condition;
    }
    public Condition GetCondition(ActorCondition.KIND kind)
    {
        return _condition.GetCondition(kind);
    }
    // RPCにする------------------
    public void SkillExecute(int elem)
    {
        if (!_skillController.NowWaiting()) return;
        if (_condition.DontUseSkill()) return;
        _myPhotonView.RPC("RPCSkillExecute", PhotonTargets.AllViaServer, elem,transform.position, transform.eulerAngles.y);
    }
    [PunRPC]
    void RPCSkillExecute(int elem, Vector3 pos, float rotY)
    {
        transform.position = pos;
        transform.rotation = Quaternion.Euler(0, rotY, 0);
        _skillController.Select(elem, this);
    }
    public void SetSkills(int[] elements)
    {
        _skillController.Initialize(elements);
        //_myPhotonView.RPC("RPCSetSkills", PhotonTargets.AllBufferedViaServer, elements);
    }
    [PunRPC]
    void RPCSetSkills(int[] elements)
    {
        _skillController.Initialize(elements);
    }

    public void TakeDamage(float damage)
    {
        if (!_myPhotonView.isMine) return;
        _myPhotonView.RPC("RPCTakeDamage", PhotonTargets.AllViaServer, damage);
    }
    [PunRPC]
    void RPCTakeDamage(float damage)
    {
        _status.TakeDamage(damage);
    }

    public void ReceiveRecovery(float recovery)
    {
        if (!_myPhotonView.isMine) return;
        _myPhotonView.RPC("RPCTakeDamage", PhotonTargets.AllViaServer, recovery);
    }
    [PunRPC]
    public void RPCReceiveRecovery(float recovery)
    {
        _status.ReceiveRecovery(recovery);
    }
    public void AddCondition(ActorCondition.KIND kind, float time, float rate)
    {
        if (!_myPhotonView.isMine) return;
        _myPhotonView.RPC("RPCAddCondition", PhotonTargets.AllViaServer, (int)kind, time, rate);
    }
    [PunRPC]
    void RPCAddCondition(int kind, float time, float rate)
    {
        _condition.GetCondition((ActorCondition.KIND)kind).AddStack(time, rate, this);
    }
    // ---------------------------
    void MoveMent()
    {
        if (_skillController.NowAction()) return;
        transform.rotation = Quaternion.LookRotation(Vector3.Lerp(_moveDirection, transform.forward, 0.4f));
        _navMesh.Move(_moveDirection * _status.GetSpeed() * Time.deltaTime);
    }

    public PhotonView GetPhotonView()
    {
        return _myPhotonView;
    }
    public Animator GetAnimator()
    {
        return _myAnimator;
    }
}
