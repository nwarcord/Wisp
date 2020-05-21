using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisp : Enemy {

    // ----------------------------------------------------------------
    // Initialization overrides
    // ----------------------------------------------------------------

    [SerializeField]
    protected AudioClip meleeSound;

    protected override void SetHealth() {
        this.health = 1;
    }

    protected override void SetCombat() {
        this.combat = gameObject.GetComponent<WispCombatComponent>();
    }

    protected override void SetVision() {
        this.vision = 4;
    }

    protected override void SetAI() {
        // Set AI here
        Vector3 newDest = new Vector3();
        newDest.x = myPosition.position.x + CustomHelpers.GetRandomValue(-2, 2);
        newDest.y = myPosition.position.y + CustomHelpers.GetRandomValue(-2, 2);
        aIPath.destination = newDest;
    }

    // ----------------------------------------------------------------
    // Turn mechanics
    // ----------------------------------------------------------------

    protected override void Patrol() {
        if (aIPath.reachedEndOfPath) {
            Vector3 newDest = new Vector3();
            newDest.x = myPosition.position.x + CustomHelpers.GetRandomValue(-2, 2);
            newDest.y = myPosition.position.y + CustomHelpers.GetRandomValue(-2, 2);
            aIPath.destination = newDest;
        }
    }

    protected override void CombatBehavior() {
        if (Vector3.Magnitude(myPosition.position - GetPlayerPosition()) <= 1.42f) {
            meleeAttack.SpawnOrientation(myPosition.position, GetPlayerPosition());
            audioSource.PlayOneShot(meleeSound);
            combat.PerformAttack(GetPlayerPosition(), AttackType.Melee);
            // combat.PerformAttack(Camera.main.WorldToScreenPoint(GetPlayerPosition()), AttackType.Melee);
        }
    }

}
