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
        time += Time.deltaTime;
        if (time >= 1.0f)
        {
            time = 0.0f;
            actor.TakeDamage(8);
        }
    }
}
public class InitializeWaveCondition : Condition
{
    public InitializeWaveCondition(int maxStack, float maxTime)
        : base(maxStack, maxTime)
    {
    }
    protected override void Execute(Actor actor)
    {
        actor.GetCondition().ClearDebuff();
        actor.GetCondition().ReciveDamageRate *= 0.65f;
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
        time += Time.deltaTime;
        if (time >= 1.0f)
        {
            time = 0.0f;
            actor.TakeDamage(11);
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
    protected override void Execute(Actor actor)
    {
        actor.GetCondition().ReciveDamageRate *= 0.25f;
        actor.GetCondition().GiveDamageRate *= 1.55f;
    }
}
public class StudiiProtectCondition : Condition
{
    float time = 0.0f;
    public StudiiProtectCondition(int maxStack, float maxTime)
        : base(maxStack, maxTime)
    {
    }
    protected override void Execute(Actor actor)
    {
        time += Time.deltaTime;
        if (time >= 3.0f)
        {
            time = 0.0f;
        }
    }
}