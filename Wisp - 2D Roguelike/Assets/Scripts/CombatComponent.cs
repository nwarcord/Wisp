using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatComponent {
    
    private int health;
    private int attack;

    public CombatComponent(int health, int attack) {
        this.health = health;
        this.attack = attack;
    }

    public int BasicAttack() {
        return attack;
    }

    public void TakeDamage(int damage) {
        health -= damage;
    }

    public bool IsAlive() {
        return health > 0;
    }

}
