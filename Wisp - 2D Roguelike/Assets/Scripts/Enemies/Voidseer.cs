using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voidseer : Enemy {

    [SerializeField]
    protected AudioClip gunshot = default;

    protected override void SetHealth() {
        this.health = 5;
    }

    protected override void SetCombat() {
        this.combat = gameObject.GetComponent<VoidseerCombatComponent>();
    }

    protected override void SetAI() {
        Vector3 newDest = new Vector3();
        newDest.x = myPosition.position.x + CustomHelpers.GetRandomValue(-2, 2);
        newDest.y = myPosition.position.y + CustomHelpers.GetRandomValue(-2, 2);
        aIPath.destination = newDest;
    }

    protected override void SetVision() {
        this.vision = 6;
    }

    protected override void Patrol() {
        if (aIPath.reachedEndOfPath) {
            Vector3 newDest = new Vector3();
            newDest.x = myPosition.position.x + CustomHelpers.GetRandomValue(-2, 2);
            newDest.y = myPosition.position.y + CustomHelpers.GetRandomValue(-2, 2);
            aIPath.destination = newDest;
        }
    }

    protected override void CombatBehavior() {
        
        if (aIPath.reachedEndOfPath) {    
            aIPath.endReachedDistance = 5;
            aIPath.pickNextWaypointDist = 5;
        }

        if (Vector3.Magnitude(myPosition.position - GetPlayerPosition()) <= 1.42f) {
            meleeAttack.SpawnOrientation(myPosition.position, GetPlayerPosition());
            combat.PerformAttack(GetPlayerPosition(), AttackType.Melee);
            // combat.PerformAttack(Camera.main.WorldToScreenPoint(GetPlayerPosition()), AttackType.Melee);
        }
        else {
            audioSource.PlayOneShot(gunshot);
            combat.PerformAttack(GetPlayerPosition(), AttackType.Ranged);
        }
    }

}
