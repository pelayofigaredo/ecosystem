
public class ActionUnitModifySenses : ActionUnit
{
    public float changePercent = 1.1f;

    public override void Act()
    {
        base.Act();
        unit.ModifySenses(changePercent);
    }
}
