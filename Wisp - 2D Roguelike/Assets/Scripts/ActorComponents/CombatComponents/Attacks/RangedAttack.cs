using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : IAttack {

    private Transform actorPosition;
    private Projectile projectile;
    private Grid grid;

    public RangedAttack(Transform actorPosition, Projectile projectile, Grid grid) {
        this.actorPosition = actorPosition;
        this.projectile = projectile;
        this.grid = grid;
    }

    public bool ExecuteAttack(Vector3 attackDirection) {
    
        Vector3 actorPos = actorPosition.position;
        actorPos.y -= 0.5f;
        Vector3Int gridDirection = grid.WorldToCell(attackDirection);
        Vector3Int actorTile = grid.WorldToCell(actorPosition.position);
        actorTile.y -= 1;
        gridDirection.z = actorTile.z;
        Vector3 spawnPoint = TileSystem.AdjacentTile(gridDirection, actorTile, actorPosition.position);
        spawnPoint.y -= 0.5f;

        if (spawnPoint == actorPos) return false;

        Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, -45) * (spawnPoint - actorPos);

        Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);

        Projectile p = GameObject.Instantiate(projectile, spawnPoint, targetRotation);
        EventManager.RaiseCombatSpawn(p);
        
        return true;

    }

}
