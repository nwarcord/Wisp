using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class GameState : MonoBehaviour {

    private GameObject player;
    public static bool combatState = false;
    private static int combatants = 0;
    [SerializeField]
    private GameObject objectivePrompt = default;
    [SerializeField]
    private GameObject winPrompt = default;
    [SerializeField]
    private GameObject gameOverPrompt = default;
    private int currentEnemies = 0;

    private bool isGray = false;

    private ColorGrading cameraColor;

    private bool gameEnding = false;

    // ----------------------------------------------------------------
    // GameState Initialization
    // ----------------------------------------------------------------

    private void OnEnable() {
        EventManager.aggroPlayer += CombatEnabled;
        EventManager.enemyDeath += EnemyDeath;
        EventManager.playerDied += GameOver;
        EventManager.levelComplete += LevelComplete;
        EventManager.playerMoving += SetCameraColorNormal;
        EventManager.playerStopped += SetCameraGrayscale;
    }

    private void OnDisable() {
        EventManager.aggroPlayer -= CombatEnabled;
        EventManager.enemyDeath -= EnemyDeath;
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
    }

    private void Start() {
        StartCoroutine(ImageDelayAndShow(objectivePrompt));
        SetCurrentEnemies();
    }

    // ----------------------------------------------------------------
    // Frame-to-frame behavior
    // ----------------------------------------------------------------

    private void Update() {
        if (currentEnemies == 0) {
            LevelComplete();
        }
        if (Input.GetKey(KeyCode.Escape)) {
            LoadMainMenu();
        }
        if (!combatState && isGray) SetCameraColorNormal();
    }

    // ----------------------------------------------------------------
    // Combat State
    // ----------------------------------------------------------------

    private void CombatEnabled() {
        if (!combatState) EventManager.RaiseCombatStart();
        combatState = true;
        combatants++;
    }

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

    // ----------------------------------------------------------------
    // Level Initialization
    // ----------------------------------------------------------------

    private void IgnoreSpawnerColliders() {
        Physics2D.IgnoreLayerCollision(11, 8);
        // Physics2D.IgnoreLayerCollision(11, 9);
        Physics2D.IgnoreLayerCollision(11, 10);
    }

    public static bool GameMovementStopped() {
        return combatState && Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0;
    }

    // ----------------------------------------------------------------
    // Level and UI States
    // ----------------------------------------------------------------

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
        LoadMainMenu();
    }

    private void SetCurrentEnemies() {
        currentEnemies = Resources.FindObjectsOfTypeAll<Enemy>().Length;
    }

    private void LoadMainMenu() {
        combatants = 0;
        combatState = false;
        SceneManager.LoadScene(0);
    }

    // ----------------------------------------------------------------
    // Time-Stop Camera Effects
    // ----------------------------------------------------------------

    private void SetCameraGrayscale() {
        isGray = true;
        StopCoroutine(FadeToColor());
        StartCoroutine(FadeToGray());
    }

    private void SetCameraColorNormal() {
        isGray = false;
        StopCoroutine(FadeToGray());
        StartCoroutine(FadeToColor());
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
