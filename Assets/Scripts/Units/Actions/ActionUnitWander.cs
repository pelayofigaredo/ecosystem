using UnityEngine;
using UnityEngine.AI;


/**
 * Accion que hace a la unidad moverse entre puntos aleatorios
 */
public class ActionUnitWander : ActionUnit
{
	public float distance;
	public float time = 5;
	public float timeVariation = 2;

	public Vector3 destination;

	private float currentTime;
	private float timer;



    private void Start()
    {
		Initialice();
	}



    public override void Act()
	{
        if (!unit.isBusy)
        {
			base.Act();
			agent.destination = destination;
			timer += Time.deltaTime;
			if (timer > currentTime)
			{
				getNewPoint();
			}
			unit.DecreaseStamina(this);
		}

	}

	public override void InitializeAction()
	{
		if(destination == null)
        {
			getNewPoint();
		}

	}

	public void getNewPoint()
    {
		currentTime = time + Random.Range(-timeVariation, timeVariation);
		timer = 0;
		Vector3 randomPos = new Vector3(transform.position.x + (Random.Range(-distance, distance)),
			transform.position.y, transform.position.x + (Random.Range(-distance, distance)));
		int areaMask = 1;
		NavMeshHit navHit;
		NavMesh.SamplePosition(randomPos, out navHit, distance, areaMask);
		destination = navHit.position;
	}

}
