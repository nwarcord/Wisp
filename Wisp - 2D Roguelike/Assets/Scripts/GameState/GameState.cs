using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

    private GameObject player;
    private TurnSystem turnSystem;

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

    private void InitTurnSystem() {
        if (turnSystem != null) return;
        turnSystem = new TurnSystem();
    }

    private void ClearTurnSystem() {
        turnSystem = null;
    }

    private void IgnoreSpawnerColliders() {
        Physics2D.IgnoreLayerCollision(11, 8);
        Physics2D.IgnoreLayerCollision(11, 9);
        Physics2D.IgnoreLayerCollision(11, 10);
    }

}
