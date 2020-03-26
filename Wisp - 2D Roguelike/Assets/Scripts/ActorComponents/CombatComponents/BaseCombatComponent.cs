using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCombatComponent : MonoBehaviour {

    public bool inCombat { get; private set; }
    private int attackPower;
    protected Grid grid;
    private BoxCollider2D boxCollider;
    protected Transform actorPosition;
    private const float oneTileMax = 1.42f; // Rounded root of 2

    private void Awake() {
        inCombat = false;
        actorPosition = gameObject.transform;
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    private void Start() {
        this.grid = GameState.grid;
        SetAttacks();
    }

    protected abstract void SetAttacks();

    public void EnterCombat() {
        if (!this.inCombat) {
            this.inCombat = true;
        }
        Debug.Log("COMBAT!!!!!!!!!!");
    }

    public void ExitCombat() {
        this.inCombat = false;
        Debug.Log("Combat over.");
    }

    public abstract bool PerformAttack(Vector3 target, AttackType attackType);

}
