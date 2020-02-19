using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script to attach to objects, allowing movement in a grid-like pattern.
// When changing scenes (or Grid object) the grid of this component will have to be updated.

public enum MoveDirection { Up, Down, Left, Right }

public class MovementComponent {

    // private float moveTime = 0.1f;       // Time it will take object to move, in seconds.
    private float moveTime = 0.05f;       // Time it will take object to move, in seconds.
    private LayerMask obstructionLayer;  // Layer on which collision will be checked.
    private LayerMask characterLayer;

    private GameObject actor;
    private BoxCollider2D boxCollider;  // The BoxCollider2D component attached to this object.
    private Rigidbody2D rb2D;           // The Rigidbody2D component attached to this object.
    private Transform actorTransform;
    private Grid grid;
    private MonoBehaviour mb;
    private CircleCollider2D finalPosition;

    private float inverseMoveTime;      // Used to make movement more efficient.

    // ----------------------------------------------------------------

    public MovementComponent(GameObject actor, MonoBehaviour mb, Grid grid) {
        this.actor = actor;
        inverseMoveTime = 1f / moveTime;
        obstructionLayer = LayerMask.GetMask("Obstructions");
        characterLayer = LayerMask.GetMask("Characters");
        boxCollider = actor.GetComponent<BoxCollider2D>();
        InitFinalPosition();
        rb2D = actor.GetComponent<Rigidbody2D>();
        actorTransform = actor.transform;
        this.grid = grid;
        this.mb = mb;
    }

    private void InitFinalPosition() {
        finalPosition = actor.GetComponent<CircleCollider2D>();
        Physics2D.IgnoreCollision(boxCollider, finalPosition);
        finalPosition.radius = 0.5f;
        ResetFinalPosition();
    }

    // ----------------------------------------------------------------

    public bool AttemptMove(Vector3 moveTo) {
        
        // Boxcollider has no offset when rigidbody has height of one
        bool heightIsOne = boxCollider.offset.y == 0;

        // Get actor position and end position in cell coordinate form
        Vector3Int coordinate = grid.WorldToCell(moveTo);
        Vector3Int playerCoordinate = grid.WorldToCell(actorTransform.position);
        
        // Add correction for objects with height of two
        if (!heightIsOne) {
            playerCoordinate.y -= 1;
        }

        // Distance between actor and desired end position
        Vector2Int tileDistance = TileDistance(coordinate, playerCoordinate);
        
        // Execute if distance is within one tile
        if (Mathf.Abs(tileDistance.x) <= 1 && Mathf.Abs(tileDistance.y) <= 1) {

            // End position in world coordinate form (for when actually moving the object)
            Vector2 endPos = new Vector2(actorTransform.position.x + tileDistance.x, actorTransform.position.y + tileDistance.y);
            // The starting position for the linecast that will detect collisions
            Vector2 startCheck = new Vector2(actorTransform.position.x, actorTransform.position.y - 0.5f);
            if (heightIsOne) {
                startCheck = new Vector2(actorTransform.position.x, actorTransform.position.y);
            }

            // Calculate the end of linecast by adding distance to the start
            Vector2 endCheck = startCheck + tileDistance;

            // Detect collision, ignoring the objects own colliders
            EnableColliders(false);
            RaycastHit2D hit = Physics2D.Linecast(startCheck, endCheck, obstructionLayer);
            if (hit.transform == null) {
                hit = Physics2D.Linecast(startCheck, endCheck, characterLayer);
            }
            EnableColliders(true);

            // If no collision, then start coroutine for movement
            if (hit.transform == null) {
                mb.StartCoroutine(SmoothMovement(endPos));
                return true;
            }
        }
        return false;
    }

    public bool AttemptMove(MoveDirection direction) {
        Vector3 pos = actorTransform.position;
        pos.y -= 0.5f;
        if (direction == MoveDirection.Up) pos.y++;
        else if (direction == MoveDirection.Down) pos.y--;
        else if (direction == MoveDirection.Right) pos.x++;
        else if (direction == MoveDirection.Left) pos.x--;
        else { Debug.LogError("Invalid move direction"); }
        return AttemptMove(pos);
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
            // Set placeholder collider to save spot
            finalPosition.offset = new Vector2(end.x - actorTransform.position.x + boxCollider.offset.x, end.y - actorTransform.position.y + boxCollider.offset.y);

            // Return and loop until sqrRemainingDistance is close enough to zero to end the function
            yield return null;
        }

        ResetFinalPosition();

    }

    // ----------------------------------------------------------------

    private Vector2Int TileDistance(Vector3Int clickedTile, Vector3Int playerTile) {
        return new Vector2Int(clickedTile.x - playerTile.x, clickedTile.y - playerTile.y);
    }

    // ----------------------------------------------------------------

    public void UpdateGrid(Grid newGrid) {
        this.grid = newGrid;
    }

    private void ResetFinalPosition() {
        finalPosition.offset = boxCollider.offset;
    }

    private void EnableColliders(bool state) {
        boxCollider.enabled = state;
        finalPosition.enabled = state;
    }

}
