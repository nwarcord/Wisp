using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour {

    private GameObject player;
    private TurnSystem turnSystem;
    public static Grid grid;
    public static bool combatState = false;
    private static int combatants = 0;

    private void OnEnable() {
        EventManager.aggroPlayer += InitTurnSystem;
        EventManager.combatOver += ClearTurnSystem;
        EventManager.enemyDeath += EnemyDeath;
        SceneManager.activeSceneChanged += UpdateGrid;
    }

    private void OnDisable() {
        EventManager.aggroPlayer -= InitTurnSystem;
        EventManager.combatOver -= ClearTurnSystem;
        EventManager.enemyDeath -= EnemyDeath;
        SceneManager.activeSceneChanged -= UpdateGrid;
    }

    void Awake() {
        player = GameObject.FindWithTag("Player");
        IgnoreSpawnerColliders();
        InitGrid();
        DontDestroyOnLoad(gameObject);
    }

    private void InitTurnSystem(ITurnAct enemy) {
        combatants++;
        if (combatState) {
            EventManager.RaiseCombatSpawn(enemy);
        }
        else {
            turnSystem = gameObject.AddComponent<TurnSystem>() as TurnSystem;
            EventManager.RaiseCombatStart();
            combatState = true;
            turnSystem.NextTurn();
        }
    }

    private void ClearTurnSystem() {
        if (combatState) {
            combatants = 0;
            combatState = false;
            Destroy(turnSystem);
            Debug.Log("Turn System deleted.");
        }
    }

    private void EnemyDeath() {
        if (combatState) {
            combatants--;
            if (combatants <= 0) EventManager.RaiseCombatOver();
        }
    }

    private void IgnoreSpawnerColliders() {
        Physics2D.IgnoreLayerCollision(11, 8);
        // Physics2D.IgnoreLayerCollision(11, 9);
        Physics2D.IgnoreLayerCollision(11, 10);
    }

    private void UpdateGrid(Scene current, Scene next) {
        grid = GameObject.Find("Grid").GetComponent<Grid>();
    }

    private void InitGrid() {
        grid = GameObject.Find("Grid").GetComponent<Grid>();
    }

}
