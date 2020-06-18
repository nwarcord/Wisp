using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour {

    [SerializeField]
    private int healValue = 0;

    // If player collides with health pickup
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<PlayerController>().Heal(healValue);
            Destroy(gameObject);
        }
    }

}
