using UnityEngine;


/**
 * Accion que hace a la unidad avanzar hacia su objetivo
 */
public class ActionUnitHunt : ActionUnit
{
    public bool refreshTarget = true;

    public override void Act()
    {
        base.Act();

        if (refreshTarget)
        {
             unit.TargetNearestPrey();
        }
        Transform target = unit.Target;
        if(target != null)
        {
            agent.destination = target.position;
        }
        else
        {
            Debug.Log("Unit " + transform.root.name + " triying to hunt with no target");
        }
    }
}