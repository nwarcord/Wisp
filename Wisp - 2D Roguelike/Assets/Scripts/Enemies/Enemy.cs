using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, ICanBeDamaged, ITurnAct {

    protected Transform myPosition;
    protected MovementComponent movement;
    protected int health;
    private Transform playerTransform;
    protected CombatComponent combat;
    protected BoxCollider2D boxCollider;
    protected CircleCollider2D circleCollider;
    [SerializeField]
    protected Grid grid;
    protected int vision;
    protected InputDelay inputDelay;

    // ----------------------------------------------------------------
    // Initialization
    // ----------------------------------------------------------------

    void Awake() {
        grid = GameObject.FindWithTag("Grid").GetComponent<Grid>();
        playerTransform = GameObject.FindWithTag("Player").transform;
        Init();
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
    }

    protected abstract void SetHealth();

    protected abstract void SetCombat();

    protected abstract void SetVision();

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
        health -= damage;
        if (!IsAlive()) Die();
        // return true;
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

    // protected abstract void CheckAlive();

    public void TakeTurn() {
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

    protected abstract void TurnBehavior();

    public IEnumerator TurnRoutine() {
        TurnBehavior();
        yield return null;
        EventManager.RaiseActorTurnOver();
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
