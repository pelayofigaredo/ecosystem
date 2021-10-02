public class ActionUnitTargetPrey : ActionUnit
{
    public override void Act()
    {
        base.Act();
        unit.TargetNearestPrey();
    }
}
