using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

            if (currentTurn >= actors.Count) {    
                currentTurn = 0;
                // actors.RemoveAll(item => (item == null || (item as UnityEngine.Object) == null)); // GC for null objects
                actors.RemoveAll(item => CustomHelpers.IsNullOrDestroyed(item)); // GC for null objects
            }
            Debug.Log("Turn " + currentTurn + " Actor: " + actors[currentTurn]);

            // if (actors[currentTurn] as UnityEngine.Object != null && actors[currentTurn] != null) {
            if (!CustomHelpers.IsNullOrDestroyed(actors[currentTurn])) {
                actors[currentTurn].TakeTurn();
                currentTurn++;
            }
            else {
                currentTurn++;
                EventManager.RaiseActorTurnOver();
            }


        }

        // else { CombatOver(); }

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

    // private void CombatOver() {


    //     // Broadcast that Combat has ended
    //     EventManager.RaiseCombatOver();

    // }

}
