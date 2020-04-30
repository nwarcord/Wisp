using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enum for state choice? Patrol - Move - Attack (with type)

public abstract class BaseAIComponent : MonoBehaviour {

    protected Transform position;
    protected Transform target;
    protected Grid grid;

    private void Awake() {
        position = gameObject.transform;
    }

    private void Start() {
        grid = GameState.grid;
        SetTarget();
    }

    protected abstract void SetTarget();

    // Non-combat behavior

    // Combat behavior

    // Choosing which state to move into

    // If Patrol, decide number of tiles and direction

    // If Move, decide number of tiles and direction (using target)

    // If Attack, decide attack type

}
