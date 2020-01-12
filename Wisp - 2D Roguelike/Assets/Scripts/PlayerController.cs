using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private CombatComponent combat;
    [SerializeField]
    private MovementComponent movement;

    void Awake() {
        combat = new CombatComponent(3, 1);
    }

    private void Update() {
        if (!this.combat.IsAlive()) {
            Destroy(this.transform.parent.gameObject);
        }
    }

    public CombatComponent Combat() {
        return this.combat;
    }

}
