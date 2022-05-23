using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public CombatCharacter currentCharacter;
    public CombatCharacter target;
    public List<CombatCharacter> playerCharacters;
    public List<CombatCharacter> enemyCharacters;
    public CombatMove combatMove;
    // Start is called before the first frame update
    void Start()
    {
        //combatMove.Begin(currentCharacter, target, playerCharacters, enemyCharacters);
    }

    // Update is called once per frame
    void Update()
    {
        //combatMove.UpdateFrame();
        
    }
}
