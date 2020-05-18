using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameState : MonoBehaviour {

    private GameObject player;
    // private TurnSystem turnSystem;
    public static Grid grid;
    public static bool combatState = false;
    private static int combatants = 0;
    [SerializeField]
    private GameObject objectivePrompt =  default;

    private void OnEnable() {
        // EventManager.aggroPlayer += InitTurnSystem;
        EventManager.aggroPlayer += CombatEnabled;
        // EventManager.combatOver += ClearTurnSystem;
        EventManager.enemyDeath += EnemyDeath;
        SceneManager.activeSceneChanged += UpdateGrid;
    }

    private void OnDisable() {
        // EventManager.aggroPlayer -= InitTurnSystem;
        EventManager.aggroPlayer -= CombatEnabled;
        // EventManager.combatOver -= ClearTurnSystem;
        EventManager.enemyDeath -= EnemyDeath;
        SceneManager.activeSceneChanged -= UpdateGrid;
    }

    void Awake() {
        player = GameObject.FindWithTag("Player");
        objectivePrompt.GetComponent<Image>().enabled = false;
        IgnoreSpawnerColliders();
        InitGrid();
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        StartCoroutine(ImageDelayAndShow(objectivePrompt));
    }

    private void InitTurnSystem(ITurnAct enemy) {
        combatants++;
        if (combatState) {
            EventManager.RaiseCombatSpawn(enemy);
        }
        else {
            // turnSystem = gameObject.AddComponent<TurnSystem>() as TurnSystem;
            EventManager.RaiseCombatStart();
            combatState = true;
            // turnSystem.NextTurn();
        }
    }

    private void CombatEnabled() {
        if (!combatState) EventManager.RaiseCombatStart();
        combatState = true;
        combatants++;
    }

    // private void ClearTurnSystem() {
    //     if (combatState) {
    //         combatants = 0;
    //         combatState = false;
    //         // Destroy(turnSystem);
    //         // Debug.Log("Turn System deleted.");
    //     }
    // }

    private void EnemyDeath() {
        if (combatState) {
            combatants--;
            if (combatants <= 0) {
                EventManager.RaiseCombatOver();
                combatState = false;
                combatants = 0;
            }
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

    public static bool GameMovementStopped() {
        return combatState && Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0;
    }

    private void ShowObjective() {

    }

    private IEnumerator ImageDelayAndShow(GameObject imageObject) {
        yield return new WaitForSecondsRealtime(1.5f);
        imageObject.GetComponent<Image>().enabled = true;
        yield return new WaitForSecondsRealtime(2.5f);
        imageObject.GetComponent<Image>().enabled = false;
    }

}
