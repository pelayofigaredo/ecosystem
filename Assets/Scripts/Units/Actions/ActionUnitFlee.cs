
using UnityEngine;


/**
 * Accion que hace a la unidad moverse en direccion cotraria a su objetivo
 */
public class ActionUnitFlee : ActionUnit
{
    public bool refreshTarget = true;

    public override void Act()
    {
        if (!unit.isBusy)
        {
            base.Act();

            if (refreshTarget)
            {
                unit.TargetNearestPredator();
            }
            Transform target = unit.DangerTarget;
            if(target != null)
            {
                Vector3 direction = unit.transform.position - target.position;
                Vector3 newPos = unit.transform.position + direction;
                agent.SetDestination(newPos);
            }
            else
            {
                Debug.Log("Unit " + transform.root.name + " triying to flee with no target");
            }

        }
    }
}
