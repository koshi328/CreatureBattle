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
            GameObject collider = GameObject.Instantiate(Resources.Load("Prefabs/Collision")) as GameObject;
            collider.AddComponent<NormalCollider>().Initialize(owner, owner.transform.position, new Vector3(2, 2, 2));
            GameObject.Destroy(collider, 0.4f);
        }
        _castTime -= Time.deltaTime;
        if (_castTime <= 0)
        {
            return new TestSkill2();
            owner.AnimationSetTrigger("Idel");
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
        _castTime = 0.6f;
        _recastTime = 0.0f;
    }
    public override SkillBase Execute(Actor owner)
    {
        if (_castTime >= 0.6f)
        {
            owner.AnimationSetTrigger("NormalAttack");
        }
        _castTime -= Time.deltaTime;
        if (_castTime <= 0)
        {
            return new TestSkill3();
            owner.AnimationSetTrigger("Idel");
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
    }
    public override SkillBase Execute(Actor owner)
    {
        if (_castTime >= 1.5f)
        {
            owner.AnimationSetTrigger("NormalAttack");
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
