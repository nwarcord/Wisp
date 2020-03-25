using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour {

    protected int damage;
    protected Sprite attackIcon;
    protected string attackName;
    protected string description;
    protected int range;
    // Placeholder for animation?
    // Effects?
    // Range

    private void Awake() {
        InitVariables();
    }

    protected abstract void InitVariables();

    public abstract bool ExecuteAttack(Vector3 tileCoords, int modifier);

}

public enum AttackType {
    Melee,
    Ranged,
    Aoe
}

public interface IAttack {

    bool ExecuteAttack(Vector3 tileCoords);

}

// ----------------------------------------------------------------
// Melee attack augments
// ----------------------------------------------------------------

public class MeleeRangeAugment {

    public int totalRange;
    public bool continuous;

    public MeleeRangeAugment() {
        this.totalRange = 1;
        this.continuous = false;
    }

    public MeleeRangeAugment(int totalRange, bool continuous) {
        this.totalRange = totalRange;
        this.continuous = continuous;
    }

    // Copy constructor
    public MeleeRangeAugment(MeleeRangeAugment toCopy) {
        this.totalRange = toCopy.totalRange;
        this.continuous = toCopy.continuous;
    }

    public bool InRange(int distance) {
        return this.totalRange >= distance;
    }

}

public class DamageAugment {

    public int damageMod;
    
    public DamageAugment() {
        damageMod = 1;
    }

    public DamageAugment(int damageMod) {
        this.damageMod = damageMod;
    }

    // Copy constructor
    public DamageAugment(DamageAugment toCopy) {
        this.damageMod = toCopy.damageMod;
    }

    public int ModifiedDmg(int initialDmg) {
        return this.damageMod * initialDmg;
    }

}

public class MeleeCleaveAugment {

    public bool leftCleave;
    public bool rightCleave;

    public MeleeCleaveAugment() {
        this.leftCleave = false;
        this.rightCleave = false;
    }

    public MeleeCleaveAugment(bool leftCleave, bool rightCleave) {
        this.leftCleave = leftCleave;
        this.rightCleave = rightCleave;
    }

    // Copy constructor
    public MeleeCleaveAugment(MeleeCleaveAugment toCopy) {
        this.leftCleave = toCopy.leftCleave;
        this.rightCleave = toCopy.rightCleave;
    }

}

// ----------------------------------------------------------------
// Ranged attack augments
// ----------------------------------------------------------------

public class ProjectileAugment {

    public Projectile projectile;



}