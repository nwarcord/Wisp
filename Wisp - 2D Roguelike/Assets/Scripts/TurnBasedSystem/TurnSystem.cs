using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurnAct {
    void TakeTurn();
    IEnumerator TurnRoutine();
}

public class TurnSystem {

    private List<ITurnAct> actors;
    private int currentTurn = 0;
    private bool combatRunning = true;

    // ----------------------------------------------------------------
    // Initialization
    // ----------------------------------------------------------------

    public TurnSystem() {
        Debug.Log("Turn System Created");
        actors = new List<ITurnAct>();

        // Listeners
        EventManager.combatOver += EndCombat;
        EventManager.combatSpawn += InsertSpawn;
        EventManager.actorTurnOver += NextTurn;
        InitActors();

    }

    private void InitActors() {
        actors = FindInterfaces.Find<ITurnAct>();
        Debug.Log(actors.Count);
        for (int i = 0; i < actors.Count; i++) {
            Debug.Log("Actor " + i + ": " + actors[i]);
        }
    }

    // ----------------------------------------------------------------
    // Turn handling
    // ----------------------------------------------------------------

    public void NextTurn() {
        
        if (combatRunning) {
            
            if (actors[currentTurn] as UnityEngine.Object != null) {
                actors[currentTurn].TakeTurn();
            }
            currentTurn++;

            if (currentTurn >= actors.Count) {    
                currentTurn = 0;
                // Debug.Log("Pre-GC");
                // int removed = actors.RemoveAll(item => item as UnityEngine.Object == null); // GC for null objects
                // Debug.Log(removed + " items Garbage Collected");
            }


        }

        else { CombatOver(); }

    }

    private void InsertSpawn(ITurnAct spawn) {
        Debug.Log("Spawn insert");
        actors.Insert(currentTurn, spawn);
    }

    private void EndCombat() {
        combatRunning = false;
    }

    // ----------------------------------------------------------------
    // Clean up
    // ----------------------------------------------------------------

    private void CombatOver() {

        // Remove listeners
        EventManager.combatOver -= EndCombat;
        EventManager.combatSpawn -= InsertSpawn;
        EventManager.actorTurnOver -= NextTurn;

        // Broadcast that Combat has ended
        EventManager.RaiseCombatOver();

    }

}
