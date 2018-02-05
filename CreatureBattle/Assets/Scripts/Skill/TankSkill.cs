public class StandGuardCondition : Condition
{
    public StandGuardCondition(int maxStack, float maxTime)
        : base(maxStack, maxTime)
    {
    }
    protected override void Entry(Actor actor)
    {
        base.Entry(actor);
    }
    protected override void Execute(Actor actor)
    {
        actor.GetCondition().ReciveDamageRate *= 0.7f;
    }
}
public class AggressiveShoutCondition : Condition
{
    public AggressiveShoutCondition(int maxStack, float maxTime)
        : base(maxStack, maxTime)
    {
    }
    protected override void Entry(Actor actor)
    {
        base.Entry(actor);
    }
    protected override void Execute(Actor actor)
    {
        actor.GetCondition().GiveDamageRate *= 1.5f;
        if(actor.GetCondition().GetCondition(ActorCondition.KIND.STAN).GetStack() != 0)
        {
            _time = 0.0f;
        }
    }
}
public class AngryShoutCondition : Condition
{
    public AngryShoutCondition(int maxStack, float maxTime)
        : base(maxStack, maxTime)
    {
    }
    protected override void Entry(Actor actor)
    {
        base.Entry(actor);
    }
    protected override void Execute(Actor actor)
    {
        actor.GetCondition().ReciveDamageRate *= 0.7f;
        actor.GetCondition().SpeedDownRate *= 1.3f;
    }
}
