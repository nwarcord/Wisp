using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanBeDamaged {
    
    void TakeDamage(AttackInfo attackInfo);
    // void TakeDamage(int damage);
    bool IsAlive();

}

public class AttackInfo {
    public int damage { get; private set; }
    public Vector3 pointOfHit { get; private set; }

    public AttackInfo(int damage) {
        this.damage = damage;
        this.pointOfHit = new Vector3();
    }

    public AttackInfo(int damage, Vector3 pointOfHit) {
        this.damage = damage;
        this.pointOfHit = pointOfHit;
    }
}

public abstract class BaseCombatComponent : MonoBehaviour {

    public bool inCombat { get; private set; }
    private int attackPower;
    private BoxCollider2D boxCollider;
    protected Transform actorPosition;
    private const float oneTileMax = 1.42f; // Rounded root of 2

    private void Awake() {
        inCombat = false;
        actorPosition = gameObject.transform;
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    private void Start() {
        SetAttacks();
    }

    protected abstract void SetAttacks();

    public void EnterCombat() {
        if (!this.inCombat) {
            this.inCombat = true;
        }
    }

    public void ExitCombat() {
        this.inCombat = false;
    }

    public abstract bool PerformAttack(Vector3 target, AttackType attackType);

}
