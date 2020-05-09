using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, ICanBeDamaged {

    protected Transform myPosition;
    protected MovementComponent movement;
    protected int health;
    private Transform playerTransform;
    protected BaseCombatComponent combat;
    protected BoxCollider2D boxCollider;
    protected CircleCollider2D circleCollider;
    protected Rigidbody2D rb2D;
    [SerializeField]
    protected Grid grid;
    protected int vision;
    protected InputDelay inputDelay;
    public bool combatActive { get; private set; }
    protected BaseAIComponent ai;
    private bool movementStopped = false;

    // ----------------------------------------------------------------
    // Initialization
    // ----------------------------------------------------------------

    private void OnEnable() {
        EventManager.combatStart += CombatIsActive;
        EventManager.combatOver += CombatNotActive;
        EventManager.playerMoving += EnableMovement;
        EventManager.playerStopped += DisableMovement;
    }

    private void OnDisable() {
        EventManager.combatStart -= CombatIsActive;
        EventManager.combatOver -= CombatNotActive;
        EventManager.playerMoving -= EnableMovement;
        EventManager.playerStopped -= DisableMovement;
    }

    void Awake() {
        grid = GameObject.FindWithTag("Grid").GetComponent<Grid>();
        playerTransform = GameObject.FindWithTag("Player").transform;
        Init();
    }

    protected virtual void Update() {
        if (!movementStopped) Patrol();
        else rb2D.velocity = Vector2.zero;
    }

    protected void Init() {
        myPosition = gameObject.transform;
        movement = new MovementComponent(gameObject, this, grid);
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        circleCollider = gameObject.GetComponent<CircleCollider2D>();
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        inputDelay = new InputDelay();
        combatActive = GameState.combatState;
        if (combatActive) movementStopped = true; // FIXME: Still has bug if player is moving when combat starts
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
        EventManager.RaiseAggroPlayer();
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

    protected virtual void NonCombatBehavior() {}

    public void CombatIsActive() {
        combatActive = true;
    }

    public void CombatNotActive() {
        combatActive = false;
    }

    public void EnableMovement() {
        movementStopped = false;
    }

    public void DisableMovement() {
        movementStopped = true;
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
