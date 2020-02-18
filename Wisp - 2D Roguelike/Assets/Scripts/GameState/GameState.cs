using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

    private GameObject player;
    private TurnSystem turnSystem;
    public static bool combatState = false;
    // private bool turnSystemRunning = false;
    // private bool actorTurnOver = true;

    private void OnEnable() {
        EventManager.playerEntersCombat += InitTurnSystem;
        EventManager.combatOver += ClearTurnSystem;
    }

    private void OnDisable() {
        EventManager.playerEntersCombat -= InitTurnSystem;
        EventManager.combatOver -= ClearTurnSystem;
    }

    void Awake() {
        player = GameObject.FindWithTag("Player");
        IgnoreSpawnerColliders();
        DontDestroyOnLoad(gameObject);
    }

    // private void Update() {
        // if (actorTurnOver && turnSystemRunning) ProcessTurn();
    // }

    private void InitTurnSystem() {
        if (turnSystem != null) return;
        turnSystem = new TurnSystem();
        combatState = true;
        turnSystem.NextTurn();
        // actorTurnOver = false;
        // turnSystemRunning = true;
    }

    // private void ProcessTurn() {
        // actorTurnOver = false;
        // turnSystem.NextTurn();
        // actorTurnOver = true;
    // }

    private void ClearTurnSystem() {
        turnSystem = null;
        combatState = false;
        // GameObject.Destroy(turnSystem);
        // turnSystemRunning = false;
    }

    private void IgnoreSpawnerColliders() {
        Physics2D.IgnoreLayerCollision(11, 8);
        Physics2D.IgnoreLayerCollision(11, 9);
        Physics2D.IgnoreLayerCollision(11, 10);
    }

}
