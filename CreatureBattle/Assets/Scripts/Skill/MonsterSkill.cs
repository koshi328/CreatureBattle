using UnityEngine;
public class StanBlessCondition : Condition
{
    float time = 0.0f;
    public StanBlessCondition(int maxStack, float maxTime)
        : base(maxStack, maxTime)
    {
    }
    protected override void Execute(Actor actor)
    {
        if (!actor.GetPhotonView().isMine) return;
        time += Time.deltaTime;
        if (time >= 1.0f)
        {
            time = 0.0f;
            actor.TakeDamage(120);
        }
    }
}
public class InitializeWaveCondition : Condition
{
    public InitializeWaveCondition(int maxStack, float maxTime)
        : base(maxStack, maxTime)
    {
    }
    protected override void Entry(Actor actor)
    {
        base.Entry(actor);
        if (!actor.GetPhotonView().isMine) return;
        ChatController.Instance.AddMessage("イニシャライズウェーブ!防御力＆移動速度up");
    }
    protected override void Execute(Actor actor)
    {
        actor.GetCondition().ClearDebuff();
        actor.GetCondition().ReciveDamageRate *= 0.65f;
        actor.GetCondition().SpeedDownRate *= 1.5f;
    }
}
public class ExplosionCondition : Condition
{
    float time = 0.0f;
    public ExplosionCondition(int maxStack, float maxTime)
        : base(maxStack, maxTime)
    {
    }
    protected override void Execute(Actor actor)
    {
        actor.GetCondition().RecoveryRate *= 0.0f;
        if (!actor.GetPhotonView().isMine) return;
        time += Time.deltaTime;
        if (time >= 1.0f)
        {
            time = 0.0f;
            actor.TakeDamage(10);
        }
    }
}
public class DoubleEdgeCondition : Condition
{
    public DoubleEdgeCondition(int maxStack, float maxTime)
        : base(maxStack, maxTime)
    {
    }
    protected override void Exit(Actor actor)
    {

    }
}
public class LimitBreakCondition : Condition
{
    public LimitBreakCondition(int maxStack, float maxTime)
        : base(maxStack, maxTime)
    {
    }
    protected override void Entry(Actor actor)
    {
        base.Entry(actor);
        if (!actor.GetPhotonView().isMine) return;
        ChatController.Instance.AddMessage("リミットブレイク発動!攻撃力＆防御力up");
    }
    protected override void Execute(Actor actor)
    {
        _time = 1.0f;
        actor.GetCondition().ReciveDamageRate *= 0.75f;
        actor.GetCondition().GiveDamageRate *= 1.55f;
    }
}
public class StudiiProtectCondition : Condition
{
    float time = 0.0f;
    Material material;
    public StudiiProtectCondition(int maxStack, float maxTime)
        : base(maxStack, maxTime)
    {
    }
    protected override void Entry(Actor actor)
    {
        material = actor.GetEffectMaterial();
        material.SetColor("_EmissionColor", new Color(0, 0.0f, 0.0f, 1.0f));
        base.Entry(actor);
    }
    protected override void Execute(Actor actor)
    {
        material.SetColor("_EmissionColor", Color.Lerp(material.GetColor("_EmissionColor"), new Color(0.2f, 0.8f, 1.3f, 1.0f), 0.1f));
        _time = 1.0f;
    }
    protected override void Exit(Actor actor)
    {
        material.SetColor("_EmissionColor", new Color(0, 0.0f, 0.0f, 1.0f));
        base.Exit(actor);
    }
}

public class AbnormalCounterCondition : Condition
{
    public AbnormalCounterCondition(int maxStack, float maxTime)
        : base(maxStack, maxTime)
    {

    }
}
