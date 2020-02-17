using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using System.Threading;

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
        EventManager.playerLeftCombat += EndCombat;
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
        Debug.Log("Turn " + currentTurn);
        if (combatRunning) {

            if (currentTurn >= actors.Count) {    
                currentTurn = 0;
                actors.RemoveAll(item => item == null); // GC for null objects
            }

            if (actors[currentTurn] != null) {
                actors[currentTurn].TakeTurn();
            }
            currentTurn++;

        }

        else { CombatOver(); }

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

    private void CombatOver() {

        // Remove listeners
        EventManager.playerLeftCombat -= EndCombat;
        EventManager.combatSpawn -= InsertSpawn;
        EventManager.actorTurnOver -= NextTurn;

        // Broadcast that Combat has ended
        EventManager.RaiseCombatOver();

    }

}
