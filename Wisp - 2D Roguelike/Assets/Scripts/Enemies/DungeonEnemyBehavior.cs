using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEnemyBehavior : MonoBehaviour {

    public List<GameObject> enemies;
    public GameObject playerObject;
    private Vector3 playerPosition;
    private GameObject gooMonster;
    private MovementComponent gooMove;
    private ICanBeDamaged vitals;
    [SerializeField]
    private Grid grid = default;

    [SerializeField]
    private BoardManager boardManager;

    public void Awake() {
        playerPosition = playerObject.transform.position;
        gooMonster = GameObject.Instantiate(enemies[0], new Vector3(5.5f, 2.5f, 0), new Quaternion());
        gooMove = gooMonster.GetComponent<MovementComponent>();
        gooMove.UpdateGrid(grid);
        vitals = gooMonster.GetComponent<ICanBeDamaged>();
    }

    public void Update() {
        // if (!vitals.IsAlive()) {
        //     Destroy(gooMonster);
        //     gooMonster = GameObject.Instantiate(enemies[0], new Vector3(5.5f, 2.5f, 0), new Quaternion());
        // }
        
        if (Input.GetMouseButtonUp(0)) {
            playerPosition = playerObject.transform.position;
            playerPosition.y -= 0.5f;
            Vector3 move = gooMonster.transform.position;
            if (playerPosition.x != move.x) {
                if (playerPosition.x < move.x) {
                    move.x -= 1;
                }
                else {
                    move.x += 1;
                }
            }
            if (playerPosition.y != move.y) {
                if (playerPosition.y < move.y) {
                    move.y -= 1;
                }
                else {
                    move.y += 1;
                }
            }
            // if (boardManager.PlaceObjectOnBoard(gooMonster, move)) {
            //     gooMonster.transform.position = move;
            // }
            if (playerPosition != move)
                gooMove.AttemptMove(move);

        }
    }

    private bool PlayerAdjacent(Vector3 monster) {
        return Vector3.Magnitude(monster - this.playerPosition) <= 1;
    }

}
