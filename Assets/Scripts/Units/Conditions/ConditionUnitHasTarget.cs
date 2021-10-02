using UnityEngine;

/**
 * Condicion que devuelve verdadero cuando la unidad tiene un objetivo
 */
public class ConditionUnitHasTarget : ConditionUnit
{
   
    public Unit.TargetType target;
    public override bool Test()
    {
        Transform t = unit.Target;
        switch (target)
        {
            case Unit.TargetType.Danger:
                t = unit.DangerTarget;
                break;
            case Unit.TargetType.Mate:
                t = unit.MateTarget;
                break;
        }
        if (t == null)
        {
            return false;
        }
        return true;
    }
}
