using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeAttack : IAttack {

    private Transform actorPosition;
    // private AoeAugment augment;
    private Grid grid;

    public AoeAttack(Transform actorPosition, /*AoeAugment augment,*/ Grid grid) {

        this.actorPosition = actorPosition;
        // this.augment = augment;
        this.grid = grid;

    }

    public void UpdateAugment(/*AoeAugment newAugment*/) {
        // this.augment = newAugment;
    }

    public bool ExecuteAttack(Vector3 target) {
        return false;
    }

}
