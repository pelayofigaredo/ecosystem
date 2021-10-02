using UnityEngine;
using System.Collections;

/// <summary>
/// Makes use (play/stop) an steering agent.
/// </summary>
public class ActionUseSteeringAgent : IAction
{
	/// <summary>
	/// Steering agent that will be stopped/played.
	/// </summary>
	// public SteeringAgent agent; // O la clase que usemos para nuestros steering agents

	public bool shouldStopInstead = false;


	public override void Act ()
	{
		if (shouldStopInstead)
        {
			// agent.Stop();
        }
		else
		{
			// agent.Play();
		}
	}
}
