using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooMonsterAI : MonoBehaviour, ITurnAct, ICanBeDamaged {

    private int health;
    private CombatComponent combat;
    private MovementComponent movement;

    // Start is called before the first frame update
    void Awake() {
        health = 2;
        // combat = new CombatComponent(1);
        // movement = new MovementComponent();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void TakeTurn() {
        // return true;
    }

    // public bool MyTurn() {
    //     return false;
    // }

    public bool TakeDamage(int damage) {
        health -= damage;
        return true;
    }

    public bool IsAlive() {
        return health > 0;
    }

    public IEnumerator TurnRoutine() {
        yield return null;
    }

}
