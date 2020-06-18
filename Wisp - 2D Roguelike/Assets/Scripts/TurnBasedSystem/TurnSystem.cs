using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ----------------------------------------------------------------
// Obsolete TurnSystem class - kept for reference
// ----------------------------------------------------------------

public interface ITurnAct {
    void TakeTurn();
    IEnumerator TurnRoutine();
}

public class TurnSystem : MonoBehaviour {

    private List<ITurnAct> actors;
    private int currentTurn = 0;
    private bool combatRunning = true;

    // ----------------------------------------------------------------
    // Initialization
    // ----------------------------------------------------------------

    private void OnEnable() {
        // Listeners
        EventManager.combatOver += EndCombat;
        EventManager.combatSpawn += InsertSpawn;
        EventManager.actorTurnOver += NextTurn;
    }

    private void OnDisable() {
        // Remove listeners
        EventManager.combatOver -= EndCombat;
        EventManager.combatSpawn -= InsertSpawn;
        EventManager.actorTurnOver -= NextTurn;
    }

    private void Awake() {
        Debug.Log("Turn System Created");
        actors = new List<ITurnAct>();
        InitActors();
    }

    private void InitActors() {
        actors = FindInterfaces.Find<ITurnAct>();
    }

    // ----------------------------------------------------------------
    // Turn handling
    // ----------------------------------------------------------------

    public void NextTurn() {
        if (actors.Count <= 1) EndCombat();
        if (combatRunning) {

            if (currentTurn >= actors.Count) {    
                currentTurn = 0;
                actors.RemoveAll(item => CustomHelpers.IsNullOrDestroyed(item)); // GC for null objects
            }

            // Debug.Log("Actor Turn: " + actors[currentTurn] + " at turn num: " + currentTurn + " with total actors: " + actors.Count);

            if (!CustomHelpers.IsNullOrDestroyed(actors[currentTurn])) {
                actors[currentTurn].TakeTurn();
                currentTurn++;
            }
            else {
                currentTurn++;
                EventManager.RaiseActorTurnOver();
            }


        }

    }

    private void InsertSpawn(ITurnAct spawn) {
        actors.Insert(currentTurn, spawn);
    }

    private void EndCombat() {
        combatRunning = false;
    }

    // ----------------------------------------------------------------
    // Clean up
    // ----------------------------------------------------------------

    // private void CombatOver() {
    //     // Broadcast that Combat has ended
    //     EventManager.RaiseCombatOver();
    // }

}
