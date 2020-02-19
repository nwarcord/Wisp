﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, ICanBeDamaged {

    private int health;
    private CombatComponent combat;
    private MovementComponent movement;
    private BoxCollider2D boxCollider;
    private CircleCollider2D circleCollider;
    [SerializeField]
    private Grid grid = default;
    private int frames = 0;

    // ----------------------------------------------------------------
    // Event subscribe and unsubscribe
    // ----------------------------------------------------------------

    private void OnEnable() {
    //     EventManager.enemyDeath += CheckIfCombatOver;
        EventManager.combatStart += PlayerEnterCombat;
        EventManager.combatOver += PlayerLeaveCombat;
    }

    private void OnDisable() {
    //     EventManager.enemyDeath -= CheckIfCombatOver;
        EventManager.combatStart -= PlayerEnterCombat;
        EventManager.combatOver -= PlayerLeaveCombat;
    }

    // ----------------------------------------------------------------
    // Initialization
    // ----------------------------------------------------------------

    void Awake() {
        health = 3;
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        circleCollider = gameObject.GetComponent<CircleCollider2D>();
        combat = new CombatComponent(gameObject, 2, this.grid, this.boxCollider);
        movement = new MovementComponent(gameObject, this, grid);
    }

    private void Update() {
        frames++;
        if (frames >= 240) {
            frames = 0;
            Debug.Log("Hi from Player Controller!");
        }
    }

    public CombatComponent Combat() {
        return this.combat;
    }

    // ----------------------------------------------------------------
    // Combat mechanics
    // ----------------------------------------------------------------

    public void PlayerEnterCombat() {
        // if (!combat.inCombat) EventManager.RaisePlayerEntersCombat();
        combat.EnterCombat();
    }

    public void PlayerLeaveCombat() {
        combat.ExitCombat();
        Debug.Log("Player left combat");
    }

    // public void CheckIfCombatOver() {
    //     if (!combat.inCombat) return;
    //     combat.ExitCombat();
    //     if (!combat.inCombat) EventManager.RaisePlayerLeftCombat();
    // }

    public void TakeDamage(int damage) {
        health -= damage;
        Debug.Log("Player health: " + this.health + " | Damage taken: " + damage);
        // return true;
    }

    public bool IsAlive() {
        return health > 0;
    }

    // ----------------------------------------------------------------
    // Turn mechanics
    // ----------------------------------------------------------------

    public MovementComponent GetMovement() {
        return movement;
    }

}
