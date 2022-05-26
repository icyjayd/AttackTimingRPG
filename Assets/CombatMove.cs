using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class CombatMove : MonoBehaviour
{
	public static int FramesSinceLastButtonOnePress = 100;
	public static int FramesSinceLastButtonTwoPress = 100;
	public static bool ButtonPressed = false;
	public static int ButtonTiming;
	public new string name;
	[Multiline]
	public string description = "";

	[HideInInspector]
	public bool isDone;

	public CombatMoveNode startNode = null;
	
	public List<CombatMoveNode> activeNodes = null;

	public CombatMoveNode.FireArguments.eTargetType targetType = CombatMoveNode.FireArguments.eTargetType.Single;

	void OnValidate()
	{
		if (description != null)
		{
			description = description.Replace("\\n", "\n");
		}
	}
 
    public void Begin(CombatCharacter currentCharacter, CombatCharacter target, List<CombatCharacter> playerCharacters, List<CombatCharacter> enemyCharacters) //CombatManager combatManager)
	{
		isDone = false;
		activeNodes = new List<CombatMoveNode>();
		startNode.Fire(new CombatMoveNode.FireArguments(currentCharacter, targetType, target, activeNodes));
	}

	public void UpdateFrame()
	{
		//if (CombatManager.Singleton.keyInput.GetButton(Keybindings.EButton.Ok, Keybindings.EButtonState.Down))
		if (Keyboard.current.spaceKey.wasPressedThisFrame && !ButtonPressed)
        {
            FramesSinceLastButtonOnePress = 0;
			ButtonPressed = true;
        }
        else
        {
            FramesSinceLastButtonOnePress++;
        }
        //if (CombatManager.Singleton.keyInput.GetButton(Keybindings.EButton.Cancel, Keybindings.EButtonState.Down))
        //{
        //	FramesSinceLastButtonTwoPress = 0;
        //}
        //else
        //{
        //	FramesSinceLastButtonTwoPress++;
        //}

        for (int i = 0; i < activeNodes.Count; i++)
		{
			activeNodes[i].UpdateFrame();
		}
		for (int i = activeNodes.Count - 1; i >= 0; i--)
		{
			if (activeNodes[i].isFinished)
			{
				CombatMoveNode node = activeNodes[i];
				activeNodes.Remove(node);
			}
		}
		if (activeNodes.Count == 0)
		{
			isDone = true;
			Debug.Log("done");
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		if (activeNodes != null)
		{
			foreach (CombatMoveNode c in activeNodes)
			{
				Gizmos.DrawWireSphere(c.transform.position, 1.0f);
			}
		}
	}
}