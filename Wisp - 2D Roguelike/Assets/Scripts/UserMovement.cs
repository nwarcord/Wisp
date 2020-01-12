using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UserMovement : MonoBehaviour {

    public Grid grid; // You can also use the Tilemap object
    public Tilemap floor;
    public GameObject player;
    private Transform playerPosition;

    public void Awake() {
        playerPosition = player.GetComponent<Transform>();
    }

    public void Update() {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int coordinate = grid.WorldToCell(mouseWorldPos);
        Vector3Int playerCoordinate = grid.WorldToCell(playerPosition.position);
        playerCoordinate.y -= 1;

        Vector2Int tileDistance = TileDistance(coordinate, playerCoordinate);

        if (Input.GetMouseButtonUp(0)) {

            if (Mathf.Abs(tileDistance.x) <= 1 && Mathf.Abs(tileDistance.y) <= 1 && floor.GetTile(coordinate)) {
                playerPosition.position = new Vector3(playerPosition.position.x + tileDistance.x, playerPosition.position.y + tileDistance.y, 0);
            }
        }
    }

    private Vector2Int TileDistance(Vector3Int clickedTile, Vector3Int playerTile) {
        return new Vector2Int(clickedTile.x - playerTile.x, clickedTile.y - playerTile.y);
    }

    private bool CoordInRange(float coord1, float coord2) {
        return Mathf.Abs(coord1 - coord2) <= 1;
    }

}
