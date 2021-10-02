
public class ActionUnitSetTarget : ActionUnit
{
    public Unit.TargetType targetTipe;

    public override void Act()
    {
        base.Act();
        switch (targetTipe)
        {
            case Unit.TargetType.Prey:
                unit.TargetNearestPrey();
                break;
            case Unit.TargetType.Danger:
                unit.TargetNearestPredator();
                break;
            case Unit.TargetType.Mate:
                unit.TargetNearestMate();
                break;
        }
    }
}
