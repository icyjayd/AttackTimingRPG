using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveNode_PlayerAttack : CombatMoveNode
{
	public static int framesForTimedHit = 100;

	public List<CombatMoveNode> activateTimedHitSuccessful;
	public List<CombatMoveNode> activateTimedHitFailed;

	public enum InputType
	{
		PressAOnHit,
		None
	};
	public InputType timedHitType = InputType.PressAOnHit;

	public override void OnDrawGizmos ()
	{
		base.OnDrawGizmos();

		Gizmos.color = Color.green;
		foreach (CombatMoveNode c in activateTimedHitSuccessful)
		{
			Gizmos.DrawLine(this.transform.position+arrowWidth, c.transform.position);
			Gizmos.DrawLine(this.transform.position-arrowWidth, c.transform.position);
		}
		Gizmos.color = Color.yellow;
		foreach (CombatMoveNode c in activateTimedHitFailed)
		{
			Gizmos.DrawLine(this.transform.position+arrowWidth, c.transform.position);
			Gizmos.DrawLine(this.transform.position-arrowWidth, c.transform.position);
		}
	}

	public override void Fire (FireArguments args)
	{
		base.Fire (args);

		bool timedHitSuccess = false;

		switch (timedHitType)
		{
		case InputType.PressAOnHit:
				timedHitSuccess = (CombatMove.FramesSinceLastButtonOnePress < framesForTimedHit);
			Debug.Log("Timed hit frames:" + CombatMove.FramesSinceLastButtonOnePress);
				//Debug.Log("Timed hit success: " + timedHitSuccess.ToString());
				//Debug.Log("Button pressed before timed hit: " + CombatMove.ButtonPressed.ToString());
				
			break;
		default:
			break;
		}
		if (timedHitSuccess)
		{

			foreach (CombatMoveNode c in activateTimedHitSuccessful)
			{
				c.Fire(args);
			}
		}
		else
		{
			foreach (CombatMoveNode c in activateTimedHitFailed)
			{
				c.Fire(args);
			}
		}
		CombatMove.ButtonPressed = false;

	}

}
