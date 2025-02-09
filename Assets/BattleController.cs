using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.InputSystem;
using TMPro;
public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}
public enum MenuType { ATTACK, BLOCK, ITEM, SPECIAL}
public class BattleController : MonoBehaviour
{
    public PlayerCharacter currentPlayer;
    public List<CombatCharacter> playerList;
    public Enemy currentEnemy;
    public CombatCharacter selectedTarget;
    public List<CombatCharacter> enemyList;
    public List<CombatCharacter> combatants;
    BattleState battleState;

    public Button attackButton;
    public Button blockButton;
    public Button itemButton;
    public Button specialButton;

    public Button buttonPrefab;
    public Button targetSelector;
    public int targetSelectorIndex = 0;
    TextMeshProUGUI targetSelectorText;
    bool targetSelectorMoved = false;
    public GameObject attackSubmenu;
    CombatMove combatMove;
    public CombatMove basicAttack;
    Vector2 _menuMovement;
    UnityAction selectedAction;
    //public GameObject blockSubmenu;
    //public GameObject itemSubmenu;
    //public GameObject specialSubmenu;
    EventSystem m_EventSystem;


    public static PlayerInputActions controls;


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
        targetSelectorText = targetSelector.GetComponentInChildren<TextMeshProUGUI>();
        //fireAction.performed += Fire;
    }
    private void Start()
    {
        battleState = BattleState.START;
        SetUpBattle();
    }
    static int SortBySpeed(CombatCharacter p1, CombatCharacter p2)
    {
        return p1.speed.CompareTo(p2.speed);
    }
    
    static int SortByPosition(CombatCharacter p1, CombatCharacter p2)
    {
        return p1.transform.position.x.CompareTo(p2.transform.position.x);
    }

    private void Update()
    {

        if (targetSelector.isActiveAndEnabled)
        {
            HandleTargetSelectorIndex();
            selectedTarget = enemyList[targetSelectorIndex];
            SelectAttackAction();

        }
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
        AdvanceBattleStage();
        //GameObject playerGO = Instantiate()
    }

    void HandleTargetSelectorIndex()
    {
        _menuMovement = controls.Player.Move.ReadValue<Vector2>();
        if (_menuMovement.x > 0.2f && !targetSelectorMoved)
        {
            targetSelectorIndex = Mathf.Clamp(targetSelectorIndex + 1, 0, enemyList.Count - 1);
            targetSelectorMoved = true;
        }
        else if (_menuMovement.x < -0.2f && !targetSelectorMoved)
        {
            targetSelectorIndex = Mathf.Clamp(targetSelectorIndex - 1, 0, enemyList.Count - 1);
            targetSelectorMoved = true;
        }
        else if (_menuMovement.x == 0 && targetSelectorMoved == true)
        {
            targetSelectorMoved = false;
        }
        targetSelector.onClick.RemoveListener(selectedAction);

    }
    void AdvanceBattleStage()
    {
        enemyList.Sort(SortBySpeed);
        combatants.Sort(SortBySpeed);
        switch (battleState)
        {
            case BattleState.START:
                battleState = BattleState.PLAYERTURN;
                Debug.Log("It is now the player's turn");
                break;
            case BattleState.PLAYERTURN:
                playerList.Sort(SortBySpeed);
                foreach (CombatCharacter p in playerList)
                {
                    if (!p.turnOver)
                    {
                        currentPlayer = p.GetComponent<PlayerCharacter>();
                        EnableButtons();
                        ActivateButtons();
                        return;
                    }
                }
                enemyList.Sort(SortBySpeed);
                currentEnemy = enemyList[0].GetComponent<Enemy>();
                battleState = BattleState.ENEMYTURN;
                Debug.Log("It is now the enemy's turn");
                BeginEnemyTurn();
                
                foreach(CombatCharacter p in playerList)
                {
                    p.turnOver = false;
                }
                break;
            case BattleState.ENEMYTURN:
                enemyList.Sort(SortBySpeed);
                foreach(CombatCharacter e in enemyList)
                {
                    if (!e.turnOver)
                    {
                        currentEnemy = e.GetComponent<Enemy>();
                        //TODO: start enemy turn
                        BeginEnemyTurn();
                        return;
                    }
                    
                }
                playerList.Sort(SortBySpeed);
                currentPlayer = playerList[0].GetComponent<PlayerCharacter>();
                battleState = BattleState.PLAYERTURN;
                Debug.Log("it is now the player's turn");
                EnableButtons();
                ActivateButtons();
                foreach(CombatCharacter e in enemyList)
                {
                    e.turnOver = false;
                }
                break;
            default:
                break;
        }
        


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
        AdvanceBattleStage();

    
    }
    public void Attack(CombatCharacter attacker, CombatCharacter target, CombatMove chosenMove = null)
    {
        if (chosenMove != null)
        {
            combatMove = chosenMove;
            StartCoroutine(BeginCombatMove(combatMove, attacker, target));

        }
        print(string.Format("attacking {0}", target.name));
    }
    #endregion

    #region BattleManagement

    void BeginEnemyTurn()
    {
        Debug.Log(string.Format("Beginning {0}'s turn!", currentEnemy.name));
        int moveChoice = Random.Range(0, currentEnemy.moveList.Count);
        CombatMove c = currentEnemy.moveList[moveChoice];
        int playerChoice = Random.Range(0, playerList.Count);
        CombatCharacter target = playerList[playerChoice];
        StartCoroutine(BeginCombatMove(c, currentEnemy, target));

        //currentEnemy.turnOver = true;
        //AdvanceBattleStage();
    }
    #endregion
    #region ButtonManagement
    public void DisableButtons()
    {
        attackButton.enabled = false;
        blockButton.enabled = false;
        itemButton.enabled = false;
        specialButton.enabled = false;
    }
    public void EnableButtons()
    {
        attackButton.enabled = true;
        blockButton.enabled = true;
        itemButton.enabled = true;
        specialButton.enabled = true;
    }
    public void DeactivateButtons()
    {
        targetSelector.gameObject.SetActive(false);
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
        m_EventSystem.SetSelectedGameObject(attackButton.gameObject);

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
    void OnAttackButtonPress()
    {
        targetSelector.gameObject.SetActive(true);
        m_EventSystem.SetSelectedGameObject(targetSelector.gameObject);
        enemyList.Sort(SortByPosition);
        selectedTarget = enemyList[0];
        //PopulateSubmenu(MenuType.ATTACK);
        //m_EventSystem.SetSelectedGameObject(attackSubmenu.GetComponentInChildren<Button>().gameObject);
        print("Attack button was pressed!");
        targetSelector.onClick.AddListener(() => DeactivateButtons());
        SelectAttackAction();
        DisableButtons();
    }

    void SelectAttackAction()
    {
        selectedAction = new UnityAction(() => Attack(currentPlayer, selectedTarget, currentPlayer.currentAttack));
        targetSelectorText.text = selectedTarget.name;
        targetSelector.onClick.AddListener(selectedAction);

    }
    #endregion
    public void Fire()///InputAction.CallbackContext context)
    {
        Debug.Log("Fire!");
    }


    #region OnButtonPress

    
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
