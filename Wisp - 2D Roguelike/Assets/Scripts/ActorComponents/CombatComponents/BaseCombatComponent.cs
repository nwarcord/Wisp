﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCombatComponent : MonoBehaviour {

    public bool inCombat { get; private set; }
    private int attackPower;
    protected Grid grid;
    private BoxCollider2D boxCollider;
    protected Transform actorPosition;
    private const float oneTileMax = 1.42f; // Rounded root of 2

    private void Awake() {
        inCombat = false;
        actorPosition = gameObject.transform;
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        this.grid = GameState.grid;
        SetAttacks();
        // SetAttackPower();
    }

    // protected abstract void SetAttackPower();

    protected abstract void SetAttacks();

    public void EnterCombat() {
        if (!this.inCombat) {
            this.inCombat = true;
        }
        Debug.Log("COMBAT!!!!!!!!!!");
    }

    public void ExitCombat() {
        this.inCombat = false;
        Debug.Log("Combat over.");
    }

    // public void BasicAttack(ICanBeDamaged target) {
    //     target.TakeDamage(attackPower);
    // }

    // public bool OneTileAttack(Vector3 tileCoords) {
    //     return OneTileAttack(tileCoords, attackPower);
    // }

    // public bool OneTileAttack(Vector3 tileCoords, int damage) {

    //     Transform target = TileSystem.ObjectAtTile(tileCoords);
    //     if (target != null && TileSystem.TileDistance(target.position, actorPosition.position) <= 1) {
    //         ICanBeDamaged victim = target.GetComponent<ICanBeDamaged>();
    //         if (victim != null) {
    //             victim.TakeDamage(damage);
    //             return true;
    //         }   

    //     }
    //     return false;
    // }

    // public bool RangedAttack(Vector3 attackDirection, Projectile projectile) {

    //     Vector3 actorPos = actorPosition.position;
    //     actorPos.y -= 0.5f;
    //     Vector3Int gridDirection = grid.WorldToCell(attackDirection);
    //     Vector3Int actorTile = grid.WorldToCell(actorPosition.position);
    //     actorTile.y -= 1;
    //     gridDirection.z = actorTile.z;
    //     Vector3 spawnPoint = TileSystem.AdjacentTile(gridDirection, actorTile, actorPosition.position);
    //     spawnPoint.y -= 0.5f;

    //     Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, -45) * (spawnPoint - actorPos);

    //     Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);

    //     Projectile p = GameObject.Instantiate(projectile, spawnPoint, targetRotation);
    //     EventManager.RaiseCombatSpawn(p);
        
    //     return true;
    // }

}
