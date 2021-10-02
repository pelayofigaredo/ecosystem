
public class ConditionUnitCanReproduce : ConditionUnit
{
    public override bool Test()
    {
        return unit.CanReproduce();
    }
}
