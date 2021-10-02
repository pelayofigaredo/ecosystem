using UnityEngine;


/**
 * Accion que elimina al objetivo de su unidad
 */
public class ActionUnitEatTarget : ActionUnit
{
    public override void Act()
    {
        base.Act();
        Transform target = unit.Target;
        Food victim = target.GetComponent<Food>();
        if(victim != null)
        {
            unit.Target = null;
            unit.Eat(victim);
        }
        else
        {
            Debug.LogError(transform.root.gameObject.name + " is trying to eat a game objecto with no food");
        }
        
    }
}
