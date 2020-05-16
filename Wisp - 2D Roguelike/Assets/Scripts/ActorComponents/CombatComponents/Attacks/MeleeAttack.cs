using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : IAttack {

    // private const int damage = 1;
    // private const int range = 1;
    private const float midPoint = 0.707f;
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

//     public bool ExecuteAttack(Vector3 target) {
//         Vector3 actorPos = actorPosition.position;
//         Vector3 targetModified = target;
//         targetModified.z = 0;
//         Vector3 difference = (targetModified - actorPosition.position).normalized;
//         List<ICanBeDamaged> victims = new List<ICanBeDamaged>();
//         // If pointing up
//         if (difference.y > 0 && (difference.x <= midPoint && difference.x >= -midPoint)) {
//             victims.Add(TileSystem.ObjectAtTile(new Vector3(actorPos.x + 1, actorPos.y + 1, 0)).GetComponent<ICanBeDamaged>());
//             victims.Add(TileSystem.ObjectAtTile(new Vector3(actorPos.x, actorPos.y + 1, 0)).GetComponent<ICanBeDamaged>());
//             victims.Add(TileSystem.ObjectAtTile(new Vector3(actorPos.x - 1, actorPos.y + 1, 0)).GetComponent<ICanBeDamaged>());
//         }
//         // If pointing down
//         else if (difference.y < 0 && ((difference.x <= midPoint && difference.x >= -midPoint))) {
//             victims.Add(TileSystem.ObjectAtTile(new Vector3(actorPos.x + 1, actorPos.y - 1, 0)).GetComponent<ICanBeDamaged>());
//             victims.Add(TileSystem.ObjectAtTile(new Vector3(actorPos.x, actorPos.y - 1, 0)).GetComponent<ICanBeDamaged>());
//             victims.Add(TileSystem.ObjectAtTile(new Vector3(actorPos.x - 1, actorPos.y - 1, 0)).GetComponent<ICanBeDamaged>());
//         }
//         // If pointing right
//         else if (difference.x > 0 && (difference.y <= midPoint && difference.y >= -midPoint)) {
//             victims.Add(TileSystem.ObjectAtTile(new Vector3(actorPos.x + 1, actorPos.y + 1, 0)).GetComponent<ICanBeDamaged>());
//             victims.Add(TileSystem.ObjectAtTile(new Vector3(actorPos.x + 1, actorPos.y, 0)).GetComponent<ICanBeDamaged>());
//             victims.Add(TileSystem.ObjectAtTile(new Vector3(actorPos.x + 1, actorPos.y - 1, 0)).GetComponent<ICanBeDamaged>());
//         }
//         else {
//             victims.Add(TileSystem.ObjectAtTile(new Vector3(actorPos.x - 1, actorPos.y + 1, 0)).GetComponent<ICanBeDamaged>());
//             victims.Add(TileSystem.ObjectAtTile(new Vector3(actorPos.x - 1, actorPos.y, 0)).GetComponent<ICanBeDamaged>());
//             victims.Add(TileSystem.ObjectAtTile(new Vector3(actorPos.x - 1, actorPos.y - 1, 0)).GetComponent<ICanBeDamaged>());
//         }
//         bool gottaHit = false;
//         foreach(ICanBeDamaged victim in victims) {
//             if (victim != null) {
//                 victim.TakeDamage(damageAugment.ModifiedDmg(this.damage));
//                 gottaHit = true;
//             }
//         }
//         return gottaHit;
//     }

}
