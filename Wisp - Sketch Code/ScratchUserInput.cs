using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour {//, ITurnAct {

    public GameObject player; // User controlled GameObject
    private MovementComponent playerMovement;
    private CombatComponent playerCombat;
    private InputDelay inputDelay;
    private bool inputEnabled = true; // User input flag

    // Keybindings
    // private KeyCode activate = KeyCode.E;
    // private KeyCode attack = KeyCode.Q;
    // private KeyCode up = KeyCode.W;
    // private KeyCode down = KeyCode.S;
    // private KeyCode left = KeyCode.A;
    // private KeyCode right = KeyCode.D;

    // ----------------------------------------------------------------
    // Initialization    
    // ----------------------------------------------------------------

    // Add listeners
    private void OnEnable() {
        // Pause constant-enables user input during combat
        // EventManager.playerEntersCombat += DisableInput;
        // EventManager.playerLeftCombat += EnableInput;
    }

    // Remove listeners
    private void OnDisable() {
        // EventManager.playerEntersCombat -= DisableInput;
        // EventManager.playerLeftCombat -= EnableInput;
    }

    // Using Start instead of Awake to ensure Player is initialized first
    public void Start() {
        inputDelay = new InputDelay();
        playerMovement = player.GetComponent<PlayerController>().GetMovement();
        playerCombat = player.GetComponent<PlayerController>().Combat();
    }

    // ----------------------------------------------------------------
    // Each frame update
    // ----------------------------------------------------------------

    public void Update() {
        
        if (inputEnabled && inputDelay.CanAct()) {
            
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetKey(KeyCode.Q) && Input.GetMouseButtonUp(0)) {
            // if (AttackAction()) {
                Debug.Log("Attack action");
                playerCombat.OneTileAttack(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0)) {
            // if (LeftClick()) {
                Debug.Log("Move action");
                playerMovement.AttemptMove(mouseWorldPos);
            }

        }
    }

    // ----------------------------------------------------------------
    // Set input flag
    // ----------------------------------------------------------------

    private void DisableInput() {
        inputEnabled = false;
    }

    private void EnableInput() {
        inputEnabled = true;
    }

    // ----------------------------------------------------------------
    // Key Events
    // ----------------------------------------------------------------

    private bool AttackAction() {
        // return Input.GetKey(attack) && Input.GetMouseButtonUp(0);
        return Input.GetKey(KeyCode.Q) && LeftClick();
    }

    private bool LeftClick() {
        return Input.GetMouseButtonUp(0);
    }

    // ----------------------------------------------------------------
    // Turn Actions
    // ----------------------------------------------------------------

    // public void TakeTurn() {
    //     StartCoroutine(WaitForAction());
    // }

    // bool ParseTurn() {
    //     if (inputEnabled && inputDelay.CanAct()) {

    //         Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //         if (Input.GetKey(attack) && Input.GetMouseButtonUp(0)) {
    //         // if (AttackAction()) {
    //             playerCombat.OneTileAttack(Input.mousePosition);
    //             return true;
    //         }
    //         else if (Input.GetMouseButtonUp(0)) {
    //         // if (LeftClick()) {
    //             playerMovement.AttemptMove(mouseWorldPos);
    //             return true;
    //         }

    //     }
    //     return false;
    // }

    // private IEnumerator WaitForAction() {

    //     while(!ParseTurn()) {
    //         yield return null;
    //     }

    //     EventManager.RaiseActorTurnOver();

    // }

}