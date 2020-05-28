using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrownCooldown : MonoBehaviour {

    private Image image;
    private PlayerController player;

    private int duration = 200;

    private bool combatActive = false;
    private bool isActive = false;

    private bool coolingDown = false;
    private int countdown = 0;

    private void OnEnable() {
        EventManager.combatStart += CombatIsActive;
        EventManager.combatOver += CombatIsNotActive;
        EventManager.playerMoving += SetIsActive;
        EventManager.playerStopped += SetIsNotActive;
    }

    private void OnDisable() {
        EventManager.combatStart -= CombatIsActive;
        EventManager.combatOver -= CombatIsNotActive;
        EventManager.playerMoving -= SetIsActive;
        EventManager.playerStopped -= SetIsNotActive;
    }

    private void Awake() {
        image = GetComponent<Image>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    private void FixedUpdate() {
        if (coolingDown && (!combatActive || isActive)) {
            countdown--;
        }
        if (coolingDown && countdown <= 0) {
            countdown = duration;
            RefreshCooldown();
        }
    }

    private void Update() {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonUp(0)) {
            OnCooldown();
        }
    }

    private void OnCooldown() {
        // if (coolingDown) {
        //     // StopAllCoroutines();
        //     player.TakeDamage(1);
        // }
        // else {
        if (!coolingDown) {
            // countdown = duration;
            // image.color = Color.gray;
            // coolingDown = true;
            // EventManager.RaiseThrownOnCooldown();
            StartCoroutine(SlightWait());
        }
        
        // StartCoroutine(RefreshCooldown());
    }

    private void RefreshCooldown() {
        image.color = Color.white;
        coolingDown = false;
        EventManager.RaiseThrownOffCooldown();
    }

    IEnumerator SlightWait() {
        yield return new WaitForSecondsRealtime(0.001f);
        countdown = duration;
        image.color = Color.gray;
        coolingDown = true;
        EventManager.RaiseThrownOnCooldown();
    }

    // IEnumerator RefreshCooldown() {
    //     yield return new WaitForSeconds(1.5f);
    //     image.color = Color.white;
    //     coolingDown = false;
    // }

    private void CombatIsActive() {
        combatActive = true;
    }

    private void CombatIsNotActive() {
        combatActive = false;
    }

    private void SetIsActive() {
        isActive = true;
    }

    private void SetIsNotActive() {
        isActive = false;
    }

}
