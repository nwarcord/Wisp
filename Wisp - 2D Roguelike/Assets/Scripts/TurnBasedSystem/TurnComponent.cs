using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Turn component to be initialized on any GameObject that will need
its actions throttled.
*/
public class TurnComponent {

    private const float delayDuration = 0.75f;
    private const float nonCombatDelayDuration = 0.65f;
    private bool inCombat = false;
    private float inputDelay = delayDuration;
    private bool available = false;

    // Counts down the time until turn is available
    public void TurnTick() {
        if (inputDelay > 0) {
            inputDelay -= Time.deltaTime;
        }
        else {
            available = true;
        }
    }

    // Resets the turn clock
    private void ResetInputDelay() {
        if (inCombat) {
            inputDelay = delayDuration;
        }
        // inputDelay = delayDuration;
        else {
            inputDelay = nonCombatDelayDuration;
        }
    }

    // Returns true if turn is available
    // If it is, then the state of the turn clock is reset
    // This assumes that the caller uses the boolean return value
    public bool CanAct() {
        if (available) {
            ResetInputDelay();
            available = false;
            return true;
        }
        return available;
    }

    public void ToggleCombat(bool toggle) {
        inCombat = toggle;
        ResetInputDelay();
    }

}
