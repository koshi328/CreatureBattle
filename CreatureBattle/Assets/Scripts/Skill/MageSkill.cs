using UnityEngine;
public class GroundFrostCondition : Condition
{
    public GroundFrostCondition(int maxStack, float maxTime)
        : base(maxStack, maxTime)
    {
    }
    protected override void Execute(Actor actor)
    {
        actor.GetCondition().SpeedDownRate *= 0.5f;
    }
}
public class MeteoImpactCondition : Condition
{
    float time = 0.0f;
    public MeteoImpactCondition(int maxStack, float maxTime)
        : base(maxStack, maxTime)
    {
    }
    protected override void Execute(Actor actor)
    {
        time += Time.deltaTime;
        if (time >= 1.0f)
        {
            time = 0.0f;
            actor.TakeDamage(6);
        }
    }
}
