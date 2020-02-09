using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Actor : MonoBehaviour {

    private int health;
    private MovementComponent movement;


    private void Awake() {
        movement = new MovementComponent();
    }

}