using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, ITurnAct {

    [SerializeField]
    private int baseDamage = 0;
    [SerializeField]
    private int tileMovePerTurn = 0;
    [SerializeField]
    private bool isContinuous = false;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    private ProjectileMovement movement;
    private bool combatActive = false;

    void Awake() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        movement = new ProjectileMovement(gameObject, this, GameObject.FindWithTag("Grid").GetComponent<Grid>());
    }

    void Update() {
        // if (!combatActive) transform.Translate(transform.up * 0.5f * Time.deltaTime);
        if (!combatActive) rb.velocity = (transform.up - transform.right) * 750.0f * Time.deltaTime;
    }

    public void TakeTurn() {
        ProjectileMove();
        // EventManager.RaiseActorTurnOver();
    }

    // public bool MyTurn() {
    //     turnComponent.TurnTick();
    //     if (Input.GetMouseButtonUp(0) && turnComponent.CanAct()) {
    //         return true;
    //     }
    //     return false;
    // }

    private void ProjectileMove() {
        movement.AttemptMove((transform.up - transform.right) * tileMovePerTurn);
    }

    private void EnableCombatFlag() {
        combatActive = true;
    }

    private void DisableCombatFlag() {
        combatActive = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        ICanBeDamaged victim = other.gameObject.GetComponent<ICanBeDamaged>();
        if (victim != null) {
            victim.TakeDamage(baseDamage);
        }
        if (!isContinuous) {
            // if (combatActive) EventManager.RaiseActorTurnOver();
            Destroy(gameObject);
        }
    }

}
