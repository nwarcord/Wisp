using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enum for state choice? Patrol - Move - Attack (with type)

public abstract class BaseAIComponent : MonoBehaviour {

    protected Transform position;
    protected Transform target;
    protected Grid grid;
    protected List<AttackType> attacks;
    protected MovementComponent movement;
    protected BaseCombatComponent combat;

    private void Awake() {
        position = gameObject.transform;
        movement = gameObject.GetComponent<MovementComponent>();
        combat = gameObject.GetComponent<BaseCombatComponent>();
    }

    private void Start() {
        grid = GameState.grid;
        SetTarget();
    }

    protected abstract void SetTarget();

    public void Think(bool inCombat) {
        if (!inCombat) Patrol();
        else Engage();
    }

    public void UpdateTarget(Transform target) {
        this.target = target;
    }

    // Non-combat behavior
    private void Patrol() { movement.AttemptMove(DecideMove(false)); }

    // Combat behavior
    protected abstract void Engage();

    // Choosing where to move to
    protected abstract Vector3 DecideMove(bool inCombat);

    // If Attack, decide attack type
    protected abstract void DecideAttack();

}
