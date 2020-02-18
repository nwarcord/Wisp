using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

    private GameObject player;
    private TurnSystem turnSystem;
    public static bool combatState = false;
    private static int combatants = 0;
    // private bool turnSystemRunning = false;
    // private bool actorTurnOver = true;

    private void OnEnable() {
        EventManager.aggroPlayer += InitTurnSystem;
        // EventManager.playerEntersCombat += InitTurnSystem;
        EventManager.combatOver += ClearTurnSystem;
        EventManager.enemyDeath += EnemyDeath;
    }

    private void OnDisable() {
        EventManager.aggroPlayer -= InitTurnSystem;
        // EventManager.playerEntersCombat -= InitTurnSystem;
        EventManager.combatOver -= ClearTurnSystem;
        EventManager.enemyDeath -= EnemyDeath;
    }

    void Awake() {
        player = GameObject.FindWithTag("Player");
        IgnoreSpawnerColliders();
        DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        // if (actorTurnOver && turnSystemRunning) ProcessTurn();
        if (combatants == 0) EventManager.RaiseCombatOver();
    }

    private void InitTurnSystem(ITurnAct enemy) {
        combatants++;
        // if (turnSystem != null) {
        if (combatState) {
            EventManager.RaiseCombatSpawn(enemy);
        }
        else {
            turnSystem = new TurnSystem();
            combatState = true;
            turnSystem.NextTurn();
        }
        // actorTurnOver = false;
        // turnSystemRunning = true;
    }

    // private void ProcessTurn() {
        // actorTurnOver = false;
        // turnSystem.NextTurn();
        // actorTurnOver = true;
    // }

    private void ClearTurnSystem() {
        combatants = 0;
        combatState = false;
        turnSystem = null;
        // GameObject.Destroy(turnSystem);
        // turnSystemRunning = false;
    }

    private void EnemyDeath() {
        if (combatState) combatants--;
    }

    private void IgnoreSpawnerColliders() {
        Physics2D.IgnoreLayerCollision(11, 8);
        Physics2D.IgnoreLayerCollision(11, 9);
        Physics2D.IgnoreLayerCollision(11, 10);
    }

}
