using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [SerializeField]
    private Enemy enemyType = default;
    private BoxCollider2D spawnCollider;
    private Vector3 position;
    private bool spawnerBlocked;

    private void OnEnable() {
        EventManager.enemyDeath += SpawnEnemy;
    }

    private void OnDisable() {
        EventManager.enemyDeath -= SpawnEnemy;
    }

    void Awake() {
        spawnCollider = this.GetComponent<BoxCollider2D>();
        position = this.transform.position;
        spawnerBlocked = false;
        SpawnEnemy();
    }


    // Update is called once per frame
    // void Update() {}

    public void SpawnEnemy() {
        if (!spawnerBlocked) {
            Enemy spawn = GameObject.Instantiate(enemyType, position, new Quaternion());
            if (GameState.combatState) EventManager.RaiseCombatSpawn(spawn);
        }
        spawnerBlocked = true;
    }

    private void OnTriggerExit2D(Collider2D other) {
        spawnerBlocked = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        spawnerBlocked = true;
    }

}
