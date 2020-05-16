using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public abstract class Enemy : MonoBehaviour, ICanBeDamaged {

    protected Transform myPosition;
    // protected MovementComponent movement;
    protected int health;
    private Transform playerTransform;
    protected BaseCombatComponent combat;
    protected BoxCollider2D boxCollider;
    protected CircleCollider2D circleCollider;
    protected Rigidbody2D rb2D;
    [SerializeField]
    // protected Grid grid;
    protected int vision;
    protected InputDelay inputDelay;
    public bool combatActive { get; private set; }
    protected BaseAIComponent ai;
    private bool movementStopped = false;

    protected AIDestinationSetter destinationSetter;
    protected Transform target;
    protected AIPath aIPath;

    protected MeleeAttackSprite meleeAttack;
    
    // protected Vector3 target;

    // public float speed = 2f;
    // public float nextWaypointDistance = 3f;

    // protected Path path;
    // protected int currentWaypoint = 0;
    // protected bool reachedEndOfPath = false;

    // protected Seeker seeker;

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
        // grid = GameObject.FindWithTag("Grid").GetComponent<Grid>();
        playerTransform = GameObject.FindWithTag("Player").transform;
        Init();
    }

    private void Start() {
        // UpdatePath();
        destinationSetter.target = playerTransform;
        destinationSetter.enabled = false;
    }

    protected virtual void Update() {
        if (!combat.inCombat && PlayerVisible()) AggroPlayer();
        if (!movementStopped) aIPath.canMove = true;
        else aIPath.canMove = false;
        if (aIPath.canMove) TakeAction();
    }

    protected void Init() {
        myPosition = gameObject.transform;
        // movement = new MovementComponent(gameObject, this, grid);
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        circleCollider = gameObject.GetComponent<CircleCollider2D>();
        // seeker = gameObject.GetComponent<Seeker>();
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        inputDelay = new InputDelay();
        combatActive = GameState.combatState;
        // if (combatActive) movementStopped = true; // FIXME: Still has bug if player is moving when combat starts
        if (GameState.GameMovementStopped()) movementStopped = true;
        destinationSetter = gameObject.GetComponent<AIDestinationSetter>();
        aIPath = gameObject.GetComponent<AIPath>();
        meleeAttack = gameObject.GetComponentInChildren<MeleeAttackSprite>();
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
        gameObject.GetComponentInChildren<AggroIndicator>().ShowAggro();
        combat.EnterCombat();
        EventManager.RaiseAggroPlayer();
        aIPath.destination = Vector3.zero;
        destinationSetter.enabled = true;
    }

    protected Vector3 GetPlayerPosition() {
        return playerTransform.position;
    }

    public void TakeDamage(int damage) {
        Debug.Log("ENEMY DAMAGED");
        health -= damage;
        if (!combat.inCombat) AggroPlayer();
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

    private void TakeAction() {
        if (MyTurn()) {
            if (combat.inCombat) CombatBehavior();
            else NonCombatBehavior();
        }
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
        // if (PlayerVisible()) AggroPlayer();
        // else Patrol();
        Patrol();
    }

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

    private void OnCollisionEnter2D(Collision2D other) {
        if (!combat.inCombat) aIPath.destination = this.transform.position;
        // Debug.Log("Collision!");
    }

}

// say this thing

// This is relevant

// That brings up this

// Wrap it up with this