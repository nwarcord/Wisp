using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidseerCombatComponent : BaseCombatComponent {

    private MeleeAttack melee;
    private RangedAttack ranged;
    [SerializeField]
    private Projectile projectile = default;

    protected override void SetAttacks() {

        this.melee = new MeleeAttack(
            3,
            actorPosition,
            new MeleeRangeAugment(),
            new DamageAugment(),
            new MeleeCleaveAugment()
        );
        ranged = new RangedAttack(this.actorPosition, projectile);

    }

    public override bool PerformAttack(Vector3 target, AttackType attackType) {
        if (attackType == AttackType.Melee) return melee.ExecuteAttack(target);
        else return ranged.ExecuteAttack(target);
    }

}
