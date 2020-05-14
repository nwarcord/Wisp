using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatComponent : BaseCombatComponent {

    private MeleeAttack melee;
    private RangedAttack ranged;
    // private AoeAttack aoe;
    private RangedAttack thrown;
    [SerializeField]
    private Projectile projectile = default;
    [SerializeField]
    private Projectile thrownProjectile = default;

    protected override void SetAttacks() {

        melee = new MeleeAttack(this.actorPosition);
        ranged = new RangedAttack(this.actorPosition, projectile);
        // aoe = new AoeAttack(this.actorPosition, thrown, this.grid);
        thrown = new RangedAttack(this.actorPosition, thrownProjectile);
    }

    public override bool PerformAttack(Vector3 target, AttackType attackType) {
        
        if (attackType == AttackType.Melee) { melee.ExecuteAttack(target); return true; }

        else if (attackType == AttackType.Ranged) { ranged.ExecuteAttack(target); return true; }

        else if (attackType == AttackType.Thrown) { thrown.ExecuteAttack(target); return true; }

        else return false;

    }

}
