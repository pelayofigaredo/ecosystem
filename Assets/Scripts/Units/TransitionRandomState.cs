using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Transition that leads from one state from another if the condition
/// associated is triggered. It may optionally have an action that is
/// executed when the transition is triggered.
/// </summary>
public class TransitionRandomState : StateTransition
{
	public List<State> targetStates;

	public override bool IsTriggered(State state)
	{
		if (condition != null && condition.Test())
		{
			// Disable previous state
			if (state != null)
			{
				//state.enabled = false;
				state.DisableState();

				// Take the actions of the transition
				for (int i = 0; i < actions.Length; ++i)
				{
					IAction action = actions[i];
					action.InitializeAction();
					action.Act();
					action.FinalizeAction();
				}

				// Trigger the animator state
				if (animatorToTrigger != null)
				{
					animatorToTrigger.SetTrigger(hashedTriggerName);
				}
			}
			targetState = targetStates[Random.Range(0, targetStates.Count)];
			// Enable target state
			if (targetState != null)
			{
				//targetState.enabled = true;
				targetState.EnableState();
			}
			else
			{
				Debug.LogError("[" + gameObject.name + "] StateTransition null targetState");
			}

			return true;
		}

		return false;
	}

}
