using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, ICanBeDamaged, ITurnAct {

    protected Transform myPosition;
    protected MovementComponent movement;
    protected int health;
    private Transform playerTransform;
    protected BaseCombatComponent combat;
    protected BoxCollider2D boxCollider;
    protected CircleCollider2D circleCollider;
    [SerializeField]
    protected Grid grid;
    protected int vision;
    protected InputDelay inputDelay;
    public bool turnSystemActive { get; private set; }
    private bool isCombatTurn = false;
    protected BaseAIComponent ai;

    // ----------------------------------------------------------------
    // Initialization
    // ----------------------------------------------------------------

    private void OnEnable() {
        EventManager.combatStart += TurnSystemIsActive;
        EventManager.combatOver += TurnSystemNotActive;
    }

    private void OnDisable() {
        EventManager.combatStart -= TurnSystemIsActive;
        EventManager.combatOver -= TurnSystemNotActive;
        if (isCombatTurn) EventManager.RaiseActorTurnOver();
    }

    void Awake() {
        turnSystemActive = GameState.combatState;
        grid = GameObject.FindWithTag("Grid").GetComponent<Grid>();
        playerTransform = GameObject.FindWithTag("Player").transform;
        Init();
    }

    protected virtual void Update() {
        if (!turnSystemActive) NonCombatBehavior();
    }

    protected void Init() {
        myPosition = gameObject.transform;
        movement = new MovementComponent(gameObject, this, grid);
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        circleCollider = gameObject.GetComponent<CircleCollider2D>();
        inputDelay = new InputDelay();
        SetHealth();
        SetCombat();
        SetVision();
        SetAI();
    }

    protected abstract void SetHealth();

    protected abstract void SetCombat();

    protected abstract void SetVision();

    protected abstract void SetAI();

    // ----------------------------------------------------------------
    // Combat mechanics
    // ----------------------------------------------------------------

    protected void AggroPlayer() {
        combat.EnterCombat();
        EventManager.RaiseAggroPlayer(this);
    }

    protected Vector3 GetPlayerPosition() {
        return playerTransform.position;
    }

    public void TakeDamage(int damage) {
        Debug.Log("ENEMY DAMAGED");
        health -= damage;
        if (!IsAlive()) Die();
    }

    public bool IsAlive() {
        return health > 0;
    }

    protected void Die() {
        EventManager.RaiseEnemyDeath();
        Destroy(gameObject);
    }

    // ----------------------------------------------------------------
    // Turn Mechanics
    // ----------------------------------------------------------------

    public void TakeTurn() {
        isCombatTurn = true;
        StartCoroutine(TurnRoutine());
    }
    
    protected abstract void Patrol();

    public bool MyTurn() {
        inputDelay.DelayTick();
        if (inputDelay.CanAct()) {
            inputDelay.ResetInputDelay(); 
            return true;
        }
        return false;
    }

    protected abstract void CombatBehavior();

    protected virtual void NonCombatBehavior() {
        if (turnSystemActive || MyTurn()) Patrol();
    }

    private void TurnBehavior() {
        if (!combat.inCombat) NonCombatBehavior();
        else CombatBehavior();
    }

    public IEnumerator TurnRoutine() {
        // Debug.Log("Blob turn started.");
        TurnBehavior();
        yield return null;
        // Debug.Log("Blob turn ended.");
        isCombatTurn = false;
        EventManager.RaiseActorTurnOver();
    }

    public void TurnSystemIsActive() {
        turnSystemActive = true;
    }

    public void TurnSystemNotActive() {
        turnSystemActive = false;
    }

    // To be used when out of combat to initiate combat with player
    // Or to determine if there is line of sight to player
    // If player is out of range of vision, not visible
    // Else, check if there are obstructions in the way
    public bool PlayerVisible() {
        if (Vector3.Magnitude(playerTransform.position - transform.position) > vision && !combat.inCombat) {
            return false;
        }
        return RayLinecastTools.ObjectVisible(boxCollider, circleCollider, transform.position, playerTransform, LayerMask.GetMask("Characters", "Obstructions"));
    }

}
