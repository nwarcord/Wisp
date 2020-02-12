using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, ICanBeDamaged {//, ITurnAct {

    private int health;
    private CombatComponent combat;
    private MovementComponent movement;
    private BoxCollider2D boxCollider;
    private TurnComponent turnComponent;
    [SerializeField]
    private Grid grid;

    // ----------------------------------------------------------------
    // Event subscribe and unsubscribe
    // ----------------------------------------------------------------

    private void OnEnable() {
        // EventManager.onCombat += combat.EnterCombat;
        // EventManager.combatExit += combat.ExitCombat;
        // EventManager.playerEntersCombat += combat.EnterCombat;
        // EventManager.playerLeftCombat += combat.ExitCombat;
        EventManager.enemyDeath += CheckIfCombatOver;
        EventManager.aggroPlayer += PlayerEnterCombat;
    }

    private void OnDisable() {
        // EventManager.onCombat -= combat.EnterCombat;
        // EventManager.combatExit -= combat.ExitCombat;
        // EventManager.playerEntersCombat -= combat.EnterCombat;
        // EventManager.playerLeftCombat -= combat.ExitCombat;
        EventManager.enemyDeath -= CheckIfCombatOver;
        EventManager.aggroPlayer -= PlayerEnterCombat;
    }

    // ----------------------------------------------------------------
    // Initialization
    // ----------------------------------------------------------------

    void Awake() {
        health = 3;
        turnComponent = new TurnComponent();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        // combat = new CombatComponent(1, this.grid, this.boxCollider, this.turnComponent);
        combat = new CombatComponent(2, this.grid, this.boxCollider);
        movement = new MovementComponent(gameObject, this, grid);
    }

    private void Update() {
        // if (!IsAlive()) {
            // Destroy(gameObject);
        // }
        // TakeTurn();
    }

    public CombatComponent Combat() {
        return this.combat;
    }

    // ----------------------------------------------------------------
    // Combat mechanics
    // ----------------------------------------------------------------

    public void PlayerEnterCombat() {
        if (!combat.inCombat) EventManager.RaisePlayerEntersCombat();
        combat.EnterCombat();
    }

    public void CheckIfCombatOver() {
        combat.ExitCombat();
        if (!combat.inCombat) EventManager.RaisePlayerLeftCombat();
    }

    public bool TakeDamage(int damage) {
        health -= damage;
        Debug.Log("Player health: " + this.health + " | Damage taken: " + damage);
        return true;
    }

    public bool IsAlive() {
        return health > 0;
    }

    // ----------------------------------------------------------------
    // Turn mechanics
    // ----------------------------------------------------------------

    public MovementComponent GetMovement() {
        return movement;
    }

    // public void TakeTurn() {
        
    //     EventManager.RaiseActorTurnOver();
    // }

    // public bool MyTurn() {
    //     return false;
    // }

}
