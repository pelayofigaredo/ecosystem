public class ConditionUnitIsHungry : ConditionUnit
{
    public override bool Test()
    {
        return unit.IsHungry();
    }
}
