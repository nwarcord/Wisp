using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomCombatComponent : BaseCombatComponent {
    
    private MeleeAttack melee;

    protected override void SetAttacks() {
        this.melee = new MeleeAttack(
            3,
            actorPosition,
            new MeleeRangeAugment(),
            new DamageAugment(),
            new MeleeCleaveAugment()
        );
    }

    public override bool PerformAttack(Vector3 target, AttackType attack) {
        return melee.ExecuteAttack(target);
    }

}
