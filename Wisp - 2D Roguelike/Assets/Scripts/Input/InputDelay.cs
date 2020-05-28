using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDelay {
    
    private const float delayDuration = 0.75f;
    private float inputDelay = delayDuration;

    // Counts down the time until available to act
    public void DelayTick() {
        if (inputDelay > 0) inputDelay -= Time.deltaTime;
    }

    // Resets the turn clock
    public void ResetInputDelay() {
        inputDelay = delayDuration;
    }

    // Returns true if available to act
    public bool CanAct() {
        return inputDelay <= 0;
    }
}
