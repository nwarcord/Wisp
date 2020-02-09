using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour {

    public GameObject player; // User controlled GameObject
    private MovementComponent playerMovement;
    private CombatComponent playerCombat;
    private InputDelay inputDelay;
    private bool inputEnabled = true; // User input flag

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
                playerCombat.OneTileAttack(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0)) {
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

}
