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
    // protected TurnComponent turnComponent;
    protected int vision;
    protected InputDelay inputDelay;

    // ----------------------------------------------------------------
    // Initialization
    // ----------------------------------------------------------------

    void Awake() {
        // position = this.GetComponent<Transform>().position;
        grid = GameObject.FindWithTag("Grid").GetComponent<Grid>();
        playerTransform = GameObject.FindWithTag("Player").transform;
        Init();
    }

    protected void Init() {
        myPosition = gameObject.transform;
        movement = new MovementComponent(gameObject, this, grid);
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        circleCollider = gameObject.GetComponent<CircleCollider2D>();
        // turnComponent = new TurnComponent();
        inputDelay = new InputDelay();
        // movement.UpdateGrid(grid);
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
        // EventManager.RaiseOnCombat();
        // EventManager.RaisePlayerEntersCombat();
        EventManager.RaiseAggroPlayer();
    }

    protected Vector3 GetPlayerPosition() {
        return playerTransform.position;
    }

    public bool TakeDamage(int damage) {
        health -= damage;
        Debug.Log("Enemy current health: " + health);
        return true;
    }

    public bool IsAlive() {
        return health > 0;
    }

    protected void Die() {
        // if (combat.inCombat) EventManager.RaiseOnCombatExit();
        if (combat.inCombat) EventManager.RaiseEnemyDeath();
        else EventManager.RaiseEnemyDeath();
        Destroy(gameObject);
    }

    // ----------------------------------------------------------------
    // Turn Mechanics
    // ----------------------------------------------------------------

    protected abstract void CheckAlive();

    public abstract void TakeTurn();
    
    protected abstract void Patrol();

    // public bool MyTurn() {
    //     turnComponent.TurnTick();
    //     if ((Input.GetMouseButtonUp(0) || !combat.inCombat) && turnComponent.CanAct()) {
    //         return true;
    //     }
    //     return false;
    // }

    public bool MyTurn() {
        // inputDelay.DelayTick();
        inputDelay.DelayTick();
        if (inputDelay.CanAct()) {
            inputDelay.ResetInputDelay(); 
            return true;
        }
        return false;
    }

    // To be used when out of combat to initiate combat with player
    // Or to determine if there is line of sight to player
    // If player is out of range of vision, not visible
    // Else, check if there are obstructions in the way
    public bool PlayerVisible() {
        if (Vector3.Magnitude(playerTransform.position - transform.position) > vision && !combat.inCombat) {
            return false;
        }
        // return RayLinecastTools.ObjectVisible(boxCollider, transform.position, playerTransform, LayerMask.GetMask("Characters", "Obstructions"));
        return RayLinecastTools.ObjectVisible(boxCollider, circleCollider, transform.position, playerTransform, LayerMask.GetMask("Characters", "Obstructions"));
    }

}
