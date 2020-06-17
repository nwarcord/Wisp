using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCombatComponent : MonoBehaviour {

    public bool inCombat { get; private set; }
    private int attackPower;
    private BoxCollider2D boxCollider;
    protected Transform actorPosition;
    private const float oneTileMax = 1.42f; // Rounded root of 2

    private void Awake() {
        inCombat = false;
        actorPosition = gameObject.transform;
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    private void Start() {
        SetAttacks();
    }

    protected abstract void SetAttacks();

    public void EnterCombat() {
        if (!this.inCombat) {
            this.inCombat = true;
        }
    }

    public void ExitCombat() {
        this.inCombat = false;
    }

    public abstract bool PerformAttack(Vector3 target, AttackType attackType);

}
