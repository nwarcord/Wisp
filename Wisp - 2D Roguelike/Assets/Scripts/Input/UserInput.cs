using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour, ITurnAct {

    public GameObject player; // User controlled GameObject
    private MovementComponent playerMovement;
    private PlayerCombatComponent playerCombat;
    private const float delayDuration = 0.5f;
    private float inputDelay = delayDuration;
    private bool inputEnabled = true; // User input flag
    private bool actionTaken = false; // For turn coroutine
    // private int frames = 0;

    // Keybindings
    private KeyCode activate = KeyCode.E;
    private KeyCode attack = KeyCode.Q;
    private KeyCode aoe = KeyCode.LeftControl; // TODO: Add functionality
    private KeyCode up = KeyCode.W;
    private KeyCode down = KeyCode.S;
    private KeyCode left = KeyCode.A;
    private KeyCode right = KeyCode.D;
    private KeyCode ranged = KeyCode.LeftShift;
    private KeyCode dodge = KeyCode.Space; // TODO: Add functionality

    // ----------------------------------------------------------------
    // Initialization    
    // ----------------------------------------------------------------

    // Add listeners
    private void OnEnable() {
        // Pause constant-enables user input during combat
        EventManager.combatStart += DisableInput;
        EventManager.combatOver += EnableInput;
    }

    // Remove listeners
    private void OnDisable() {
        EventManager.combatStart -= DisableInput;
        EventManager.combatOver -= EnableInput;
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
        // frames++;
        // if (frames >= 240) {
        //     frames = 0;
        //     Debug.Log("Hello from User Input! - Input enabled: " + inputEnabled);
        // }

        if (inputEnabled) {
            if (inputDelay > 0) {
                inputDelay -= Time.deltaTime;
            }

        else {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (AttackAction()) {
                    if (playerCombat.PerformAttack(Input.mousePosition, AttackType.Melee)) {
                        ResetInputDelay();
                    }
                }
                else if (RangedAttackAction()) {
                    if (playerCombat.PerformAttack(mouseWorldPos, AttackType.Ranged)) {
                        ResetInputDelay();
                    }
                }
                else if (AoeAttackAction()) {
                    // if (playerCombat.PerformAttack(mouseWorldPos, AttackType.Aoe)) {
                        // ResetInputDelay();
                    //}
                }
                else if (MoveAction()) {
                    ResetInputDelay();
                }
            }
        }
    }

    // ----------------------------------------------------------------
    // Turn Actions
    // ----------------------------------------------------------------
    
    private void ResetInputDelay() {
        inputDelay = delayDuration;
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

    private bool AoeAttackAction() {
        return Input.GetKey(aoe) && LeftClick();
    }

    // ----------------------------------------------------------------
    // Turn Actions
    // ----------------------------------------------------------------

    public void TakeTurn() {
        StartCoroutine(TurnRoutine());
    }

    bool ParseTurn() {
        return actionTaken;
    }

    private IEnumerator WaitForAction() {

        while(!ParseTurn()) {
            yield return null;
        }

        if (playerCombat.inCombat) { DisableInput(); }

    }

    public IEnumerator TurnRoutine() {
        EnableInput();
        yield return StartCoroutine(WaitForAction());
        EventManager.RaiseActorTurnOver();
    }

}