using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : IAttack {

    private Transform actorPosition;
    private Projectile projectile;

    // public RangedAttack(Transform actorPosition, Projectile projectile, Grid grid) {
    //     this.actorPosition = actorPosition;
    //     this.projectile = projectile;
    //     this.grid = grid;
    // }

    public RangedAttack(Transform actorPosition, Projectile projectile) {
        this.actorPosition = actorPosition;
        this.projectile = projectile;
    }

    public bool ExecuteAttack(Vector3 attackDirection) {
        
        // Debug.Log("Attack direction: " + attackDirection);

        Vector3 mousePosition = attackDirection;
        mousePosition.z = 0;
        Vector3 actorPos = actorPosition.position;
        actorPos.y -= 0.5f;
        // Vector3Int gridDirection = grid.WorldToCell(attackDirection);
        // Vector3Int actorTile = grid.WorldToCell(actorPosition.position);
        // actorTile.y -= 1;
        // gridDirection.z = actorTile.z;
        // Vector3 spawnPoint = TileSystem.AdjacentTile(gridDirection, actorTile, actorPosition.position);
        // spawnPoint.y -= 0.5f;
        
        // Vector3 spawnPoint = (attackDirection - actorPos).normalized;
        // spawnPoint *= 3.5f;
        // spawnPoint += actorPos;

        Vector3 spawnPoint = (mousePosition - actorPos).normalized;
        // spawnPoint *= 1.5f;
        spawnPoint += actorPos;

        if (spawnPoint == actorPos) return false;
        if (Vector3.Magnitude(mousePosition - actorPos) < 0.5f) return false;

        Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, -45) * (spawnPoint - actorPos);

        Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);

        Projectile p = GameObject.Instantiate(projectile, spawnPoint, targetRotation);
        
        return true;

    }

}
