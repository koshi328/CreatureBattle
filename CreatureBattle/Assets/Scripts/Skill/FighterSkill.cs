public class TempestBlowCondition : Condition
{
    public TempestBlowCondition(int maxStack, float maxTime)
        : base(maxStack, maxTime)
    {
    }
    protected override void Entry(Actor actor)
    {
        base.Entry(actor);
        if (!actor.GetPhotonView().isMine) return;
        ChatController.Instance.AddMessage("テンペストブロウ!防御力down");
    }
    protected override void Execute(Actor actor)
    {
        actor.GetCondition().ReciveDamageRate *= (1 + _stack * 0.02f);
    }
}
public class EarthDestractionCondition : Condition
{
    public EarthDestractionCondition(int maxStack, float maxTime)
        : base(maxStack, maxTime)
    {
    }
    protected override void Entry(Actor actor)
    {
        base.Entry(actor);
        if (!actor.GetPhotonView().isMine) return;
        ChatController.Instance.AddMessage("アースデストラクション!移動速度down");
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
    protected override void Exit(Actor actor)
    {
        if (!actor.GetPhotonView().isMine) return;
        if (_stack >= _maxStack) return;
        _time = _maxTime;
        actor.AddCondition(ActorCondition.KIND.DEADLY_IMPACT, 3.0f, 0.0f);
        Execute(actor);
    }
}
