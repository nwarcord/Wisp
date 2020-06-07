using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class GameState : MonoBehaviour {

    // private const int uniqueEnemies = 3;

    private GameObject player;
    // private TurnSystem turnSystem;
    public static Grid grid;
    public static bool combatState = false;
    private static int combatants = 0;
    [SerializeField]
    private GameObject objectivePrompt = default;
    [SerializeField]
    private GameObject winPrompt = default;
    [SerializeField]
    private GameObject gameOverPrompt = default;
    private int currentEnemies = 0;

    // private FloatParameter normalColor = new FloatParameter {value = 0f};
    // private FloatParameter grayscaleColor = new FloatParameter {value = -100f};

    private bool isGray = false;

    private ColorGrading cameraColor;

    private bool gameEnding = false;

    private void OnEnable() {
        // EventManager.aggroPlayer += InitTurnSystem;
        EventManager.aggroPlayer += CombatEnabled;
        // EventManager.combatOver += ClearTurnSystem;
        EventManager.enemyDeath += EnemyDeath;
        SceneManager.activeSceneChanged += UpdateGrid;
        EventManager.playerDied += GameOver;
        EventManager.levelComplete += LevelComplete;
        EventManager.playerMoving += SetCameraColorNormal;
        EventManager.playerStopped += SetCameraGrayscale;
    }

    private void OnDisable() {
        // EventManager.aggroPlayer -= InitTurnSystem;
        EventManager.aggroPlayer -= CombatEnabled;
        // EventManager.combatOver -= ClearTurnSystem;
        EventManager.enemyDeath -= EnemyDeath;
        SceneManager.activeSceneChanged -= UpdateGrid;
        EventManager.playerDied -= GameOver;
        EventManager.levelComplete -= LevelComplete;
        EventManager.playerMoving -= SetCameraColorNormal;
        EventManager.playerStopped -= SetCameraGrayscale;
    }

    void Awake() {
        combatState = false;
        combatants = 0;
        player = GameObject.FindWithTag("Player");
        objectivePrompt.GetComponent<Image>().enabled = false;
        winPrompt.GetComponent<Image>().enabled = false;
        gameOverPrompt.GetComponent<Image>().enabled = false;
        GameObject.FindWithTag("EffectCamera").GetComponent<PostProcessVolume>().profile.TryGetSettings(out cameraColor);
        IgnoreSpawnerColliders();
        InitGrid();
        // DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        StartCoroutine(ImageDelayAndShow(objectivePrompt));
        SetCurrentEnemies();
        // Debug.Log("Enemies in scene: " + currentEnemies);
        // Debug.Log("Combat State: " + combatState);
        // Debug.Log("Combatants: " + combatants);
    }

    private void Update() {
        if (currentEnemies == 0) {
            // EventManager.RaiseLevelComplete();
            LevelComplete();
        }
        if (Input.GetKey(KeyCode.Escape)) {
            LoadMainMenu();
        }
        // SetCameraGrayscale(GameMovementStopped());
        if (!combatState && isGray) SetCameraColorNormal();
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
        currentEnemies--;
        Debug.Log("Enemy killed | Enemies remaining: " + currentEnemies);
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

    private void LevelComplete() {
        if (gameEnding) return;
        gameEnding = true;
        StartCoroutine(ImageDelayAndEnd(winPrompt));
    }

    private void GameOver() {
        if (gameEnding) return;
        gameEnding = true;
        StartCoroutine(ImageDelayAndEnd(gameOverPrompt));
        player.SetActive(false);
        // GameObject.Find("UserInput").SetActive(false);
    }

    private IEnumerator ImageDelayAndShow(GameObject imageObject) {
        yield return new WaitForSecondsRealtime(1.5f);
        imageObject.GetComponent<Image>().enabled = true;
        yield return new WaitForSecondsRealtime(2.5f);
        imageObject.GetComponent<Image>().enabled = false;
    }

    private IEnumerator ImageDelayAndEnd(GameObject imageObject) {
        yield return new WaitForSecondsRealtime(1.5f);
        imageObject.GetComponent<Image>().enabled = true;
        yield return new WaitForSecondsRealtime(2.5f);
        // imageObject.GetComponent<Image>().enabled = false;
        // UnityEditor.EditorApplication.isPlaying = false;
        LoadMainMenu();
    }

    private void SetCurrentEnemies() {
        // currentEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        // Enemy[] enemies = Resources.FindObjectsOfTypeAll<Enemy>();
        // foreach (Enemy enemy in enemies) Debug.Log("Enemy -> " + enemy);
        currentEnemies = Resources.FindObjectsOfTypeAll<Enemy>().Length;
    }

    private void LoadMainMenu() {
        combatants = 0;
        combatState = false;
        SceneManager.LoadScene(0);
    }

    private void SetCameraGrayscale() {
        // if (state && cameraColor.saturation.value != -100f) {
            isGray = true;
            StopCoroutine(FadeToColor());
            StartCoroutine(FadeToGray());
            // cameraColor.saturation.value = -100f;
            // isGray = true;
        // }
        // else if (!state && cameraColor.saturation.value != 0f) {
        //     cameraColor.saturation.value = 0f;
        // }
    }

    private void SetCameraColorNormal() {
        isGray = false;
        StopCoroutine(FadeToGray());
        StartCoroutine(FadeToColor());
            // cameraColor.saturation.value = 0f;
            // isGray = false;
    }

    IEnumerator FadeToGray() {
        while (cameraColor.saturation.value > -99.99f && isGray) {
            cameraColor.saturation.value -= Time.deltaTime;
            yield return null;
        }
        if (cameraColor.saturation.value < -99f) cameraColor.saturation.value = -100f;
        yield return null;
    }

    IEnumerator FadeToColor() {
        while (cameraColor.saturation.value < 0.01f && !isGray) {
            cameraColor.saturation.value += 1;
            yield return null;
        }
        if (cameraColor.saturation.value > 0.001f) cameraColor.saturation.value = 0f;
        yield return null;
    }

}
