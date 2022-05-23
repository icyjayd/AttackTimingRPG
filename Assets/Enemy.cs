using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CombatCharacter
{
    List<CombatMove> moveList = new List<CombatMove>();
    // Start is called before the first frame update
    void Start()
    {
        BattleController.Instance.RegisterEnemy(GetComponent<CombatCharacter>());
    }

}
