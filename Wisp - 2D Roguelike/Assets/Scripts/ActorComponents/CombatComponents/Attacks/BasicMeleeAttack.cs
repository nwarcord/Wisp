using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Not used anymore - left for reference only
public class BasicMeleeAttack {

    private const int damage = 1;
    private const int range = 1;
    private Transform actorPosition;

    public BasicMeleeAttack(Transform actorPosition) {
        this.actorPosition = actorPosition;
    }

    public bool ExecuteAttack(Vector3 tileCoords, int dmgMod, int rangeMod) {
    
        Transform target = TileSystem.ObjectAtTile(tileCoords);
        if (target != null && TileSystem.TileDistance(target.position, actorPosition.position) <= (range + rangeMod)) {
            ICanBeDamaged victim = target.GetComponent<ICanBeDamaged>();
            if (victim != null) {
                // victim.TakeDamage(damage + dmgMod);
                return true;
            }

        }
        return false;

    }

}