public class TempestBlowCondition : Condition
{
    public TempestBlowCondition(int maxStack, float maxTime)
        : base(maxStack, maxTime)
    {
    }
    protected override void Execute(Actor actor)
    {
        actor.GetCondition().ReciveDamageRate *= (_stack * 1.02f);
    }
}
public class EarthDestractionCondition : Condition
{
    public EarthDestractionCondition(int maxStack, float maxTime)
        : base(maxStack, maxTime)
    {
    }
    protected override void Execute(Actor actor)
    {
        actor.GetCondition().SpeedDownRate *= 0.5f;
    }
}
public class DeadlyImpactCondition : Condition
{
    public DeadlyImpactCondition(int maxStack, float maxTime)
        : base(maxStack, maxTime)
    {
    }
    protected override void Execute(Actor actor)
    {
        actor.GetCondition().GiveDamageRate *= (_stack * 1.01f);
    }
    protected override void Exit(Actor actor)
    {
        _time = _maxTime;
        _stack++;
        Execute(actor);
    }
}
