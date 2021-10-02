using UnityEngine;

public class ConditionUnitDangerTargetOutOfDetection : ConditionUnit
{
	public float aditionalThreshold = 5;
	public override bool Test()
	{
		if (unit.DangerTarget != null)
		{
			float distance = Vector3.Distance(unit.transform.position, unit.DangerTarget.position);
			if (distance > (unit.DetectionDistance + aditionalThreshold))
			{
				return true;
			}
		}
		return false;
	}
}

