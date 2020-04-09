using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffect : MonoBehaviour, ITurnAct {

    [SerializeField]
    private int areaSquare = 0;
    [SerializeField]
    private int duration = 0; // Seconds or turns
    [SerializeField]
    private int damagePerTick = 0;
    private BoxCollider2D boxCollider;
    private bool combatActive = false;
    private bool isCombatTurn = false;

    private void Awake() {
        combatActive = GameState.combatState;
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        if (!combatActive) StartCoroutine(NonCombatBehavior());
    }

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

    public void TakeTurn() {
        StartCoroutine(TurnRoutine());
    }

    public IEnumerator TurnRoutine() {
        DealDamage();
        yield return null;
        EventManager.RaiseActorTurnOver();
        TickDuration();
    }

    private IEnumerator NonCombatBehavior() {
        DealDamage();
        yield return new WaitForSeconds(1f);
        TickDuration();
    }

    private void DealDamage() {
        Vector3 halfArea = new Vector3(areaSquare / 2, areaSquare / 2, 0);
        Collider2D[] hitColliders2d = new Collider2D[10];
        ContactFilter2D layer = new ContactFilter2D();
        layer.SetLayerMask(LayerMask.GetMask("Characters"));
        int victims = Physics2D.OverlapBox(transform.position, halfArea, 0, layer, hitColliders2d);
        // Collider[] hitColliders = Physics.OverlapBox(transform.position, halfArea, Quaternion.identity, LayerMask.GetMask("Characters"));
        Debug.Log("Actors on puddle = " + victims);
        // foreach (Collider2D actor in hitColliders2d) {
        for (int i = 0; i < victims; i++) {
            ICanBeDamaged victim = hitColliders2d[i].gameObject.GetComponent<ICanBeDamaged>();
            if (victim != null) victim.TakeDamage(damagePerTick);
        }
    }

    private void EnableCombatFlag() {
        combatActive = true;
        StopAllCoroutines();
    }

    private void DisableCombatFlag() {
        combatActive = false;
        StopAllCoroutines();
        StartCoroutine(NonCombatBehavior()); // Could casuse problems????
    }

    private void TickDuration() {
        duration--;
        if (duration <= 0) Destroy(gameObject);
        else if (!combatActive) StartCoroutine(NonCombatBehavior());
    }

}
