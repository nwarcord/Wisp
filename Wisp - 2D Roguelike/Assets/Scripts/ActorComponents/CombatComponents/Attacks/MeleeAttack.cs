using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using System.Linq;

public class MeleeAttack : IAttack {

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

    public MeleeAttack(Transform actorPosition, int damage) {
        this.damage = damage;
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

    public bool ExecuteAttack(Vector3 target) {
        Vector3 actorPos = actorPosition.position;
        Vector3 targetModified = target;
        targetModified.z = 0;
        Vector3 difference = (targetModified - actorPosition.position).normalized;
        Vector2 overlapSpawn = new Vector2();
        Vector2 overlapSize = new Vector2(1.5f, 1f);
        int overlapDegrees = 0;
        
        // If pointing up
        if (difference.y > 0 && (difference.x <= midPoint && difference.x >= -midPoint)) {
            overlapSpawn = new Vector2(actorPos.x, actorPos.y + 0.5f);
        }
        // If pointing down
        else if (difference.y < 0 && ((difference.x <= midPoint && difference.x >= -midPoint))) {
            overlapSpawn = new Vector2(actorPos.x, actorPos.y - 1f);
        }
        // If pointing right
        else if (difference.x > 0 && (difference.y <= midPoint && difference.y >= -midPoint)) {
            overlapDegrees = 90;
            overlapSpawn = new Vector2(actorPos.x + 0.5f, actorPos.y);
        }
        // If pointing left
        else {
            overlapDegrees = 90;
            overlapSpawn = new Vector2(actorPos.x - 0.5f, actorPos.y);
        }

        bool gottaHit = false;
        // Get all actors in melee zone
        Collider2D[] victims = Physics2D.OverlapBoxAll(overlapSpawn, overlapSize, overlapDegrees, LayerMask.GetMask("Characters", "Player"));
        // If the actor isn't of same type as this actor, deal damage
        foreach(Collider2D collider in victims) {
            ICanBeDamaged victim = collider.GetComponent<ICanBeDamaged>();
            if (victim != null && collider.gameObject.tag != actorPosition.gameObject.tag) {
                // victim.TakeDamage(damageAugment.ModifiedDmg(this.damage));
                victim.TakeDamage(new AttackInfo(damageAugment.ModifiedDmg(this.damage)));
                gottaHit = true;
            }
        }
        return gottaHit;
    }

}
