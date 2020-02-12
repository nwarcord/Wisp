using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurnAct {
    void TakeTurn();
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
        // InitActors();

        // Listeners
        EventManager.playerLeftCombat += EndCombat;
        EventManager.actorTurnOver += NextTurn;
        // EventManager.CheckActorTurnOver();
        EventManager.combatSpawn += InsertSpawn;
        InitActors();

    }

    private void InitActors() {
        actors = FindInterfaces.Find<ITurnAct>();
        NextTurn();
        Debug.Log(actors.Count);
        // EventManager.CheckActorTurnOver();
    }

    // ----------------------------------------------------------------
    // Turn handling
    // ----------------------------------------------------------------

    private void NextTurn() {
        Debug.Log("NextTurn!");
        if (combatRunning) {
            currentTurn++;

            if (currentTurn >= actors.Count) {    
                currentTurn = 0;
                actors.RemoveAll(item => item == null); // GC for null objects
            }

            actors[currentTurn].TakeTurn();
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
        EventManager.actorTurnOver -= NextTurn;
        EventManager.combatSpawn -= InsertSpawn;

        // Broadcast that Combat has ended
        EventManager.RaiseCombatOver();

    }

}
