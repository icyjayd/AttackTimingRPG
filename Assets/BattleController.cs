using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.InputSystem;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}
public enum MenuType { ATTACK, BLOCK, ITEM, SPECIAL}
public class BattleController : MonoBehaviour
{
    public PlayerCharacter currentPlayer;
    public List<CombatCharacter> playerList;
    public Enemy currentEnemy;
    public List<CombatCharacter> enemyList;
    public List<CombatCharacter> combatants;
    BattleState battleState;

    public Button attackButton;
    public Button blockButton;
    public Button itemButton;
    public Button specialButton;

    public GameObject attackSubmenu;
    CombatMove combatMove;
    public CombatMove basicAttack;
    
    //public GameObject blockSubmenu;
    //public GameObject itemSubmenu;
    //public GameObject specialSubmenu;
    EventSystem m_EventSystem;

    public Button buttonPrefab;
    PlayerInputActions controls;


    private static BattleController _instance = null;
    
    public static BattleController Instance
    {
        get
        { if(_instance == null)
            {
                _instance = new BattleController();
            }
            return _instance;
        }
    }


    /// <summary>
    /// Testing the input system
    /// </summary>
    private void OnEnable()
    {
        controls.Enable();
        m_EventSystem = EventSystem.current;
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Awake()
    {
        controls = new PlayerInputActions();
        controls.Player.Fire.performed += ctx => Fire();
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        enemyList = new List<CombatCharacter>();
        
        //fireAction.performed += Fire;
    }
    static int SortBySpeed(CombatCharacter p1, CombatCharacter p2)
    {
        return p1.speed.CompareTo(p2.speed);
    }
    private void Start()
    {
        battleState = BattleState.START;
        SetUpBattle();
    }
    private void Update()
    {
        //if(combatMove != null)
        //{
        //    while (!combatMove.isDone)
        //    {
        //        combatMove.UpdateFrame();
        //    }
        //    combatMove = null;
        //}
        //print(EventSystem.current);
    }
    #region Setup
    void SetUpBattle()
    {
        foreach(CombatCharacter p in playerList)
        {
            combatants.Add(p);
        }
        foreach(CombatCharacter e in enemyList)
        {
            combatants.Add(e);
        }
        //GameObject playerGO = Instantiate()
    }

    void AdvanceBattleStage()
    {
        


    }
    public void RegisterPlayer(PlayerCharacter player)
    {
        playerList.Add(player);
        if (!currentPlayer)
        {
            currentPlayer = player;
        }
    }
    public void RegisterEnemy(Enemy enemy)
    {
        enemyList.Add(enemy);
        print("Registered enemy");
    }
    #endregion
    #region Attacks

    IEnumerator BeginCombatMove(CombatMove combatMove, CombatCharacter attacker, CombatCharacter target,
                                float delay = 0.5f) {
        combatMove.Begin(attacker, target, playerList, enemyList);
        while (!combatMove.isDone)
        {
            combatMove.UpdateFrame();
            yield return null;
        }
        yield return new WaitForSeconds(delay);
        attacker.turnOver = true;

    
    }
    public void Attack(CombatCharacter attacker, CombatCharacter target, CombatMove chosenMove = null)
    {
        if (chosenMove != null)
        {
            combatMove = chosenMove;
            StartCoroutine(BeginCombatMove(combatMove, attacker, target));

        }
        print(string.Format("attacking {0}", target.GetComponent<CombatCharacter>().name));
    }
    #endregion

    #region BattleManagement
    public void ActivateNextTurn()
    {
        Debug.Log("activating next turn, supposedly");
    }
    #endregion
    #region ButtonManagement
    public void DeactivateButtons()
    {
        attackButton.gameObject.SetActive(false);
        blockButton.gameObject.SetActive(false);
        itemButton.gameObject.SetActive(false);
        specialButton.gameObject.SetActive(false);
    }

    public void ActivateButtons()
    {
        attackButton.gameObject.SetActive(true);
        blockButton.gameObject.SetActive(true);
        itemButton.gameObject.SetActive(true);
        specialButton.gameObject.SetActive(true);

    }

    public void PopulateSubmenu(MenuType menuType)
    {
        switch (menuType)
        {
            case MenuType.ATTACK:
                attackSubmenu.SetActive(true);//for each enemy in the enemy registry, add a button 

                for (int i=0; i < enemyList.Count; i++) {
                    Button buttonGO = Instantiate(buttonPrefab);
                    buttonGO.transform.parent = attackSubmenu.transform;
                    int x = i;
                    Vector3 offset = new Vector3(650, 0, 0) - x * new Vector3(0, 100, 0);
                    buttonGO.transform.position = attackSubmenu.transform.position + offset;
                    buttonGO.onClick.AddListener(() => Attack(currentPlayer, enemyList[x], currentPlayer.currentAttack));
                    buttonGO.onClick.AddListener(() => DeactivateButtons());

                }

                break;
            default:
                break;
        }

    }
    #endregion
    public void Fire()///InputAction.CallbackContext context)
    {
        Debug.Log("Fire!");
    }
    #region OnButtonPress
    public void OnAttackButtonPress()
    {
        PopulateSubmenu(MenuType.ATTACK);
        m_EventSystem.SetSelectedGameObject(attackSubmenu.GetComponentInChildren<Button>().gameObject);
        print("Attack button was pressed!");
 
    }
    
    public void OnItemButtonPressed()
    {
        print("Item button was pressed!");
    }

    public void OnBlockButtonPressed()
    {
        print("Block button was pressed!");
    }
    
    public void OnSpecialButtonPressed()
    {
        print("Special button was pressed!");
    }
    #endregion
}
