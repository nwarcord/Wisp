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
    [SerializeField]
    private int tileRange = 50;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    private ProjectileMovement movement;
    private bool combatActive = false;
    private bool isCombatTurn = false;
    private Vector3 startingPoint;
    private bool isColliding = false;

    private void OnEnable() {
        EventManager.combatStart += EnableCombatFlag;
        EventManager.combatOver += DisableCombatFlag;
    }

    private void OnDisable() {
        EventManager.combatStart -= EnableCombatFlag;
        EventManager.combatOver -= DisableCombatFlag;
        StopAllCoroutines();
        if (isCombatTurn) EventManager.RaiseActorTurnOver();
    }

    void Awake() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        movement = new ProjectileMovement(gameObject, this, GameObject.FindWithTag("Grid").GetComponent<Grid>());
        combatActive = GameState.combatState;
        startingPoint = transform.position;
        if (tileRange > 50) {
            Debug.LogError("Projectile range cannot exceed 50 tiles");
            tileRange = 50;
        }
    }

    void FixedUpdate() {
        if (!combatActive) rb.velocity = (transform.up - transform.right) * 250.0f * Time.deltaTime;
        else rb.velocity = new Vector2();
        CheckFlightDistance();
        isColliding = false;
    }

    public void TakeTurn() {
        isCombatTurn = true;
        StartCoroutine(TurnRoutine());
    }

    private void ProjectileMove() {
        movement.AttemptMove(transform.position + ((transform.up - transform.right).normalized * tileMovePerTurn));
    }

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

    private void OnTriggerEnter2D(Collider2D other) {
        ICanBeDamaged victim = other.gameObject.GetComponent<ICanBeDamaged>();
        if (victim != null && !isColliding) {
            isColliding = true;
            victim.TakeDamage(baseDamage);
        }
        if (!isContinuous) {
            Destroy(gameObject);
        }
    }

    public IEnumerator TurnRoutine() {
        ProjectileMove();
        yield return null;
        EventManager.RaiseActorTurnOver();
        isCombatTurn = false;
    }

}
