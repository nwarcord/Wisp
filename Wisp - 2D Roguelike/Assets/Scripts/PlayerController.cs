using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, ICanBeDamaged {

    private int health;
    private CombatComponent combat;
    private MovementComponent movement;
    private BoxCollider2D boxCollider;
    private CircleCollider2D circleCollider;
    private TurnComponent turnComponent;
    [SerializeField]
    private Grid grid = default;

    // ----------------------------------------------------------------
    // Event subscribe and unsubscribe
    // ----------------------------------------------------------------

    private void OnEnable() {
        EventManager.enemyDeath += CheckIfCombatOver;
        EventManager.aggroPlayer += PlayerEnterCombat;
    }

    private void OnDisable() {
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
        circleCollider = gameObject.GetComponent<CircleCollider2D>();
        combat = new CombatComponent(gameObject, 2, this.grid, this.boxCollider);
        movement = new MovementComponent(gameObject, this, grid);
    }

    private void Update() {

        // Frame to frame behavior

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

}
