using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurnComponent {
    bool isTurn { get; private set; }
    bool turnSystemActive { get; private set; }
    InputDelay inputDelay { get; private set; }
    void TakeTurn();
    bool MyTurn();
    IEnumerator TurnRoutine();
}

public abstract class TurnComponent : MonoBehaviour {

    protected bool isTurn = false;
    protected bool turnSystemActive = false;
    
    private void OnEnable() {
        EventManager.combatStart += TurnSystemIsActive;
        EventManager.combatOver += TurnSystemNotActive;
    }

    private void OnDisable() {
        EventManager.combatStart -= TurnSystemIsActive;
        EventManager.combatOver -= TurnSystemNotActive;
        StopAllCoroutines();
        if (isTurn) EventManager.RaiseActorTurnOver();
    }

    private void Awake() {
        turnSystemActive = GameState.combatState;
    }

    private void TurnSystemIsActive() {
        turnSystemActive = true;
    }

    private void TurnSystemNotActive() {
        turnSystemActive = false;
    }

    protected abstract void TurnBehavior();

    public void TakeTurn() {
        isTurn = true;
        StartCoroutine(TurnRoutine());
    }

    private IEnumerator TurnRoutine() {
        TurnBehavior();
        yield return null;
        EventManager.RaiseActorTurnOver();
        isTurn = false;
    }

}