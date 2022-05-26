using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveNode_MoveInFrontOfTarget : CombatMoveNode
{
	// TODO Velocity-based move option.

	public Vector3 positionRelativeToTarget;
	
	private Vector3 startPosition;
	private Vector3 targetPosition;
	
	public override void Fire (FireArguments args)
	{
		base.Fire (args);
		
		startPosition = combatData.actor.transform.position;
		if (combatData.actor.isPlayer)
		{
			targetPosition = combatData.target.transform.position + positionRelativeToTarget;
		}
		else
		{
			targetPosition = combatData.target.transform.position - positionRelativeToTarget;
		}
	}
	
	public override void UpdateFrame ()
	{
		float t = 1.0f * framesProgress / frameDuration;

		combatData.actor.transform.position =
			Vector3.Lerp(startPosition, targetPosition, t);
		
		base.UpdateFrame();
	}
}
