using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using System.Linq;

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

    // public bool ExecuteAttack(Vector3 tileCoords) {
    
    //     Transform target = TileSystem.ObjectAtTile(tileCoords);
    //     if (target != null && rangeAugment.InRange(TileSystem.TileDistance(target.position, actorPosition.position))) {
    //         ICanBeDamaged victim = target.GetComponent<ICanBeDamaged>();
    //         if (victim != null) {
    //             victim.TakeDamage(damageAugment.ModifiedDmg(this.damage));
    //             return true;
    //         }

    //     }
    //     return false;

    // }

    public bool ExecuteAttack(Vector3 target) {
        Vector3 actorPos = actorPosition.position;
        Vector3 targetModified = target;
        targetModified.z = 0;
        Vector3 difference = (targetModified - actorPosition.position).normalized;
        // List<ICanBeDamaged> victims = new List<ICanBeDamaged>();
        // List<Collider2D> victims = new List<Collider2D>();
        Vector2 overlapSpawn = new Vector2();
        Vector2 overlapSize = new Vector2(1.5f, 1f);
        int overlapDegrees = 0;
        // If pointing up
        if (difference.y > 0 && (difference.x <= midPoint && difference.x >= -midPoint)) {
            Debug.Log("UP melee attack");
            overlapSpawn = new Vector2(actorPos.x, actorPos.y + 1);
            // victims = Physics2D.OverlapBoxAll(new Vector2(actorPos.x, actorPos.y + 1), new Vector2(1.5f, 1f), 0, LayerMask.GetMask("Characters")).OfType<Collider2D>().ToList();
        }
        // If pointing down
        else if (difference.y < 0 && ((difference.x <= midPoint && difference.x >= -midPoint))) {
            Debug.Log("DOWN melee attack");
            overlapSpawn = new Vector2(actorPos.x, actorPos.y - 1.6f);
            // victims = Physics2D.OverlapBoxAll(new Vector2(actorPos.x, actorPos.y - 1.6f), new Vector2(1.5f, 1f), 0, LayerMask.GetMask("Characters")).OfType<Collider2D>().ToList();
        }
        // If pointing right
        else if (difference.x > 0 && (difference.y <= midPoint && difference.y >= -midPoint)) {
            Debug.Log("RIGHT melee attack");
            overlapDegrees = 90;
            overlapSpawn = new Vector2(actorPos.x + 1.1f, actorPos.y);
            // victims = Physics2D.OverlapBoxAll(new Vector2(actorPos.x + 1.1f, actorPos.y), new Vector2(1.5f, 1f), 90, LayerMask.GetMask("Characters")).OfType<Collider2D>().ToList();
        }
        // If pointing left
        else {
            Debug.Log("LEFT melee attack");
            overlapDegrees = 90;
            overlapSpawn = new Vector2(actorPos.x - 1.1f, actorPos.y);
            // victims = Physics2D.OverlapBoxAll(new Vector2(actorPos.x - 1.1f, actorPos.y), new Vector2(1.5f, 1f), 90, LayerMask.GetMask("Characters")).OfType<Collider2D>().ToList();
        }
        bool gottaHit = false;
        Collider2D[] victims = Physics2D.OverlapBoxAll(overlapSpawn, overlapSize, overlapDegrees, LayerMask.GetMask("Characters"));
        foreach(Collider2D collider in victims) {
            ICanBeDamaged victim = collider.GetComponent<ICanBeDamaged>();
            if (victim != null) {
                victim.TakeDamage(damageAugment.ModifiedDmg(this.damage));
                gottaHit = true;
            }
        }
        return gottaHit;
    }

}
