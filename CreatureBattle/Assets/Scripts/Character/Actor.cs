﻿using System.Collections;
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

        // カメラの位置の設定
        Camera.main.transform.localPosition = new Vector3(
            Camera.main.transform.localPosition.x, 
            Camera.main.transform.localPosition.y, 
            actorData.data[actorID].cameraDistance);
        TrackCamera camera = Camera.main.GetComponentInParent<TrackCamera>();
        camera.SetOffset(actorData.data[actorID].cameraOffset);
    }

    public void Update()
    {
        _condition.Execute(this);
        _skillController.Execute(this);
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
        _myPhotonView.RPC("RPCSetSkills", PhotonTargets.AllBufferedViaServer, elements[0], elements[1], elements[2], elements[3]);
    }
    [PunRPC]
    void RPCSetSkills(int e1, int e2, int e3, int e4)
    {
        int[] elements = { e1, e2, e3, e4 };
        _skillController.Initialize(elements);
    }

    public void TakeDamage(float damage)
    {
        _myPhotonView.RPC("RPCTakeDamage", PhotonTargets.AllViaServer, damage);
    }
    [PunRPC]
    void RPCTakeDamage(float damage)
    {
        _status.TakeDamage(damage);
    }

    public void ReceiveRecovery(float recovery)
    {
        _myPhotonView.RPC("RPCReceiveRecovery", PhotonTargets.AllViaServer, recovery);
    }
    [PunRPC]
    public void RPCReceiveRecovery(float recovery)
    {
        _status.ReceiveRecovery(recovery);
    }
    public void AddCondition(ActorCondition.KIND kind, float time, float rate, bool isTimeUpdate = true)
    {
        _myPhotonView.RPC("RPCAddCondition", PhotonTargets.AllViaServer, (int)kind, time, rate,isTimeUpdate);
    }
    [PunRPC]
    void RPCAddCondition(int kind, float time, float rate, bool isTimeUpdate)
    {
        _condition.GetCondition((ActorCondition.KIND)kind).AddStack(time, rate, this, isTimeUpdate);
    }

    public void CancelSkill()
    {
        _myPhotonView.RPC("RPCCancelSkill", PhotonTargets.AllViaServer);
    }
    [PunRPC]
    void RPCCancelSkill()
    {
        _skillController.CancelSkill(this);
    }
    // ---------------------------
    void MoveMent()
    {
        if (_condition.DontMove() || _skillController.NowAction() || _skillController.NowCasting())
        {
            _myAnimator.SetFloat("Run", 0.0f);
            return;
        }
        transform.rotation = Quaternion.LookRotation(Vector3.Lerp(_moveDirection, transform.forward, 0.4f));
        _navMesh.Move(_moveDirection * _status.GetSpeed() * Time.deltaTime);
        _myAnimator.SetFloat("Run", Vector3.Distance(_moveDirection, Vector3.zero));
    }

    public PhotonView GetPhotonView()
    {
        return _myPhotonView;
    }
    public Animator GetAnimator()
    {
        return _myAnimator;
    }
    public ActorStatus GetStatus()
    {
        return _status;
    }
}
