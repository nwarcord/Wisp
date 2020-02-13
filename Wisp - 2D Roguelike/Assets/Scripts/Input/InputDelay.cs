using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDelay {
    
    private const float delayDuration = 1.25f;
    private float inputDelay = delayDuration;
    // private bool available = false;

    // Counts down the time until available to act
    // private void DelayTick() {
    //     if (inputDelay > 0) {
    //         inputDelay -= Time.deltaTime;
    //     }
    //     else {
    //         available = true;
    //     }
    // }

    public void DelayTick() {
        if (inputDelay > 0) inputDelay -= Time.deltaTime;
    }

    // Resets the turn clock
    // private void ResetInputDelay() {
    //     inputDelay = delayDuration;
    // }
    
    public void ResetInputDelay() {
        inputDelay = delayDuration;
    }

    // Returns true if available to act
    // If it is, then the state of the wait clock is reset
    // This assumes that the caller uses the boolean return value
    // public bool CanAct() {
    //     if (available) {
    //         ResetInputDelay();
    //         available = false;
    //         return true;
    //     }
    //     DelayTick();
    //     return available;
    // }
    public bool CanAct() {
        return inputDelay <= 0;
    }
}
