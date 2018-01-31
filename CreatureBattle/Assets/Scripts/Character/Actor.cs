using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Actor : MonoBehaviour {

    SkillController _skillController;
    ActorCondition _condition;
    ActorStatus _status;
    NavMeshAgent _navMesh;
    PhotonView _myPhotonView;
    Animator _myAnimator;
    Vector3 _moveDirection;
    DamageRenderer _damageRenderer;
    [SerializeField]
    Image _hpBar;
    [SerializeField]
    Image _castingBar;
    [SerializeField]
    Transform _reactTrans;
    [SerializeField]
    Transform _footTrans;
    [SerializeField]
    ScriptableAllSkills _scriptableAllSkills;
    [SerializeField]
    GameObject _silenceIcon;
    [SerializeField]
    Material _effectMaterial;

    private void Awake()
    {
        _skillController = new SkillController(_scriptableAllSkills);
        _condition = gameObject.AddComponent<ActorCondition>();
        _navMesh = GetComponent<NavMeshAgent>();
        _myPhotonView = GetComponent<PhotonView>();
        _myAnimator = GetComponent<Animator>();
        _status = new ActorStatus();
        _condition.Initialize();
        _damageRenderer = GameObject.Find("DamageRenderer").GetComponent<DamageRenderer>();
        _moveDirection = Vector3.zero;
        // ステータスの設定
        _status.Initialize(_condition, 1, 1, 1, 1);
        if (!_myPhotonView.isMine) return;
        object value;
        PhotonNetwork.player.CustomProperties.TryGetValue("ActorID", out value);
        int actorID = (int)value;
        _myPhotonView.RPC("SetStatus", PhotonTargets.AllViaServer, PhotonNetwork.playerName, actorID);
    }

    private void Start()
    {
        GameObject.Find("BattleManager").GetComponent<BattleManager>().SetPlayer(this);
        _effectMaterial.SetColor("_EmissionColor", new Color(0, 0.0f, 0.0f, 1.0f));
        SetSilence(false);
    }

    public void Update()
    {
        // 死んだとき
        if(_status.GetHP() <= 0)
        {
            _skillController.CancelSkill(this);
            _myAnimator.SetBool("Death", true);
            _myAnimator.SetFloat("Run", 0.0f);
            return;
        }
        _condition.Execute(this);
        _skillController.Execute(this);
        MoveMent();
        SetCastingBarFillRate();
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
        _myPhotonView.RPC("RPCSetSkills", PhotonTargets.AllBufferedViaServer, elements);
    }
    [PunRPC]
    void RPCSetSkills(int[] elements)
    {
        _skillController.Initialize(elements);
    }

    public void TakeDamage(float damage)
    {
        _myPhotonView.RPC("RPCTakeDamage", PhotonTargets.AllViaServer, damage + Random.Range(-5, 5));
    }
    [PunRPC]
    void RPCTakeDamage(float damage)
    {
        if (_status.GetHP() <= 0) return;
        float d = _status.TakeDamage(damage);
        _damageRenderer.Render(transform.position, (int)d, Color.red);
        SetHpBarFillRate();
        if (_skillController.NowAction() || _skillController.NowCasting()) return;
        EffectManager.Instance.CreateEffect(0, _reactTrans.position);
        if (_skillController.NowAction()) return;
        _myAnimator.SetTrigger("React");
    }

    public void ReceiveRecovery(float recovery)
    {
        _myPhotonView.RPC("RPCReceiveRecovery", PhotonTargets.AllViaServer, recovery);
    }
    [PunRPC]
    public void RPCReceiveRecovery(float recovery)
    {
        if (_status.GetHP() <= 0) return;
        float r = _status.ReceiveRecovery(recovery);
        _damageRenderer.Render(transform.position, (int)r, Color.green);
        SetHpBarFillRate();
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

    [PunRPC]
    void SetStatus(string playerName, int actorID)
    {
        // ステータスの設定
        ScriptableActor actorData;
        if (playerName == "monster")
            actorData = Resources.Load("MonsterData") as ScriptableActor;
        else
            actorData = Resources.Load("ActorData") as ScriptableActor;

        _status.Initialize(
            _condition,
            actorData.data[actorID].attackDamage,
            actorData.data[actorID].attackInterval,
            actorData.data[actorID].hp,
            actorData.data[actorID].maxSpeed);

        if (!_myPhotonView.isMine) return;
        // カメラの位置の設定
        Camera.main.transform.localPosition = new Vector3(
            Camera.main.transform.localPosition.x,
            Camera.main.transform.localPosition.y,
            actorData.data[actorID].cameraDistance);
        TrackCamera camera = Camera.main.GetComponentInParent<TrackCamera>();
        camera.SetOffset(actorData.data[actorID].cameraOffset);
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

    void SetHpBarFillRate()
    {
        float rate = _status.GetHP() / _status.GetMaxHP();
        _hpBar.fillAmount = rate;
    }

    void SetCastingBarFillRate()
    {
        for (int i = 0; i < 3; i++)
        {
            SkillBase skill = _skillController.GetSkill(i);

            if (skill == null) continue;
            if (skill.NowCasting())
            {
                float rate = 1.0f - (skill.GetTimer() / skill.GetCastTime());
                _castingBar.fillAmount = rate;
                _castingBar.gameObject.SetActive(true);
                return;
            }
        }

        _castingBar.gameObject.SetActive(false);
    }

    public Transform GetCenterTrans()
    {
        return _reactTrans;
    }
    public Transform GetFootTrans()
    {
        return _footTrans;
    }
    public void SetSilence(bool isOn)
    {
        _silenceIcon.SetActive(isOn);
    }
    public Material GetEffectMaterial()
    {
        return _effectMaterial;
    }
}
