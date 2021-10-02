public class ActionUnitRemoveTarget : ActionUnit
{
    public Unit.TargetType target;

    public override void Act()
    {
        base.Act();
        switch (target)
        {
            case Unit.TargetType.Prey:
                unit.Target = null;
                break;
            case Unit.TargetType.Danger:
                unit.DangerTarget = null;
                break;
            case Unit.TargetType.Mate:
                unit.MateTarget = null;
                break;
        }
    }

}
