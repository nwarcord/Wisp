using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : IAttack {

    private Transform actorPosition;
    private Projectile projectile;

    public RangedAttack(Transform actorPosition, Projectile projectile) {
        this.actorPosition = actorPosition;
        this.projectile = projectile;
    }

    public bool ExecuteAttack(Vector3 attackDirection) {
        
        Vector3 mousePosition = attackDirection;
        mousePosition.z = 0;
        Vector3 actorPos = actorPosition.position;
        actorPos.y -= 0.5f;

        Vector3 spawnPoint = (mousePosition - actorPos).normalized;
        spawnPoint += actorPos;

        if (spawnPoint == actorPos) return false; // Don't spawn on this actor
        if (Vector3.Magnitude(mousePosition - actorPos) < 0.5f) return false; // Extra check for spawn on actor

        Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, -45) * (spawnPoint - actorPos); // Direction to target

        Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget); // Rotation facing target

        Projectile p = GameObject.Instantiate(projectile, spawnPoint, targetRotation); // Spawn projectile
        
        return true;

    }

}
