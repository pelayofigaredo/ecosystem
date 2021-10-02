using UnityEngine;

public class ActionUnitGoToMate : ActionUnit
{
    public bool refreshTarget = true;

    public override void Act()
    {
        base.Act();

        if (refreshTarget)
        {
            unit.TargetNearestMate();
        }
        Transform target = unit.MateTarget;
        if (target != null)
        {
            agent.destination = target.position;
        }
        else
        {
            Debug.Log("Unit " + transform.root.name + " triying to mate with no target");
        }
    }
}
