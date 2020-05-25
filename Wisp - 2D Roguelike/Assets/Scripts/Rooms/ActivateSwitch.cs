using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSwitch : MonoBehaviour {

    private RoomTrigger roomTrigger;

    private void Awake() {
        roomTrigger = GetComponent<RoomTrigger>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Trigger entered!");
        roomTrigger.enabled = false;
    }

}
