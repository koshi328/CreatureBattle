using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using antilunchbox;

public class ThunderTrap : SkillBase {
    
    public ThunderTrap()
    {
        CAST_TIME = 1.2f;
        RECAST_TIME = 10.0f;
        ACTION_TIME = 1.5f;
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
        GameObject obj = PhotonNetwork.Instantiate("ThunderTrap", actor.transform.position, Quaternion.Euler(new Vector3(160, 0, 0)), 0);
        SkillCollider col = ColliderManager.Instance.GetCollider();
        col.Initialize(actor, SkillCollider.HitTarget.Monster, 9999.0f, 9999.0f, (defActor, atkActor) =>
        {
            defActor.AddCondition(ActorCondition.KIND.STAN, 3.0f, 0.0f);
            defActor.TakeDamage(60.0f, atkActor);
        }, true);
        col.SetSphereCollider(actor.transform.position, 4.0f);
        obj.GetComponent<HitDestroyObject>().SetParentCollider(col);
		SoundManager.PlaySFX("se_011");
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
