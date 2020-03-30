using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffect : MonoBehaviour, ITurnAct {

    [SerializeField]
    private int areaSquare;
    [SerializeField]
    private int duration; // Seconds or turns
    [SerializeField]
    private int damagePerTick;
    private BoxCollider2D boxCollider;
    private bool combatActive = false;
    private bool isCombatTurn = false;

    private void Awake() {
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
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
        Collider[] hitColliders = Physics.OverlapBox(transform.position, halfArea, Quaternion.identity, LayerMask.GetMask("Characters"));
        foreach (Collider actor in hitColliders) {
            ICanBeDamaged victim = actor.gameObject.GetComponent<ICanBeDamaged>();
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
    }

    private void TickDuration() {
        duration--;
        if (duration <= 0) Destroy(gameObject);
    }

}
