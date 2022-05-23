using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : CombatCharacter
{
 
    public CombatMove currentAttack;

    public void Start()
    {
        BattleController.Instance.RegisterPlayer(this);
    }
}
