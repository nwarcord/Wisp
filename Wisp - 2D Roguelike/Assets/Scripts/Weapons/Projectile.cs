using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, ITurnAct {

    [SerializeField]
    private int baseDamage;
    [SerializeField]
    private int tileMovePerTurn;
    [SerializeField]
    private bool isContinuous;
    private BoxCollider2D boxCollider;
    private MovementComponent movement;
    private TurnComponent turnComponent;

    void Awake() {
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        turnComponent = new TurnComponent();
        movement = new MovementComponent(gameObject, this, GameObject.FindWithTag("Grid").GetComponent<Grid>());
    }

    void Update() {
        TakeTurn();
    }

    public void TakeTurn() {
        // bool isTurn = MyTurn();
        // if (isTurn) {
        //     return;
        // }
    }

    // public bool MyTurn() {
    //     turnComponent.TurnTick();
    //     if (Input.GetMouseButtonUp(0) && turnComponent.CanAct()) {
    //         return true;
    //     }
    //     return false;
    // }

    private void OnTriggerEnter2D(Collider2D other) {
        ICanBeDamaged victim = other.gameObject.GetComponent<ICanBeDamaged>();
        if (victim != null) {
            victim.TakeDamage(baseDamage);
        }
        if (!isContinuous) {
            Destroy(gameObject);
        }
    }

}
