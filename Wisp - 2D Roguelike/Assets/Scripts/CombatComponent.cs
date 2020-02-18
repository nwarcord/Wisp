using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.Tilemaps;

public interface ICanBeDamaged {
    bool TakeDamage(int damage);
    bool IsAlive();
}

public class CombatComponent {
    
    public bool inCombat { get; private set; }
    // private int combatants;
    private int attackPower;
    private Grid grid;
    private BoxCollider2D boxCollider;
    private Transform actorPosition;
    private const float oneTileMax = 1.42f; // Rounded root of 2

    public CombatComponent(GameObject actor, int attackPower, Grid grid, BoxCollider2D boxCollider) {
        // this.combatants = 0;
        this.inCombat = false;
        this.attackPower = attackPower;
        this.grid = grid;
        this.boxCollider = boxCollider;
        this.actorPosition = actor.transform;
    }

    public void EnterCombat() {
        if (!this.inCombat) {
            this.inCombat = true;
        }
        Debug.Log("COMBAT!!!!!!!!!!");
    //     combatants += 1;
    }

    public void ExitCombat() {
    //     this.combatants -= 1;
    //     if (combatants <= 0) {
    //         this.combatants = 0;
        this.inCombat = false;
        Debug.Log("Combat over.");
    //     }
    }

    public bool BasicAttack(ICanBeDamaged target) {
        return target.TakeDamage(attackPower);
    }

    public bool OneTileAttack(Vector3 tileCoords) {
        return OneTileAttack(tileCoords, attackPower);
    }

    public bool OneTileAttack(Vector3 tileCoords, int damage) {

        Transform target = TileSystem.ObjectAtTile(tileCoords);
        if (target != null && TileSystem.TileDistance(target.position, actorPosition.position) <= 1) {
            ICanBeDamaged victim = target.GetComponent<ICanBeDamaged>();
            if (victim != null) {
                victim.TakeDamage(damage);
                return true;
            }   

        }
        return false;
    }

    public bool RangedAttack(Vector3 attackDirection, Projectile projectile) {

        Vector3 actorPos = actorPosition.position;
        actorPos.y -= 0.5f;
        Vector3Int gridDirection = grid.WorldToCell(attackDirection);
        Vector3Int actorTile = grid.WorldToCell(actorPosition.position);
        actorTile.y -= 1;
        gridDirection.z = actorTile.z;
        Vector3 spawnPoint = TileSystem.AdjacentTile(gridDirection, actorTile, actorPosition.position);
        spawnPoint.y -= 0.5f;

        Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, -45) * (spawnPoint - actorPos);

        Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);

        Projectile p = GameObject.Instantiate(projectile, spawnPoint, targetRotation);
        EventManager.RaiseCombatSpawn(p);
        
        return true;
    }

}
