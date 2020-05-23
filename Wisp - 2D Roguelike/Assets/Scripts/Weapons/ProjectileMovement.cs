using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement {

// Script to attach to objects, allowing movement in a grid-like pattern.
// When changing scenes (or Grid object) the grid of this component will have to be updated.

    // private float moveTime = 0.1f;       // Time it will take object to move, in seconds.
    private float moveTime = 0.05f;       // Time it will take object to move, in seconds.
    private LayerMask obstructionLayer;  // Layer on which collision will be checked.
    private LayerMask characterLayer;
    private LayerMask playerLayer;

    private GameObject actor;
    private BoxCollider2D boxCollider;  // The BoxCollider2D component attached to this object.
    private Rigidbody2D rb2D;           // The Rigidbody2D component attached to this object.
    private Transform actorTransform;
    private Grid grid;
    private MonoBehaviour mb;

    private float inverseMoveTime;      // Used to make movement more efficient.

    // ----------------------------------------------------------------

    public ProjectileMovement(GameObject actor, MonoBehaviour mb, Grid grid) {
        this.actor = actor;
        inverseMoveTime = 1f / moveTime;
        obstructionLayer = LayerMask.GetMask("Obstructions");
        characterLayer = LayerMask.GetMask("Characters");
        playerLayer = LayerMask.GetMask("Player");
        boxCollider = actor.GetComponent<BoxCollider2D>();
        rb2D = actor.GetComponent<Rigidbody2D>();
        actorTransform = actor.transform;
        this.grid = grid;
        this.mb = mb;
    }

    // ----------------------------------------------------------------

    public bool AttemptMove(Vector3 moveTo) {
        
        // Get actor position and end position in cell coordinate form
        Vector3Int coordinate = grid.WorldToCell(moveTo);
        Vector3Int projectileCoordinate = grid.WorldToCell(actorTransform.position);

        // Distance between actor and desired end position
        Vector2Int tileDistance = TileDistance(coordinate, projectileCoordinate);
        
        // End position in world coordinate form (for when actually moving the object)
        Vector2 endPos = new Vector2(actorTransform.position.x + tileDistance.x, actorTransform.position.y + tileDistance.y);

        mb.StartCoroutine(SmoothMovement(endPos));
        return true;
    }

    // ----------------------------------------------------------------

    // Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
    private IEnumerator SmoothMovement (Vector3 end) {

        float sqrRemainingDistance = (actorTransform.position - end).sqrMagnitude;
        
        while (sqrRemainingDistance > float.Epsilon) {

            Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);

            // Call MovePosition on attached Rigidbody2D and move it to the calculated position.
            rb2D.MovePosition(newPostion);

            // Recalculate the remaining distance after moving.
            sqrRemainingDistance = (actorTransform.position - end).sqrMagnitude;

            // Return and loop until sqrRemainingDistance is close enough to zero to end the function
            yield return null;
        }

    }

    // ----------------------------------------------------------------

    private Vector2Int TileDistance(Vector3Int clickedTile, Vector3Int playerTile) {
        return new Vector2Int(clickedTile.x - playerTile.x, clickedTile.y - playerTile.y);
    }

    // ----------------------------------------------------------------

    public void UpdateGrid(Grid newGrid) {
        this.grid = newGrid;
    }

}
