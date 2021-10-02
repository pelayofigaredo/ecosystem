/**
 * Accion que hace a la unidad comenzar a descansar
 */
public class ActionUnitRest : ActionUnit
{
    private void Start()
    {
        Initialice();
    }

    public override void Act()
    {
        if (!unit.isBusy)
        {
            base.Act();
            unit.Rest();
        }

    }
}
