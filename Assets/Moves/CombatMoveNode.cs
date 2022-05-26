using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMoveNode : MonoBehaviour
{
    public int frameDuration = 10;
    protected int framesProgress = 0; 
    public List<CombatMoveNode> activateImmediate = new List<CombatMoveNode>();
    public List<CombatMoveNode> activateFinished = new List<CombatMoveNode>();

    public bool isFinished;
	public struct FireArguments
	{
		public enum eTargetType
		{
			Single,
			Row,
			RowAndBehind,
			All,
			AllRowDecay
		}

		public CombatCharacter actor;
		public eTargetType targetType;
		public CombatCharacter target;
		public List<CombatMoveNode> activeMoveNodes;

		public FireArguments(CombatCharacter actor, eTargetType targetType, CombatCharacter target, List<CombatMoveNode> activeMoveNodes)
		{
			this.actor = actor;
			this.targetType = targetType;
			this.target = target;
			this.activeMoveNodes = activeMoveNodes;
		}
	}
	protected FireArguments combatData;

	public virtual void Fire(FireArguments args)
	{
		Debug.Log(gameObject.name);
		isFinished = false;
		combatData = args;
		combatData.activeMoveNodes.Add(this);
		framesProgress = 0;
		foreach (CombatMoveNode c in activateImmediate)
		{
			c.Fire(args);
		}
	}

	public virtual void UpdateFrame()
	{
		framesProgress++;
		if (framesProgress > frameDuration)
		{
			foreach (CombatMoveNode c in activateFinished)
			{
				c.Fire(combatData);
			}
			isFinished = true;
		}
	}

	public static Vector3 arrowWidth = new Vector3(0.0f, 0.05f, 0.0f);
	public virtual void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		foreach (CombatMoveNode c in activateImmediate)
		{
			Gizmos.DrawLine(this.transform.position + arrowWidth, c.transform.position);
			Gizmos.DrawLine(this.transform.position - arrowWidth, c.transform.position);
		}
		Gizmos.color = Color.blue;
		foreach (CombatMoveNode c in activateFinished)
		{
			Gizmos.DrawLine(this.transform.position + arrowWidth, c.transform.position);
			Gizmos.DrawLine(this.transform.position - arrowWidth, c.transform.position);
		}
	}

}
