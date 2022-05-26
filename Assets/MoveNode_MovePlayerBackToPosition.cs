using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNode_MovePlayerBackToPosition : CombatMoveNode
{
	Vector3 startPosition;
	Vector3 targetPosition;
	// Start is called before the first frame update
	public override void Fire(FireArguments args)
	{
		base.Fire(args);
		startPosition = args.actor.transform.position;
		//Debug.Log(startPosition + args.actor.name);
		targetPosition = args.actor.startingPosition;

	}

	// Update is called once per frame
	public override void UpdateFrame()
	{
		float t = 1.0f * framesProgress / frameDuration;

		combatData.actor.transform.position =
			Vector3.Lerp(startPosition, targetPosition, t);

		base.UpdateFrame();
		//Debug.Log("Start position of " + combatData.actor.name + ": " + startPosition.ToString() + combatData.actor.startingPosition);


	}
}
