using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : IAttack {

    // private const int damage = 1;
    // private const int range = 1;
    private int damage = 1;
    private Transform actorPosition;
    // TODO: Use augments and make ability to update them
    private MeleeRangeAugment rangeAugment;
    private DamageAugment damageAugment;
    private MeleeCleaveAugment cleaveAugment;

    public MeleeAttack(Transform actorPosition) {
        this.actorPosition = actorPosition;
        rangeAugment = new MeleeRangeAugment();
        damageAugment = new DamageAugment();
        cleaveAugment = new MeleeCleaveAugment();
    }

    public MeleeAttack(int damage, Transform actorPosition, MeleeRangeAugment rangeAugment, DamageAugment damageAugment, MeleeCleaveAugment cleaveAugment) {
        this.damage = damage;
        this.actorPosition = actorPosition;
        this.rangeAugment = new MeleeRangeAugment(rangeAugment);
        this.damageAugment = new DamageAugment(damageAugment);
        this.cleaveAugment = new MeleeCleaveAugment(cleaveAugment);
    }

    public bool ExecuteAttack(Vector3 tileCoords) {
    
        Transform target = TileSystem.ObjectAtTile(tileCoords);
        if (target != null && rangeAugment.InRange(TileSystem.TileDistance(target.position, actorPosition.position))) {
            ICanBeDamaged victim = target.GetComponent<ICanBeDamaged>();
            if (victim != null) {
                victim.TakeDamage(damageAugment.ModifiedDmg(this.damage));
                return true;
            }

        }
        return false;

    }

    // public bool ExecuteAttack(Vector3 target) {
    //     // if (Vector2.SqrMagnitude(target - actorPosition.position) <= Mathf.Pow(rangeAugment.totalRange, 2)) {
    //     if (rangeAugment.InRangeSqr(Vector2.SqrMagnitude(target - actorPosition.position))) {
    //         ICanBeDamaged victim = RayLinecastTools.ObjectAtCoords(target).GetComponent<ICanBeDamaged>();
    //         if (victim != null) {
    //             victim.TakeDamage(damageAugment.ModifiedDmg(this.damage));
    //             return true;
    //         }
    //     }
    //     return false;
    // }

}
