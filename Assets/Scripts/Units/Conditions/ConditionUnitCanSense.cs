using UnityEngine;

/**
 * Condicion que devuelve verdadero cuando la unidad puede escuchar al menos un objeto de la mascar indicadda
 * y almacena los que encuentre en una lista
 */
public class ConditionUnitCanSense : ConditionUnit
{
	[SerializeField]
	public enum SenseTarget { Prey,Predator,Mate}
	[SerializeField]
	SenseTarget senseTarget;

	public override bool Test()
	{
		LayerMask layers = 0;
		switch (senseTarget)
        {
            case SenseTarget.Prey:
				layers = unit.preyLayers;
                break;
            case SenseTarget.Predator:
				layers = unit.predatorLayers;
				break;
			case SenseTarget.Mate:
				layers = unit.gameObject.layer;
				break;
		}
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, unit.DetectionDistance, layers);
		if (hitColliders.Length > 0)
		{
			return true;
		}
		return false;
    }
}