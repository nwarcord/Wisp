﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatComponent : BaseCombatComponent {

    private MeleeAttack melee;
    private RangedAttack ranged;
    private RangedAttack thrown;
    [SerializeField]
    private Projectile projectile = default;
    [SerializeField]
    private Projectile thrownProjectile = default;

    protected override void SetAttacks() {

        melee = new MeleeAttack(this.actorPosition, 2);
        ranged = new RangedAttack(this.actorPosition, projectile);
        thrown = new RangedAttack(this.actorPosition, thrownProjectile);
    }

    public override bool PerformAttack(Vector3 target, AttackType attackType) {
        
        if (attackType == AttackType.Melee) { melee.ExecuteAttack(target); return true; }

        else if (attackType == AttackType.Ranged) { ranged.ExecuteAttack(target); return true; }

        else if (attackType == AttackType.Thrown) { thrown.ExecuteAttack(target); return true; }

        else return false;

    }

}
