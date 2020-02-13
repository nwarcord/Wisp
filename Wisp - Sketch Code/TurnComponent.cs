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

/*
    Turn component
        
        On initialization - Listens for a broadcast of player being in combat
        On destruction - Removes the reference for the broadcast listener

        When broadcast is received, turn component starts its turn behavior
        Else, turns are not used (input delay will have to be handled elsewhere)

        ----------------------------------

        Outside of combat, everything moves relative to its input delay
        When the player is in combat, everything moves relative to their turn order
        The player is a BLOCK in this system, holding up the turn order while they decide
        how to act. Once the player acts, the turn order can recommence.

        ----------------------------------

        Input delay should be a separate component, used in the User Input, AI think behaviors,
        and the Turn System itself.
        Turn system will have to have a way to throttle player input while it isn't the
        player's turn. A starting point would be to have a check for when combat is in progress,
        which (along with the input delay) would throttle the player.

        The Turn System will notify the actor when it is their turn, allowing them to initiate
        their turn behavior.
        This Turn System can then keep track of the number of combatants, "releasing" the game
        from the combat behavior when that value reaches 0 or 1 (depending if the player is
        counted in that number or not).
        Objects that are combat related (for example, projectiles) will also need a spot in
        the turn order, but will NOT be considered combatants.

        NEED a way for Turn System to be initialized and for new objects / actors to be placed
        in the turn order (sometimes in-between preexisting ones). Double Linked list as queue?
        NEED for the Turn System to notify everything that combat is over once resolved.

        Turn System does not need to be static.
        Turn System does not need to be globally available.
        NEED a place for Turn System to be initialized.

*/