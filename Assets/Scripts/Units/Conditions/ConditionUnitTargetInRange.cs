using UnityEngine;

/**
 * Condicion que devuelve verdadero cuando el actual objetivo de la unidad este dentro del rango dado
 */
public class ConditionUnitTargetInRange : ConditionUnit
{
	public float range = 1f;

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
        else
        {
            float distance = Vector3.Distance(unit.transform.position, t.position);
            if (distance <= range)
            {
                return true;
            }
            return false;
        }

    }
    private void OnDrawGizmosSelected()
    {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, range);

    }
}


