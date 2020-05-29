using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phantom : Enemy {

    [SerializeField]
    protected AudioClip meleeSound = default;

    protected override void SetHealth() {
        this.health = 4;
    }

    protected override void SetCombat() {
        this.combat = gameObject.GetComponent<PhantomCombatComponent>();
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

    protected override void CombatBehavior() {
        if (Vector3.Magnitude(myPosition.position - GetPlayerPosition()) <= 1.42f) {
            audioSource.PlayOneShot(meleeSound);
            meleeAttack.SpawnOrientation(myPosition.position, GetPlayerPosition());
            combat.PerformAttack(GetPlayerPosition(), AttackType.Melee);
            // combat.PerformAttack(Camera.main.WorldToScreenPoint(GetPlayerPosition()), AttackType.Melee);
        }
    }

    protected override void Patrol() {
        if (aIPath.reachedEndOfPath) {
            Vector3 newDest = new Vector3();
            newDest.x = myPosition.position.x + CustomHelpers.GetRandomValue(-2, 2);
            newDest.y = myPosition.position.y + CustomHelpers.GetRandomValue(-2, 2);
            aIPath.destination = newDest;
        }
    }

}
