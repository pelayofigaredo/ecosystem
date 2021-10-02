public class ConditionUnitIsTired : ConditionUnit
{
    public override bool Test()
    {
        return unit.IsTired();
    }
}
