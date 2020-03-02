using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeleeAttack : Attack {

    protected override void InitVariables() {
        this.attackName = "Basic Melee";
        this.damage = 1;
        this.description = "A basic melee attack.";
        this.range = 1;
    }

    public override bool ExecuteAttack(Vector3 tileCoords, int modifier) {
    
        Transform actorPosition = gameObject.transform;
        Transform target = TileSystem.ObjectAtTile(tileCoords);
        if (target != null && TileSystem.TileDistance(target.position, actorPosition.position) <= range) {
            ICanBeDamaged victim = target.GetComponent<ICanBeDamaged>();
            if (victim != null) {
                victim.TakeDamage(damage * modifier);
                return true;
            }   

        }
        return false;

    }

}
