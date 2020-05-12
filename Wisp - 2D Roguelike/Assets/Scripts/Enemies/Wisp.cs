using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisp : Enemy {

    // ----------------------------------------------------------------
    // Frame to frame behavior
    // ----------------------------------------------------------------

    // void Update() {
    // }

    // ----------------------------------------------------------------
    // Initialization overrides
    // ----------------------------------------------------------------

    private Vector2 direction = Vector2.zero;

    protected override void SetHealth() {
        this.health = 1;
    }

    protected override void SetCombat() {
        this.combat = gameObject.GetComponent<WispCombatComponent>();
    }

    protected override void SetVision() {
        this.vision = 4;
    }

    protected override void SetAI() {
        // Set AI here
        // System.Random rand = new System.Random();
        Vector3 newDest = new Vector3();
        // newDest.x = myPosition.position.x + rand.Next(3);
        // newDest.y = myPosition.position.y + rand.Next(3);
        newDest.x = myPosition.position.x + CustomHelpers.GetRandomValue(-2, 2);
        newDest.y = myPosition.position.y + CustomHelpers.GetRandomValue(-2, 2);
        aIPath.destination = newDest;
    }

    // ----------------------------------------------------------------
    // Turn mechanics
    // ----------------------------------------------------------------

    protected override void Patrol() {
        if (aIPath.reachedEndOfPath) {
            // System.Random rand = new System.Random();
            Vector3 newDest = new Vector3();
            // newDest.x = myPosition.position.x + rand.Next(3) - 1;
            // newDest.y = myPosition.position.y + rand.Next(3) - 1;
            newDest.x = myPosition.position.x + CustomHelpers.GetRandomValue(-2, 2);
            newDest.y = myPosition.position.y + CustomHelpers.GetRandomValue(-2, 2);
            aIPath.destination = newDest;
            // Debug.Log(newDest);
        }
        // if (reachedEndOfPath) {
        //     System.Random rand = new System.Random();
        //     target.x = myPosition.position.x + rand.Next(3) - 1;
        //     target.y = myPosition.position.y + rand.Next(3) - 1;
        //     UpdateTarget(target);
        //     UpdatePath();
        // }
        // else if (MyTurn() && !reachedEndOfPath) {
        //     MoveOnPath();
        // }
    }

    protected override void CombatBehavior() {
        // UpdateTarget(GetPlayerPosition());

        if (Vector3.Magnitude(myPosition.position - GetPlayerPosition()) <= 1.42f) {
            combat.PerformAttack(Camera.main.WorldToScreenPoint(GetPlayerPosition()), AttackType.Melee);
        }
        // else if (!reachedEndOfPath){
        //     MoveOnPath();
        // }
        // else {
        //     UpdatePath();
        // }
        // Vector3 playerPosition = GetPlayerPosition();
        // playerPosition.y -= 0.5f;
        // Vector3 move = myPosition.position;
        // if (playerPosition.x != move.x) {
        //     if (playerPosition.x < move.x) { move.x -= 1; }
        //     else { move.x += 1; }
        // }

        // else if (playerPosition.y != move.y) {
        //     if (playerPosition.y < move.y) { move.y -= 1; }
        //     else { move.y += 1; }
        // }

        // if (playerPosition != move) { movement.AttemptMove(move); }

        // else if (Vector3.Magnitude(myPosition.position - playerPosition) <= 1.42f) {
        //     combat.PerformAttack(Camera.main.WorldToScreenPoint(playerPosition), AttackType.Melee);
        // }
    }

}
