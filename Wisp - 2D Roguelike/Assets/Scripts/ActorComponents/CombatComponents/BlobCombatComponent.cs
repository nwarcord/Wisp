using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobCombatComponent : BaseCombatComponent {

    private MeleeAttack melee;
    
    protected override void SetAttacks() {
        melee = new MeleeAttack(actorPosition);
    }

    public bool PerformAttack(Vector3 target) {
        return melee.ExecuteAttack(target);
    }

}
