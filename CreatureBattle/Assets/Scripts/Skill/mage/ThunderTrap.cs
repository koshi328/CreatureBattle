using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderTrap : SkillBase {
    
    public ThunderTrap()
    {
        CAST_TIME = 0.0f;
        RECAST_TIME = 10.0f;
        ACTION_TIME = 2.0f;
    }

    protected override void EntryCast(Actor actor)
    {

    }

    protected override void Casting(Actor actor)
    {

    }

    protected override void EndCast(Actor actor)
    {
        actor.GetAnimator().SetTrigger("Trap");
        if (!actor.GetPhotonView().isMine) return;
        GameObject obj = PhotonNetwork.Instantiate("ThunderTrap", actor.transform.position, Quaternion.identity, 0);
        SkillCollider col = ColliderManager.Instance.GetCollider();
        col.Initialize(actor, SkillCollider.HitTarget.Monster, 9999.0f, 9999.0f, (argActor) =>
        {
            argActor.AddCondition(ActorCondition.KIND.STAN, 2.0f, 0.0f);
            argActor.TakeDamage(38.0f);
        }, true);
        col.SetSphereCollider(actor.transform.position, 4.0f);
        obj.GetComponent<HitDestroyObject>().SetParentCollider(col);
    }

    protected override void Action(Actor actor)
    {

    }

    protected override void EndAction(Actor actor)
    {

    }

    protected override void Cancel(Actor actor)
    {

    }
}
