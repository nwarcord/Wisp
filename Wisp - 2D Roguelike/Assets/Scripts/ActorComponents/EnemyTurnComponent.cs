using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnComponent : TurnComponent {

    public InputDelay inputDelay;
    private CombatComponent combat;

    protected override void InitVariables() {
        this.combat = gameObject.GetComponent<CombatComponent>();
    }

    protected override void TurnBehavior() {
        // if (!combat.inCombat) combat.NonCombatBehavior();
        // else combat.CombatBehavior();   
    }
    
    public bool MyTurn() {
        inputDelay.DelayTick();
        if (inputDelay.CanAct()) {
            inputDelay.ResetInputDelay(); 
            return true;
        }
        return false;
    }

}
