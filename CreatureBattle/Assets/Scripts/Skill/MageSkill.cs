using UnityEngine;
public class GroundFrostCondition : Condition
{
    public GroundFrostCondition(int maxStack, float maxTime)
        : base(maxStack, maxTime)
    {
    }
    protected override void Entry(Actor actor)
    {
        base.Entry(actor);
        if (!actor.GetPhotonView().isMine) return;
        ChatController.Instance.AddMessage("グラウンドフロスト!移動速度down");
    }
    protected override void Execute(Actor actor)
    {
        actor.GetCondition().SpeedDownRate *= 0.5f;
    }
}
public class MeteoImpactCondition : Condition
{
    float time = 0.0f;
    ParticleSystem _effect;
    public MeteoImpactCondition(int maxStack, float maxTime)
        : base(maxStack, maxTime)
    {
        GameObject prefab = Resources.Load("Effect/ItoEffects/Burn") as GameObject;
        _effect = GameObject.Instantiate(prefab).GetComponent<ParticleSystem>();
        _effect.Stop();
    }

    protected override void Entry(Actor actor)
    {
        _effect.transform.localScale = actor.GetCenterTrans().localScale;
        _effect.Play();
    }
    protected override void Execute(Actor actor)
    {
        _effect.transform.position = actor.GetCenterTrans().position;
        if (!actor.GetPhotonView().isMine) return;
        time += Time.deltaTime;
        if (time >= 1.0f)
        {
            time = 0.0f;
            actor.TakeDamage(6);
        }
    }
    protected override void Exit(Actor actor)
    {
        _effect.Stop();
        base.Exit(actor);
    }
}
