using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour {

    public GameObject player; // User controlled GameObject
    private PlayerController playerController;
    private PlayerCombatComponent playerCombat;
    private const float delayDuration = 25f;
    private float inputDelay = delayDuration;
    private bool attackEnabled = true;
    private Vector2 direction = Vector2.zero;
    private float speed = 5.0f;
    private Rigidbody2D rb2D;
    private bool playerMoving = false;
    private bool isAttacking = false;
    private MeleeAttackSprite meleeSprite;

    private bool thrownAvailable = true;

    // Keybindings
    private KeyCode activate = KeyCode.E;
    private KeyCode thrown = KeyCode.LeftControl;
    private KeyCode aoe = KeyCode.Z;
    private KeyCode ranged = KeyCode.LeftShift;

    // ----------------------------------------------------------------
    // Initialization    
    // ----------------------------------------------------------------

    // Add listeners
    private void OnEnable() {
        EventManager.thrownOffCooldown += ThrownIsAvailable;
        EventManager.thrownOnCooldown += ThrownIsUnavailable;
    }

    // Remove listeners
    private void OnDisable() {
        EventManager.thrownOffCooldown -= ThrownIsAvailable;
        EventManager.thrownOnCooldown -= ThrownIsUnavailable;
    }

    // Using Start instead of Awake to ensure Player is initialized first
    public void Start() {
        playerController = player.GetComponent<PlayerController>();
        playerCombat = playerController.Combat();
        rb2D = player.GetComponent<Rigidbody2D>();
        meleeSprite = player.transform.GetChild(0).GetComponent<MeleeAttackSprite>();
    }

    private void FixedUpdate() {
        if (!attackEnabled) inputDelay--;
        if (inputDelay <= 0) attackEnabled = true;
    }

    // ----------------------------------------------------------------
    // Each frame update
    // ----------------------------------------------------------------

    public void Update() {
        if (!player.activeInHierarchy) return;

        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");
        Vector2 movement = new Vector2(horizontal, vertical);
        movement.Normalize();

        // Red Light. Green Light.
        if (movement != Vector2.zero) { // If user pressing move input
            if (!playerMoving && playerCombat.inCombat) { // If player wasn't moving previously and they're in combat
                EventManager.RaisePlayerMoving(); // Let everyone know player is now moving
            }
            direction = movement; // Move player
            playerMoving = true; // Flag player as moving
        }
        else { // If user isn't pressing move input
            if (playerMoving && playerCombat.inCombat) { // If player was moving previously and they're in combat
                EventManager.RaisePlayerStopped(); // Let everyone know player stopped
            }
            else if (playerCombat.inCombat && isAttacking && !playerMoving) {
                EventManager.RaisePlayerMoving();
            }
            else if (playerCombat.inCombat && !isAttacking && !playerMoving) {
                EventManager.RaisePlayerStopped();
            }
            playerMoving = false; // Flag player as stopped
        }

        // If player is in combat, is attacking, and wasn't previously moving = Signal player moving
        // If player is in combat, was previously attacking, and isn't moving = Signal player stopped

        rb2D.velocity = movement * speed;        

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if (attackEnabled) {
            if (RangedAttackAction()) {
                if (playerCombat.PerformAttack(mouseWorldPos, AttackType.Ranged)) {
                    playerController.PlayRangedAttack();
                    ResetAttackDelay();
                    isAttacking = true;
                    StartCoroutine(Attacking());
                }
            }
            else if (ThrownAttackAction()) {
                if (playerCombat.PerformAttack(mouseWorldPos, AttackType.Thrown)) {
                    ResetAttackDelay();
                    isAttacking = true;
                    StartCoroutine(Attacking());
                }
            }
            else if (AoeAttackAction()) {
                if (playerCombat.PerformAttack(mouseWorldPos, AttackType.Aoe)) {
                    ResetAttackDelay();
                    isAttacking = true;
                    StartCoroutine(Attacking());
                }
            }
            else if (AttackAction()) {
                meleeSprite.SpawnOrientation(player.transform.position, mouseWorldPos);
                playerController.PlayMeleeAttack();
                if (playerCombat.PerformAttack(mouseWorldPos, AttackType.Melee)) {
                    ResetAttackDelay();
                    isAttacking = true;
                    StartCoroutine(Attacking());
                }
            }
        }

    }

    // ----------------------------------------------------------------
    // Turn Actions
    // ----------------------------------------------------------------

    private void ResetAttackDelay() {
        inputDelay = delayDuration;
        attackEnabled = false;
    }

    // ----------------------------------------------------------------
    // Key Events
    // ----------------------------------------------------------------

    private bool AttackAction() {
        return LeftClick();
    }

    private bool LeftClick() {
        return Input.GetMouseButtonDown(0);
    }

    private bool InteractAction() {
        return Input.GetKey(activate);
    }

    private bool RangedAttackAction() {
        return Input.GetKey(ranged) && LeftClick();
    }

    private bool ThrownAttackAction() {
        return Input.GetKey(thrown) && LeftClick() && thrownAvailable;
    }

    private bool AoeAttackAction() {
        return Input.GetKey(aoe) && LeftClick();
    }

    // ----------------------------------------------------------------
    // Turn Actions
    // ----------------------------------------------------------------

    private void PerformAttack() {
        isAttacking = true;
        StartCoroutine(Attacking());
    }

    private IEnumerator Attacking() {
        yield return new WaitForSeconds(.2f);
        isAttacking = false;
    }

    public bool PlayerIsMoving() {
        return playerMoving;
    }

    private void ThrownIsAvailable() {
        thrownAvailable = true;
    }

    private void ThrownIsUnavailable() {
        thrownAvailable = false;
    }

}