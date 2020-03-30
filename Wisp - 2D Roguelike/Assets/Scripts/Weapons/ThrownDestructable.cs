using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownDestructable : MonoBehaviour, ITurnAct {
    
    // [SerializeField]
    // private AreaOfEffect aoeSpawn;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private int tileMovePerTurn = 0;
    private BoxCollider2D boxCollider;
    private ThrownDestructableMovement movement;
    private bool combatActive = false;
    private bool isCombatTurn = false;
    private Vector3 targetPoint;

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

    private void Awake() {
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        movement = new ThrownDestructableMovement(gameObject, this, GameObject.FindWithTag("Grid").GetComponent<Grid>());
    }

    private void Start() {
        combatActive = GameState.combatState;
        if (!combatActive) MoveToTarget(transform.position, targetPoint);
    }

    // private void Update() {
        // if (!combatActive) rb.velocity = (transform.up - transform.right) * 250.0f * Time.deltaTime;
        // else rb.velocity = new Vector2();
        // if (Vector3.Magnitude(targetPoint - transform.position) <= .001f) Destroy(gameObject);
        // if (!combatActive) transform.position *= Time.deltaTime;
    // }

    private void OnDestroy() {
        // GameObject.Instantiate(aoeSpawn, targetPoint, new Quaternion());
    }

    public void TakeTurn() {
        isCombatTurn = true;
        StartCoroutine(TurnRoutine());
    }

    public void SetTargetPoint(Vector3 target) {
        this.targetPoint = target;
    }

    private void DestructableMove() {
        Vector3 endPoint = CloserVector(DirectionVector() * tileMovePerTurn, targetPoint);
        StartCoroutine(MoveToTarget(transform.position, endPoint));
    }

    private void EnableCombatFlag() {
        combatActive = true;
        StopAllCoroutines();
    }

    private void DisableCombatFlag() {
        combatActive = false;
        MoveToTarget(transform.position, targetPoint);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // ICanBeDamaged victim = other.gameObject.GetComponent<ICanBeDamaged>();
        // if (victim != null) {
        //     victim.TakeDamage(baseDamage);
        // }
        // if (!isContinuous) {
        Destroy(gameObject);
        // }
    }

    private Vector3 DirectionVector() {
        return (targetPoint - transform.position).normalized;
    }

    private Vector3 CloserVector(Vector3 vec1, Vector3 vec2) {
        float mag1 = Vector3.Magnitude(vec1 - transform.position);
        float mag2 = Vector3.Magnitude(vec2 - transform.position);
        if (mag1 < mag2) return vec1;
        return vec2;
    }

    public IEnumerator TurnRoutine() {
        DestructableMove();
        yield return null;
        EventManager.RaiseActorTurnOver();
        isCombatTurn = false;
    }

    private IEnumerator MoveToTarget(Vector3 start, Vector3 target) {
        float t = 0f;

        while(t < 1f){
            transform.position = Vector3.Lerp(start, target, t);
            t += Time.deltaTime * moveSpeed; // if you have a movespeed
            yield return null;
        }
    }
}
