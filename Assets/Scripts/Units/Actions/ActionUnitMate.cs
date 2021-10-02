
public class ActionUnitMate : ActionUnit
{
    public override void Act()
    {
        base.Act();
        EvolutionManager.instance.Reproduce(unit, unit.MateTarget.GetComponent<Unit>());
    }
}
