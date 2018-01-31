using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCollider : MonoBehaviour {

    public enum HitTarget
    {
        Player,
        Monster
    }


    public delegate void OnHitDelegate(Actor actor, Actor atkActor);
    OnHitDelegate _hitDelegate = null;
    OnHitDelegate _genericDelegate = null;
    Actor _owner;
    BoxCollider _boxCollider;
    CapsuleCollider _sphereCollider;

    List<HitActor> _hitActors = new List<HitActor>();
    float _hitInterval;
    bool _oneHit;
    
    // 例外処理
    float _angle;
    bool _isFan;

    static int playerMask = -1;
    static int monsterMask = -1;

    private void Awake()
    {
        if(playerMask == -1)
            playerMask = LayerMask.NameToLayer("PlayerAttack");
        if (monsterMask == -1)
            monsterMask = LayerMask.NameToLayer("MonsterAttack");
    }

    // 初期生成時
    public void Create()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _sphereCollider = GetComponent<CapsuleCollider>();
        _boxCollider.enabled = false;
        _sphereCollider.enabled = false;
    }
    public void SetGenericDelegate(OnHitDelegate genericDelegate)
    {
        _genericDelegate = genericDelegate;
    }
    // ヒット時の詳細処理を設定
    public void Initialize(Actor owner, HitTarget target, float liveTime, float hitInterval, OnHitDelegate hitDelegate = null, bool oneHit = false)
    {
        _owner = owner;
        _hitDelegate = hitDelegate;
        _hitInterval = hitInterval;
        _hitActors.Clear();
        Invoke("Finalized", liveTime);
        gameObject.SetActive(true);
        _oneHit = oneHit;

        if(target == HitTarget.Player)
        {
            gameObject.layer = monsterMask;
        }
        else
        {
            gameObject.layer = playerMask;
        }
    }
    // 形の定義
    public void SetSphereCollider(Vector3 pos, float radius)
    {
        transform.position = pos;
        transform.localScale = new Vector3(radius, radius, radius);
        _boxCollider.enabled = false;
        _sphereCollider.enabled = true;
        _isFan = false;
    }
    public void SetQubeCollider(Vector3 pos, Quaternion rot, Vector3 size)
    {
        transform.position = pos;
        transform.rotation = rot;
        transform.localScale = size;
        _boxCollider.enabled = true;
        _sphereCollider.enabled = false;
        _isFan = false;
    }
    public void SetFanCollider(Vector3 pos, float radius, Vector3 forward, float angle)
    {
        transform.position = pos;
        transform.localScale = new Vector3(radius, radius, radius);
        transform.LookAt(transform.position + forward, Vector3.up);
        _boxCollider.enabled = false;
        _sphereCollider.enabled = true;
        _isFan = true;
        _angle = angle;
    }

    private void OnTriggerEnter(Collider other)
    {
        Actor hitActor = other.gameObject.GetComponent<Actor>();
        if (hitActor == null) return;
        if (_isFan) return;
        if(CounterSkillProcess(hitActor))
        {
            return;
        }
        OnDelegate(hitActor, _owner);
        OnGenericDelegate(hitActor, _owner);
        _hitActors.Add(new HitActor(hitActor));
        if(_oneHit)
        {
            Finalized();
        }
    }
    // FanCollider用
    private void OnTriggerStay(Collider other)
    {
        if (!_isFan) return;
        Actor hitActor = other.gameObject.GetComponent<Actor>();
        if (hitActor == null) return;
        if (CounterSkillProcess(hitActor))
        {
            return;
        }
        // すでに登録してある場合
        HitActor registeredActor = _hitActors.Find(x => x.actor == hitActor );
        // Fanの範囲外
        bool inFan = InFanCollider(transform, other.transform.position);
        if (registeredActor != null)
        {
            if(!inFan)
            {
                _hitActors.Remove(registeredActor);
            }
            return;
        }
        if (!inFan) return;
        OnDelegate(hitActor, _owner);
        OnGenericDelegate(hitActor, _owner);
        _hitActors.Add(new HitActor(hitActor));
        if (_oneHit)
        {
            Finalized();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < _hitActors.Count; i++)
        {
            Actor actor = other.gameObject.GetComponent<Actor>();
            if (_hitActors[i].actor == actor)
            {
                _hitActors.RemoveAt(i);
                break;
            }
        }
    }

    public void MyUpdate()
    {
        for (int i = 0; i < _hitActors.Count; i++)
        {
            _hitActors[i].time += Time.deltaTime;
            if (_hitActors[i].time < _hitInterval) continue;
            _hitActors[i].time = 0.0f;
            OnDelegate(_hitActors[i].actor, _owner);
        }
    }

    private void OnDelegate(Actor actor, Actor atkActor)
    {
        if (_hitDelegate == null) return;
        _hitDelegate(actor, atkActor);
    }
    private void OnGenericDelegate(Actor actor, Actor atkActor)
    {
        if (_genericDelegate == null) return;
        _genericDelegate(actor, atkActor);
    }

    public void Finalized()
    {
        _hitActors.Clear();
        _hitDelegate = null;
        _genericDelegate = null;
        _hitInterval = 0.0f;
        if(gameObject.activeSelf == true)
            gameObject.SetActive(false);
    }

    bool InFanCollider(Transform trans, Vector3 target)
    {
        Vector3 targetDir = Vector3.Normalize(target - trans.position);
        float angle = Mathf.Acos(Vector3.Dot(trans.forward, Vector3.Normalize(targetDir))) * 180 / 3.14f;
        if(angle <= _angle)
        {
            return true;
        }
        return false;
    }

    bool CounterSkillProcess(Actor hitActor)
    {
        if (hitActor.GetCondition().GetCondition(ActorCondition.KIND.STUDII_PROTECT).GetStack() != 0)
        {
            OnGenericDelegate(hitActor, _owner);
            hitActor.AddCondition(ActorCondition.KIND.STUDII_PROTECT, -0.1f, 0.0f);
            Finalized();
            return true;
        }
        if (hitActor.GetCondition().GetCondition(ActorCondition.KIND.ABNORMAL_COUNTER).GetStack() != 0)
        {
            OnGenericDelegate(hitActor, _owner);
            hitActor.AddCondition(ActorCondition.KIND.ABNORMAL_COUNTER, -0.1f, 0.0f);
            _owner.TakeDamage(200);
            _owner.AddCondition(ActorCondition.KIND.STAN, 3.0f, 0.0f, false);
            Finalized();
            return true;
        }
        return false;
    }

}
class HitActor
{
    public Actor actor;
    public float time;
    public HitActor(Actor argActor)
    {
        actor = argActor;
        time = 0.0f;
    }
}