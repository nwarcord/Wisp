using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour {

    public GameObject player;
    private MovementComponent playerMovement;
    private const float delayDuration = 0.75f;
    private float inputDelay = delayDuration;

    public void Awake() {
        playerMovement = player.GetComponent<MovementComponent>();
    }

    public void Update() {
        
        if (inputDelay > 0) {
            inputDelay -= Time.deltaTime;
        }
        else {

            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonUp(0)) {
                playerMovement.AttemptMove(mouseWorldPos);
                ResetInputDelay();
            }

        }
    }

    private void ResetInputDelay() {
        inputDelay = delayDuration;
    }

}
