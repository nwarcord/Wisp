using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobMonster : Enemy {

    // ----------------------------------------------------------------
    // Initialization overrides
    // ----------------------------------------------------------------

    protected override void SetHealth() {
        this.health = 2;
    }

    protected override void SetCombat() {
        this.combat = new CombatComponent(gameObject, 1, this.grid, this.boxCollider);
    }

    protected override void SetVision() {
        this.vision = 4;
    }

    // ----------------------------------------------------------------
    // Turn mechanics
    // ----------------------------------------------------------------

    protected override void Patrol() {
        if (PlayerVisible()) AggroPlayer();
        else {
            System.Random rand = new System.Random();
            Vector3 move = myPosition.position;
            int randX = rand.Next(3) - 1;
            int randY = rand.Next(3) - 1;
            move.x += randX;
            move.y += randY;
            movement.AttemptMove(move);
        }
    }

    protected override void CombatBehavior() {
        Vector3 playerPosition = GetPlayerPosition();
        playerPosition.y -= 0.5f;
        Vector3 move = myPosition.position;
        if (playerPosition.x != move.x) {
            if (playerPosition.x < move.x) { move.x -= 1; }
            else { move.x += 1; }
        }

        if (playerPosition.y != move.y) {
            if (playerPosition.y < move.y) { move.y -= 1; }
            else { move.y += 1; }
        }

        if (playerPosition != move) { movement.AttemptMove(move); }

        else if (Vector3.Magnitude(myPosition.position - playerPosition) <= 1.42f) {
            combat.OneTileAttack(Camera.main.WorldToScreenPoint(playerPosition));
        }
    }

}
