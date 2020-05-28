using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public abstract class Enemy : MonoBehaviour, ICanBeDamaged {

    protected Transform myPosition;
    protected int health;
    private Transform playerTransform;
    protected BaseCombatComponent combat;
    protected CircleCollider2D circleCollider;
    protected Rigidbody2D rb2D;
    [SerializeField]
    protected int vision;
    protected InputDelay inputDelay;
    public bool combatActive { get; private set; }
    protected BaseAIComponent ai;
    private bool movementStopped = false;
    private SpriteRenderer sprite;

    protected AIDestinationSetter destinationSetter;
    protected Transform target;
    protected AIPath aIPath;

    protected MeleeAttackSprite meleeAttack;

    protected AudioSource audioSource;
    [SerializeField]
    protected AudioClip damagedSound = default;

    protected DyingBreath dyingBreath;

    private LootPool loot;

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
        circleCollider = gameObject.GetComponent<CircleCollider2D>();
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        inputDelay = new InputDelay();
        combatActive = GameState.combatState;
        if (GameState.GameMovementStopped()) movementStopped = true;
        destinationSetter = gameObject.GetComponent<AIDestinationSetter>();
        aIPath = gameObject.GetComponent<AIPath>();
        meleeAttack = gameObject.GetComponentInChildren<MeleeAttackSprite>();
        audioSource = gameObject.GetComponent<AudioSource>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        dyingBreath = gameObject.GetComponent<DyingBreath>();
        loot = gameObject.GetComponent<LootPool>();
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
        // Debug.Log("ENEMY DAMAGED");
        health -= damage;
        StartCoroutine(TakeDamageAnim());
        if (!IsAlive()) Die();
        if (!combat.inCombat) AggroPlayer();
        PlayDamagedSound();
    }

    public bool IsAlive() {
        return health > 0;
    }

    protected void Die() {
        EventManager.RaiseEnemyDeath();
        gameObject.GetComponentInChildren<AggroIndicator>().gameObject.GetComponent<SpriteRenderer>().enabled = false;
        // Destroy(gameObject);
        circleCollider.enabled = false;
        sprite.enabled = false;
        loot.DropLoot();
        dyingBreath.StartDeathBreath();
    }

    private void PlayDamagedSound() {
        audioSource.PlayOneShot(damagedSound);
    }

    // ----------------------------------------------------------------
    // Turn Mechanics
    // ----------------------------------------------------------------

    private void TakeAction() {
        if (MyTurn() && IsAlive()) {
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
        return RayLinecastTools.ObjectVisible(circleCollider, transform.position, playerTransform, LayerMask.GetMask("Characters", "Obstructions", "Player"));
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (!combat.inCombat) aIPath.destination = this.transform.position;
    }

    private IEnumerator TakeDamageAnim() {
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        rend.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        rend.color = Color.white;
    }

}