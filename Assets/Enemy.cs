using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CombatCharacter
{
    public List<CombatMove> moveList = new List<CombatMove>();
    // Start is called before the first frame update
    public override void Start()
    {
        BattleController.Instance.RegisterEnemy(this);
        base.Start();
    }

}
