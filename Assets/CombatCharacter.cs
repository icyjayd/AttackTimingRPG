using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCharacter : MonoBehaviour


{
    public string unitName;
    public int maxHealth;
    public int currentHealth;
    [Tooltip("Player characters should go first (100-200)\n Enemy Characters should go last (1-10)")]
    public int speed;

    public bool turnOver = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
