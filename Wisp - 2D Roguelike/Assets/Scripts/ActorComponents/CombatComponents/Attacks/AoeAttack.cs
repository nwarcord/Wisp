using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeAttack : IAttack {

    private Transform actorPosition;
    // private AoeAugment augment;
    private ThrownDestructable thrownDestructable;
    private Grid grid;
    private AudioSource attackSound;

    public AoeAttack(Transform actorPosition, ThrownDestructable thrownDestructable, /*AoeAugment augment,*/ Grid grid, AudioSource attackSound) {

        this.actorPosition = actorPosition;
        // this.augment = augment;
        this.thrownDestructable = thrownDestructable;
        this.grid = grid;
        this.attackSound = attackSound;

    }

    public void UpdateAugment(/*AoeAugment newAugment*/) {
        // this.augment = newAugment;
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

        ThrownDestructable t = GameObject.Instantiate(thrownDestructable, spawnPoint, new Quaternion());
        
        return true;
    }

}
