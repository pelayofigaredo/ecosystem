using UnityEngine.AI;
/**
 * Clase padre de mis acciones propias que guarda informacion sobre una unidad
 */
public class ActionUnit : IAction
{

    protected NavMeshAgent agent;
    protected Unit unit;
    public float staminaCost = 0.1f;

    void Awake()
    {
        Initialice();
    }

    protected void Initialice()
    {
        unit = transform.root.GetComponent<Unit>();
        agent = unit.GetComponent<NavMeshAgent>();
    }

    public override void Act()
    {
        unit.DecreaseStamina(this);
    }
}
