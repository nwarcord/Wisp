using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurnAction {
    void TakeTurn();
}

public class TurnSystem {

    private List<ITurnAction> actors;
    private int currentTurn = 0;
    private bool combatRunning = true;

    // ----------------------------------------------------------------
    // Initialization
    // ----------------------------------------------------------------

    public TurnSystem() {
        
        actors = new List<ITurnAction>();
        InitActors();

        // Listeners
        // EventManager.playerLeftCombat += EndCombat;
        // EventManager.actorTurnOver += NextTurn;

    }

    private void InitActors() {
        actors = FindInterfaces.Find<ITurnAction>();
    }

    // ----------------------------------------------------------------
    // Turn handling
    // ----------------------------------------------------------------

    private void NextTurn() {

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

    private void EndCombat() {
        combatRunning = false;
    }

    // ----------------------------------------------------------------
    // Clean up
    // ----------------------------------------------------------------

    private void CombatOver() {

        // Remove listeners

        // EventManager.playerLeftCombat -= EndCombat;
        // EventManager.actorTurnOver -= NextTurn;

        // Broadcast that Combat has ended

        // EventManager.RaiseCombatOver();

    }

}
