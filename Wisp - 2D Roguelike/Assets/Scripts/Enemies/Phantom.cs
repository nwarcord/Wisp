using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phantom : Enemy {

    protected override void SetHealth() {
        this.health = 4;
    }

    protected override void SetCombat() {
        this.combat = gameObject.GetComponent<PhantomCombatComponent>();
    }

    protected override void SetVision() {
        this.vision = 3;
    }

    protected override void SetAI() {
        // Set AI here
    }

    protected override void CombatBehavior() {

        Vector3 playerPosition = GetPlayerPosition();
        // Vector3 move = CustomHelpers.Pathfinding(myPosition.position, playerPosition);
        // playerPosition.y -= 0.5f;
        Vector3 move = myPosition.position;
        move.y -= 0.5f;
        // Vector3 currentPos = myPosition.position;
        // currentPos.y -= 0.5f;
        if (playerPosition.x != move.x) {
            if (playerPosition.x < move.x) { move.x -= 1; }
            else { move.x += 1; }
        }

        else if (playerPosition.y != move.y) {
            if (playerPosition.y < move.y) { move.y -= 1; }
            else { move.y += 1; }
        }

        if (playerPosition != move) {
            // if (movement.AttemptMove(move)) {
            //     combat.PerformAttack(currentPos, AttackType.Aoe); 
            // }
        // if (move != myPosition.position) {
            movement.AttemptMove(move);
        // }
        }

        else if (Vector3.Magnitude(myPosition.position - playerPosition) <= 1.42f) {
            combat.PerformAttack(Camera.main.WorldToScreenPoint(playerPosition), AttackType.Melee);
        }

    }

    protected override void Patrol() {
        if (PlayerVisible()) AggroPlayer();
        else {
            System.Random rand = new System.Random();
            Vector3 move = myPosition.position;
            // Vector3 currentPos = myPosition.position;
            // currentPos.y -= 0.5f;
            int randX = rand.Next(3) - 1;
            int randY = rand.Next(3) - 1;
            if (randX != 0) randY = 0;
            move.x += randX;
            move.y += randY;
            // if (movement.AttemptMove(move)) {
            //     combat.PerformAttack(currentPos, AttackType.Aoe);
            // }
            movement.AttemptMove(move);
        }
    }

}
