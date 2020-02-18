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

    private void OnEnable() {
        EventManager.combatStart += EnableCombatFlag;
        EventManager.combatOver += DisableCombatFlag;
    }

    private void OnDisable() {
        EventManager.combatStart -= EnableCombatFlag;
        EventManager.combatOver -= DisableCombatFlag;
        StopAllCoroutines();
    }

    void Awake() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        movement = new ProjectileMovement(gameObject, this, GameObject.FindWithTag("Grid").GetComponent<Grid>());
        combatActive = GameState.combatState;
    }

    void FixedUpdate() {
        if (!combatActive) rb.velocity = (transform.up - transform.right) * 250.0f * Time.deltaTime;
        else rb.velocity = new Vector2();
    }

    public void TakeTurn() {
        StartCoroutine(TurnRoutine());
    }

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
            Destroy(gameObject);
        }
    }

    public IEnumerator TurnRoutine() {
        ProjectileMove();
        yield return null;
        EventManager.RaiseActorTurnOver();
    }

}
