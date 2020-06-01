using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour {
    
    [SerializeField]
    private Canvas levelSelect = default;
    [SerializeField]
    private Button level1 = default;
    [SerializeField]
    private Button level2 = default;

    private bool loading = true;

    private void OnEnable() {
        level1.onClick.AddListener(delegate { LevelPlay(1); });
        level2.onClick.AddListener(delegate { LevelPlay(2); });
        level2.gameObject.GetComponent<Image>().color = Color.gray;
    }

    private void Awake() {
        levelSelect.enabled = false;
    }
    private void Start() {
        StartCoroutine(MenuScreenLoad());
    }

    private void Update() {
        if (!loading && Input.anyKey) {
            levelSelect.enabled = true;
        }
        if (levelSelect.enabled && Input.GetKey(KeyCode.Escape)) {
            Application.Quit();
        }
    }

    IEnumerator MenuScreenLoad() {
        yield return new WaitForSeconds(2f);
        loading = false;
    }

    private void LevelPlay(int num) {
        SceneManager.LoadScene(num);
    }

}
