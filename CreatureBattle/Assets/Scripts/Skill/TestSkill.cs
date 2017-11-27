using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSkill : SkillBase {

    //public override void Initialize()
    //{
    //    _castTime = 0.1f;
    //    _recastTime = 0.0f;
    //}
    // 前方に素早く動く
    //public override SkillBase Execute(Actor owner)
    //{
    //    _castTime -= Time.deltaTime;
    //    if (_castTime <= 0) return null;
    //    owner.transform.Translate(Vector3.forward * 20 * Time.deltaTime);
    //    return this;
    //}

    //public override void Initialize()
    //{
    //    _castTime = 2.0f;
    //    _recastTime = 0.0f;
    //}
    //public override SkillBase Execute(Actor owner)
    //{
    //    _castTime -= Time.deltaTime;
    //    if (_castTime <= 0) return null;
    //    owner.transform.Translate(Vector3.forward * 20 * Time.deltaTime);
    //    GameObject bullet = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //    bullet.transform.position = owner.transform.position + owner.transform.forward * 2;
    //    bullet.transform.rotation = owner.transform.rotation;
    //    GameObject.Destroy(bullet, 3.0f);
    //    return this;
    //}
    GameObject collider;
    public override void Initialize()
    {
        _castTime = 0.4f;
        _recastTime = 0.0f;
    }
    public override SkillBase Execute(Actor owner)
    {
        if(_castTime >= 0.4f)
        {
            owner.AnimationSetTrigger("NormalAttack");
            collider = GameObject.Instantiate(Resources.Load("Prefabs/Collision")) as GameObject;
            collider.AddComponent<NormalCollider>().Initialize(owner, owner.transform.position, new Vector3(2, 2, 2));
        }
        _castTime -= Time.deltaTime;
        if (_castTime <= 0)
        {
            owner.CancelAction();
            owner.CallExecuteSkill((int)SKILL_ID.SWORD_ATTACK_NORMAL_2);
            GameObject.Destroy(collider);
            return null;
        }
        return this;
    }

    public override void Dispose()
    {

    }
}
public class TestSkill2 : SkillBase
{
    public override void Initialize()
    {
        _castTime = 0.4f;
        _recastTime = 0.0f;
        Debug.Log("Skill2:Initialize");
    }
    public override SkillBase Execute(Actor owner)
    {
        if (_castTime >= 0.4f)
        {
            Debug.Log("Skill2:Execute");
            owner.AnimationSetTrigger("NormalAttack");
        }
        _castTime -= Time.deltaTime;
        if (_castTime <= 0)
        {
            owner.CancelAction();
            owner.CallExecuteSkill((int)SKILL_ID.SWORD_ATTACK_NORMAL_3);
            return null;
        }
        return this;
    }

    public override void Dispose()
    {

    }
}
public class TestSkill3 : SkillBase
{
    public override void Initialize()
    {
        _castTime = 1.5f;
        _recastTime = 0.0f;
        Debug.Log("Skill3:Initialize");
    }
    public override SkillBase Execute(Actor owner)
    {
        if (_castTime >= 1.5f)
        {
            owner.AnimationSetTrigger("NormalAttack");
            Debug.Log("Skill3:Execute");
        }
        _castTime -= Time.deltaTime;
        if (_castTime <= 0)
        {
            owner.AnimationSetTrigger("Idel");
            return null;
        }
        return this;
    }

    public override void Dispose()
    {

    }
}


public class Shield : SkillBase
{
    GameObject ShieldObj;
    public override void Initialize()
    {
        _castTime = 1.5f;
        _recastTime = 0.0f;
    }
    public override SkillBase Execute(Actor owner)
    {
        if(_castTime >= 1.5f)
        {
            ShieldObj = GameObject.Instantiate(Resources.Load("Prefabs/Shield")) as GameObject;
        }
        ShieldObj.transform.position = owner.transform.position;
        _castTime -= Time.deltaTime;
        if (_castTime <= 0)
        {
            GameObject.Destroy(ShieldObj);
            return null;
        }
        return this;
    }

    public override void Dispose()
    {
        if (ShieldObj == null) return;
        GameObject.Destroy(ShieldObj);
    }
}