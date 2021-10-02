using UnityEngine;

/**
 * Condicion que devuelve verdadero cuando la estamina de la unidad sea mayor que el valor dado
 */
public class ConditionUnitStaminaGreaterThan : ConditionUnit
{
	[Range(0,1)]
	public float value = 0.5f;

	public override bool Test()
	{
		if (unit.GetStaminaPercent() >= value)
		{
			return true;
		}
		return false;
	}
}
