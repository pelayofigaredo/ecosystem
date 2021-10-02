public class ActionUnitTargetPredator : ActionUnit
{
    public override void Act()
    {
        base.Act();
        unit.TargetNearestPredator();
    }
}
