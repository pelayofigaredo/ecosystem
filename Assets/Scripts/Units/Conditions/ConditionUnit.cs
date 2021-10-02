public class ConditionUnit : ICondition
{
    protected Unit unit;
    void Start()
    {
        unit = transform.root.GetComponent<Unit>();
    }
}
