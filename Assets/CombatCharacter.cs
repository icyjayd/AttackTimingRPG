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
    public bool isPlayer;
    public Vector3 startingPosition;
    // Start is called before the first frame update
    public virtual void Start()
    {
        startingPosition = transform.position;
        //Debug.Log(this.name + "'s starting position: " + startingPosition.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
