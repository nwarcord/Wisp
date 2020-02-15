using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour, ITurnAct {

    public GameObject player; // User controlled GameObject
    private MovementComponent playerMovement;
    private CombatComponent playerCombat;
    // private InputDelay inputDelay;
    private const float delayDuration = 0.75f;
    private const float shortDelay = 0.5f;
    private float inputDelay = delayDuration;
    private bool inputEnabled = true; // User input flag
    private bool actionTaken = false; // For turn coroutine
    public Projectile arrows;

    // Keybindings
    private KeyCode activate = KeyCode.E;
    private KeyCode attack = KeyCode.Q;
    private KeyCode up = KeyCode.W;
    private KeyCode down = KeyCode.S;
    private KeyCode left = KeyCode.A;
    private KeyCode right = KeyCode.D;
    private KeyCode ranged = KeyCode.LeftShift;

    // ----------------------------------------------------------------
    // Initialization    
    // ----------------------------------------------------------------

    // Add listeners
    private void OnEnable() {
        // Pause constant-enables user input during combat
        EventManager.playerEntersCombat += DisableInput;
        EventManager.playerLeftCombat += EnableInput;
    }

    // Remove listeners
    private void OnDisable() {
        EventManager.playerEntersCombat -= DisableInput;
        EventManager.playerLeftCombat -= EnableInput;
    }

    // Using Start instead of Awake to ensure Player is initialized first
    public void Start() {
        playerMovement = player.GetComponent<PlayerController>().GetMovement();
        playerCombat = player.GetComponent<PlayerController>().Combat();
    }

    // ----------------------------------------------------------------
    // Each frame update
    // ----------------------------------------------------------------

    public void Update() {
        
        if (inputDelay > 0) {
            inputDelay -= Time.deltaTime;
        }

        else if (inputEnabled) {
                
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // if (Input.GetKey(attack) && Input.GetMouseButtonUp(0)) {
            if (AttackAction()) {
                if (playerCombat.OneTileAttack(Input.mousePosition)) {
                    ResetInputDelay();
                }
            }
            else if (RangedAttackAction()) {
                if (playerCombat.RangedAttack(mouseWorldPos, arrows)) {
                    ResetInputDelay();
                }
            }
            // else if (Input.GetMouseButtonUp(0)) {
            // else if (LeftClick()) {
            else if (MoveAction()) {
                // playerMovement.AttemptMove(mouseWorldPos);
                ResetInputDelay();
            }

        }
    }

    // ----------------------------------------------------------------
    // Turn Actions
    // ----------------------------------------------------------------
    
    private void ResetInputDelay() {
        if (playerCombat.inCombat) {
            inputDelay = delayDuration;
        }
        else {
            inputDelay = shortDelay;
        }
        // inputDelay = delayDuration;
        actionTaken = true;
    }

    // ----------------------------------------------------------------
    // Set input flag
    // ----------------------------------------------------------------

    private void DisableInput() {
        inputEnabled = false;
        actionTaken = false;
    }

    private void EnableInput() {
        inputEnabled = true;
        actionTaken = false;
    }

    // ----------------------------------------------------------------
    // Key Events
    // ----------------------------------------------------------------

    private bool AttackAction() {
        return Input.GetKey(attack) && LeftClick();
    }

    private bool LeftClick() {
        return Input.GetMouseButtonUp(0);
    }

    private bool MoveAction() {
        if (Input.GetKey(up))
            return playerMovement.AttemptMove(MoveDirection.Up);
        else if (Input.GetKey(down))
            return playerMovement.AttemptMove(MoveDirection.Down);
        else if (Input.GetKey(right))
            return playerMovement.AttemptMove(MoveDirection.Right);
        else if (Input.GetKey(left))
            return playerMovement.AttemptMove(MoveDirection.Left);
        else return false;
    }

    private bool InteractAction() {
        return Input.GetKey(activate);
    }

    private bool RangedAttackAction() {
        return Input.GetKey(ranged) && LeftClick();
    }

    // ----------------------------------------------------------------
    // Turn Actions
    // ----------------------------------------------------------------

    public void TakeTurn() {
        EnableInput();
        StartCoroutine(WaitForAction());
    }

    bool ParseTurn() {
        return actionTaken;
    }

    private IEnumerator WaitForAction() {

        while(!ParseTurn()) {
            yield return null;
        }

        DisableInput();

        EventManager.RaiseActorTurnOver();

    }

}