using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomCombatComponent : BaseCombatComponent {
    
    private MeleeAttack melee;
    // [SerializeField]
    // private AreaOfEffect poisonTrail = default;

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
        // if (attack == AttackType.Aoe) {
        //     AreaOfEffect aoe = GameObject.Instantiate(poisonTrail, target, new Quaternion());
        //     if (GameState.combatState) EventManager.RaiseCombatSpawn(aoe);
        //     return true;
        // }
        // else return melee.ExecuteAttack(target);
        return melee.ExecuteAttack(target);
    }

}
