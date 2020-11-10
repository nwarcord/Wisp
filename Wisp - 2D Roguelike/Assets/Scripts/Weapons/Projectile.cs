using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour/*, ITurnAct*/ {

    [SerializeField]
    private int baseDamage = 0;
    [SerializeField]
    private bool isContinuous = false;
    [SerializeField]
    private int tileRange = 50;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    private bool combatActive = false;
    private bool isCombatTurn = false;
    private Vector3 startingPoint;
    private bool isColliding = false;
    private bool movementStopped = false;

    private void OnEnable() {
        EventManager.combatStart += EnableCombatFlag;
        EventManager.combatOver += DisableCombatFlag;
        EventManager.playerMoving += EnableMovement;
        EventManager.playerStopped += DisableMovement;
    }

    private void OnDisable() {
        EventManager.combatStart -= EnableCombatFlag;
        EventManager.combatOver -= DisableCombatFlag;
        EventManager.playerMoving -= EnableMovement;
        EventManager.playerStopped -= DisableMovement;
        StopAllCoroutines();
        if (isCombatTurn) EventManager.RaiseActorTurnOver();
    }

    void Awake() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        combatActive = GameState.combatState;
        startingPoint = transform.position;
        if (GameState.GameMovementStopped()) movementStopped = true;
        if (tileRange > 50) {
            Debug.LogError("Projectile range cannot exceed 50 tiles");
            tileRange = 50;
        }
    }

    void FixedUpdate() {
        if (!combatActive || !movementStopped) rb.velocity = (transform.up - transform.right) * 250.0f * Time.deltaTime;
        else rb.velocity = new Vector2();
        CheckFlightDistance();
        isColliding = false;
    }

    // Determine if max range has been met
    private void CheckFlightDistance() {
        if (CurrentFlightDistance() >= tileRange) {
            Destroy(gameObject);
        }
    }

    private float CurrentFlightDistance() {
        return Vector3.Magnitude(transform.position - startingPoint);
    }

    private void EnableCombatFlag() {
        combatActive = true;
    }

    private void DisableCombatFlag() {
        combatActive = false;
    }

    // When colliding with another object
    private void OnTriggerEnter2D(Collider2D other) {
        ICanBeDamaged victim = other.gameObject.GetComponent<ICanBeDamaged>();
        if (victim != null && !isColliding) {
            isColliding = true;
            // victim.TakeDamage(baseDamage);
            victim.TakeDamage(new AttackInfo(baseDamage));
        }
        if (!isContinuous) {
            Destroy(gameObject);
        }
    }

    private void EnableMovement() {
        movementStopped = false;
    }

    private void DisableMovement() {
        movementStopped = true;
    }

}
