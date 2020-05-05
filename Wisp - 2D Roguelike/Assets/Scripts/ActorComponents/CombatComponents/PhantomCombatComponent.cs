using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomCombatComponent : BaseCombatComponent {
    
    private MeleeAttack melee;
    [SerializeField]
    private AreaOfEffect poisonTrail;

    protected override void SetAttacks() {
        this.melee = new MeleeAttack(
            2,
            actorPosition,
            new MeleeRangeAugment(),
            new DamageAugment(),
            new MeleeCleaveAugment()
        );
    }

    public override bool PerformAttack(Vector3 target, AttackType attack) {
        if (attack == AttackType.Aoe) {
            GameObject.Instantiate(poisonTrail, transform.position, new Quaternion());
        }
        return melee.ExecuteAttack(target);
    }

}
